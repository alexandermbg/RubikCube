using RubikCube.Server.Models;
using RubikCube.Server.Models.Constants;
using RubikCube.Server.Models.Enums;
using RubikCube.Server.Models.Interfaces;

namespace RubikCube.Server.App
{
    public class Cube : IRubikCube<Face>
    {
        private readonly Face[] _faces = new Face[Constants.CubeFacesCount];

        public Cube()
        {
            Reset();
        }

        /// <summary>
        /// Resets the cube to its initial state.
        /// </summary>
        public void Reset()
        {
            for (int i = 0; i < _faces.Length; i++)
            {
                var face = (Faces)i;

                _faces[i] = new Face(face, GetNewColors((Colors)i, Constants.CubeDimensions));
            }

            foreach (var face in _faces)
            {
                face.AffectedFacesColors = GetAffectedColors(face.Id);
            }
        }

        /// <summary>
        /// Gets the face of the cube by its identifier.
        /// </summary>
        /// <param name="face"></param>
        /// <returns></returns>
        public Face GetFace(Faces face) => _faces.First(f => f.Id == face);

        /// <summary>
        /// Gets all the faces of the cube.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Face> GetFaces() => _faces;

        /// <summary>
        /// Rotates the specified face of the cube.
        /// </summary>
        /// <param name="face"></param>
        /// <param name="clockwise"></param>
        public void RotateFace(Faces face, bool clockwise)
        {
            var colors = GetFace(face).Colors;
            var copy = GetColorsCopy(colors);

            if (clockwise)
            {
                for (int i = 0; i < colors.Length; i++)
                {
                    for (int j = 0; j < colors.Length; j++)
                    {
                        colors[j][colors.Length - 1 - i].Value = copy[i][j].Value;
                    }
                }
            }
            else
            {
                for (int i = 0; i < colors.Length; i++)
                {
                    for (int j = 0; j < colors.Length; j++)
                    {
                        colors[colors.Length - 1 - j][i].Value = copy[i][j].Value;
                    }
                }
            }

            ShiftAffectedFaces(face, clockwise);
        }

        /// <summary>
        /// Shifts the affected faces colors after rotating a face.
        /// </summary>
        /// <param name="rotatedFace"></param>
        /// <param name="clockwise"></param>
        private void ShiftAffectedFaces(Faces rotatedFace, bool clockwise)
        {
            var affectedFacesColors = GetFace(rotatedFace).AffectedFacesColors;

            //always shift the colors in the same direction
            if (clockwise)
            {
                affectedFacesColors = affectedFacesColors.Reverse().ToArray();
            }

            Color[] firstBatchCopy = affectedFacesColors
                .Take(Constants.CubeDimensions)
                .Select(c => new Color(c.Value))
                .ToArray();

            for (int i = 0; i < affectedFacesColors.Length; i += Constants.CubeDimensions)
            {
                for (int j = i; j < i + Constants.CubeDimensions; j++)
                {
                    var index = Constants.CubeDimensions + j;
                    var indexOutOfRange = index >= affectedFacesColors.Length;

                    if (indexOutOfRange)
                    {
                        affectedFacesColors[j].Value = firstBatchCopy[index % affectedFacesColors.Length].Value;
                        continue;
                    }

                    affectedFacesColors[j].Value = affectedFacesColors[index].Value;
                }
            }
        }

        /// <summary>
        /// Gets a column of colors from all the face's colors.
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="colors"></param>
        /// <param name="rowReverse"></param>
        /// <returns></returns>
        private Color[] GetColumn(int columnIndex, Color[][] colors, bool rowReverse)
        {
            var column = new Color[colors.Length];

            for (int i = 0; i < colors.Length; i++)
            {
                column[i] = colors[i][columnIndex];
            }
            return rowReverse
                ? column.Reverse().ToArray()
                : column;
        }

