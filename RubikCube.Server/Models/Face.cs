using RubikCube.Server.Models.Enums;
using RubikCube.Server.Models.Interfaces;

namespace RubikCube.Server.Models
{
    public record Face : IFace
    {
        public Face(Faces id, Color[][] colors)
        {
            Id = id;
            Colors = colors;
        }

        public Faces Id { get; set; }
        
        public Color[][] Colors { get; set; }
        
        public Color[] AffectedFacesColors { get; set; } = Array.Empty<Color>();
    }
}