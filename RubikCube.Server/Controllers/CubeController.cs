using Microsoft.AspNetCore.Mvc;
using RubikCube.Server.Models;
using RubikCube.Server.Models.Enums;
using RubikCube.Server.Models.Interfaces;

namespace RubikCube.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CubeController : ControllerBase
    {
        private readonly IRubikCube<Face> _rubikCube;
        private readonly ILogger<CubeController> _logger;

        public CubeController(ILogger<CubeController> logger, IRubikCube<Face> cube)
        {
            _logger = logger;
            _rubikCube = cube;
        }

        [HttpGet("GetCubeFaces", Name = "GetCubeFaces")]
        public ActionResult<IEnumerable<Face>> GetCubeFaces()
        {
            var faces = _rubikCube.GetFaces();
            if (faces == null || !faces.Any())
            {
                return NotFound("No faces found.");
            }
            return Ok(faces);
        }

        [HttpPost("Rotate", Name = "Rotate")]
        public ActionResult<IEnumerable<Face>> Rotate(int faceId, bool clockwise)
        {
            _rubikCube.RotateFace((Faces)faceId, clockwise);

            return GetCubeFaces();
        }

        [HttpPost("Reset", Name = "Reset")]
        public ActionResult<IEnumerable<Face>> Reset()
        {
            _rubikCube.Reset();

            return GetCubeFaces();
        }
    }
}
