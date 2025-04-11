using RubikCube.Server.Models.Enums;

namespace RubikCube.Server.Models.Interfaces
{
    public interface IRubikCube<TFace>
        where TFace : IFace
    {
        /// <summary>
        /// Resets the cube to its initial state.
        /// </summary>
        /// <param name="face"></param>
        /// <param name="clockwise"></param>
        void RotateFace(Faces face, bool clockwise);

        /// <summary>
        /// Gets the face of the cube by its identifier.
        /// </summary>
        /// <param name="face"></param>
        /// <returns></returns>
        TFace GetFace(Faces face);

        /// <summary>
        /// Gets all the faces of the cube.
        /// </summary>
        /// <returns></returns>
        IEnumerable<TFace> GetFaces();

        /// <summary>
        /// Resets the cube to its initial state.
        /// </summary>
        void Reset();
    }
}