        /// <summary>
        /// Gets a row of colors from all the face's colors.
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="colors"></param>
        /// <param name="colReverse"></param>
        /// <returns></returns>
        private Color[] GetRow(int rowIndex, Color[][] colors, bool colReverse)
        {
            var row = new Color[colors.Length];

            for (int i = 0; i < colors.Length; i++)
            {
                row[i] = colors[rowIndex][i];
            }

            return colReverse
                ? row.Reverse().ToArray()
                : row;
        }

        /// <summary>
        /// Creates a copy of the colors array.
        /// </summary>
        /// <param name="colors"></param>
        /// <returns></returns>
        private Color[][] GetColorsCopy(Color[][] colors)
        {
            var colorsCopy = new Color[colors.Length][];

            for (int i = 0; i < colors.Length; i++)
            {
                colorsCopy[i] = new Color[colors.Length];

                for (int j = 0; j < colors.Length; j++)
                {
                    colorsCopy[i][j] = new Color(colors[i][j].Value);
                }
            }

            return colorsCopy;
        }

        /// <summary>
        /// Creates a new colors array with the specified color and dimension.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="dimension"></param>
        /// <returns></returns>
        private Color[][] GetNewColors(Colors color, int dimension)
        {
            var colors = new Color[dimension][];
            for (int i = 0; i < dimension; i++)
            {
                colors[i] = new Color[dimension];

                for (int j = 0; j < dimension; j++)
                {
                    colors[i][j] = new Color(color);
                }
            }

            return colors;
        }

        /// <summary>
        /// Gets the affected colors of the specified face.
        /// </summary>
        /// <param name="face"></param>
        /// <returns></returns>
        private Color[] GetAffectedColors(Faces face)
        {
            var lastIndex = Constants.CubeDimensions - 1;

            switch (face)
            {
                case Faces.Left:
                    return [
                        ..GetColumn(0, GetFace(Faces.Upper).Colors, false),
                        ..GetColumn(0, GetFace(Faces.Front).Colors, false),
                        ..GetColumn(0, GetFace(Faces.Down).Colors, false),
                        ..GetColumn(lastIndex, GetFace(Faces.Back).Colors, true)];
                case Faces.Right:
                    return [
                        ..GetColumn(lastIndex, GetFace(Faces.Upper).Colors,true),
                        ..GetColumn(0, GetFace(Faces.Back).Colors, false),
                        ..GetColumn(lastIndex, GetFace(Faces.Down).Colors, true),
                        ..GetColumn(lastIndex, GetFace(Faces.Front).Colors, true)];
                case Faces.Upper:
                    return [
                        ..GetRow(0, GetFace(Faces.Front).Colors, true),
                        ..GetRow(0, GetFace(Faces.Left).Colors, true),
                        ..GetRow(0, GetFace(Faces.Back).Colors, true),
                        ..GetRow(0, GetFace(Faces.Right).Colors, true)];
                case Faces.Down:
                    return [
                        ..GetRow(lastIndex, GetFace(Faces.Front).Colors, false),
                        ..GetRow(lastIndex, GetFace(Faces.Right).Colors, false),
                        ..GetRow(lastIndex, GetFace(Faces.Back).Colors, false),
                        ..GetRow(lastIndex, GetFace(Faces.Left).Colors, false)];
                case Faces.Front:
                    return [
                        ..GetColumn(lastIndex, GetFace(Faces.Left).Colors, true),
                        ..GetRow(lastIndex, GetFace(Faces.Upper).Colors, false),
                        ..GetColumn(0, GetFace(Faces.Right).Colors,false),
                        ..GetRow(0, GetFace(Faces.Down).Colors, true)];
                case Faces.Back:
                    return [
                        ..GetRow(0, GetFace(Faces.Upper).Colors, true),
                        ..GetColumn(0, GetFace(Faces.Left).Colors, false),
                        ..GetRow(lastIndex, GetFace(Faces.Down).Colors, false),
                        ..GetColumn(lastIndex, GetFace(Faces.Right).Colors, true)];
                default: return [];
            }
        }
    }
}