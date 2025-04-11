using RubikCube.Server.App;
using RubikCube.Server.Models.Enums;

namespace RubikCube.Tests
{
    public class CubeTests
    {
        private readonly Cube _cube;

        public CubeTests()
        {
            _cube = new Cube();
        }

        [Fact]
        public void Reset_ShouldInitializeCubeToDefaultState()
        {
            // Act
            _cube.Reset();

            // Assert
            foreach (var face in _cube.GetFaces())
            {
                var expectedColor = (Colors)face.Id;

                foreach (var row in face.Colors)
                {
                    foreach (var color in row)
                    {
                        Assert.Equal(expectedColor, color.Value);
                    }
                }
            }
        }

        [Theory]
        [InlineData(Faces.Front, true)]
        [InlineData(Faces.Front, false)]
        [InlineData(Faces.Left, true)]
        [InlineData(Faces.Left, false)]
        public void RotateFace_ShouldRotateFaceCorrectly(Faces face, bool clockwise)
        {
            // Arrange
            Colors[][] originalColors = _cube.GetFace(face).Colors
                .Select(row => row.Select(c => c.Value).ToArray()).ToArray();

            // Act
            _cube.RotateFace(face, clockwise);

            // Assert
            var rotatedColors = _cube.GetFace(face).Colors
                .Select(row => row.Select(c => c.Value).ToArray()).ToArray();

            if (clockwise)
            {
                for (int i = 0; i < originalColors.Length; i++)
                {
                    for (int j = 0; j < originalColors.Length; j++)
                    {
                        Assert.Equal(originalColors[i][j], rotatedColors[j][originalColors.Length - 1 - i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < originalColors.Length; i++)
                {
                    for (int j = 0; j < originalColors.Length; j++)
                    {
                        Assert.Equal(originalColors[i][j], rotatedColors[originalColors.Length - 1 - j][i]);
                    }
                }
            }
        }

        [Fact]
        public void GetFace_ShouldReturnCorrectFace()
        {
            // Act
            var face = _cube.GetFace(Faces.Front);

            // Assert
            Assert.NotNull(face);
            Assert.Equal(Faces.Front, face.Id);
            foreach (var row in face.Colors)
            {
                foreach (var color in row)
                {
                    Assert.Equal(Colors.Green, color.Value);
                }
            }
        }

        [Fact]
        public void Rotate_ShouldReturnCorrectFacesWhenFrontRotated()
        {
            //Arrange
            Colors[,] expectedFront =
            {
                { Colors.Green, Colors.Green, Colors.Green },
                { Colors.Green, Colors.Green, Colors.Green },
                { Colors.Green, Colors.Green, Colors.Green }
            };

            Colors[,] expectedRight =
            {
                { Colors.White, Colors.Red, Colors.Red },
                { Colors.White, Colors.Red, Colors.Red },
                { Colors.White, Colors.Red, Colors.Red }
            };

            Colors[,] expectedLeft =
            {
                {  Colors.Orange, Colors.Orange,Colors.Yellow },
                {  Colors.Orange, Colors.Orange,Colors.Yellow },
                {  Colors.Orange, Colors.Orange,Colors.Yellow }
            };

            Colors[,] expectedBack =
            {
                { Colors.Blue, Colors.Blue, Colors.Blue },
                { Colors.Blue, Colors.Blue, Colors.Blue },
                { Colors.Blue, Colors.Blue, Colors.Blue }
            };

            Colors[,] expectedUp =
            {
                { Colors.White, Colors.White, Colors.White },
                { Colors.White, Colors.White, Colors.White },
                { Colors.Orange, Colors.Orange, Colors.Orange}
            };

            Colors[,] expectedDown =
            {
                { Colors.Red, Colors.Red, Colors.Red},
                { Colors.Yellow, Colors.Yellow, Colors.Yellow },
                { Colors.Yellow, Colors.Yellow, Colors.Yellow }
            };

            var expectedColors = new Dictionary<Faces, Colors[,]>
            {
                { Faces.Front, expectedFront },
                { Faces.Right, expectedRight },
                { Faces.Left, expectedLeft },
                { Faces.Back, expectedBack },
                { Faces.Upper, expectedUp },
                { Faces.Down, expectedDown }
            };

            // Act
            _cube.RotateFace(Faces.Front, true);

            // Assert
            foreach (var face in expectedColors)
            {
                var actualFace = _cube.GetFace(face.Key).Colors
                    .Select(row => row.Select(c => c.Value).ToArray()).ToArray();

                for (int i = 0; i < face.Value.GetLength(0); i++)
                {
                    for (int j = 0; j < face.Value.GetLength(1); j++)
                    {
                        Assert.Equal(face.Value[i, j], actualFace[i][j]);
                    }
                }
            }
        }

        [Fact]
        public void Rotate_ShouldReturnCorrectFacesOnManyRotations()
        {
            //Arrange
            Colors[,] expectedFront =
            {
                { Colors.Yellow, Colors.Yellow, Colors.Blue},
                { Colors.Red, Colors.Green, Colors.Red},
                { Colors.White, Colors.White, Colors.White}
            };

            Colors[,] expectedRight =
            {
                { Colors.Red, Colors.White, Colors.Yellow},
                { Colors.Yellow, Colors.Red, Colors.Green},
                { Colors.Orange, Colors.Orange, Colors.Green}
            };

            Colors[,] expectedLeft =
            {
                {  Colors.White, Colors.Blue,Colors.Blue},
                {  Colors.Blue, Colors.Orange,Colors.Green},
                {  Colors.White, Colors.Yellow,Colors.Orange }
            };

            Colors[,] expectedBack =
            {
                { Colors.Green, Colors.White, Colors.Red },
                { Colors.Yellow, Colors.Blue, Colors.Orange },
                { Colors.Red, Colors.White, Colors.Red }
            };

            Colors[,] expectedUp =
            {
                { Colors.Green, Colors.Blue, Colors.Orange },
                { Colors.Red, Colors.White, Colors.Red },
                { Colors.Orange, Colors.Blue, Colors.Yellow}
            };

            Colors[,] expectedDown =
            {
                { Colors.Green, Colors.Green, Colors.Blue},
                { Colors.Orange, Colors.Yellow, Colors.Green },
                { Colors.Blue, Colors.Orange, Colors.Yellow }
            };

            var expectedColors = new Dictionary<Faces, Colors[,]>
            {
                { Faces.Front, expectedFront },
                { Faces.Right, expectedRight },
                { Faces.Left, expectedLeft },
                { Faces.Back, expectedBack },
                { Faces.Upper, expectedUp },
                { Faces.Down, expectedDown }
            };

            // Act - F, R', U, B', L, D', F', R, U', B, L', D
            _cube.RotateFace(Faces.Front, true);
            _cube.RotateFace(Faces.Right, false);
            _cube.RotateFace(Faces.Upper, true);
            _cube.RotateFace(Faces.Back, false);
            _cube.RotateFace(Faces.Left, true);
            _cube.RotateFace(Faces.Down, false);
            _cube.RotateFace(Faces.Front, false);
            _cube.RotateFace(Faces.Right, true);
            _cube.RotateFace(Faces.Upper, false);
            _cube.RotateFace(Faces.Back, true);
            _cube.RotateFace(Faces.Left, false);
            _cube.RotateFace(Faces.Down, true);

            // Assert
            foreach (var face in expectedColors)
            {
                var actualFace = _cube.GetFace(face.Key).Colors
                    .Select(row => row.Select(c => c.Value).ToArray()).ToArray();

                for (int i = 0; i < face.Value.GetLength(0); i++)
                {
                    for (int j = 0; j < face.Value.GetLength(1); j++)
                    {
                        Assert.Equal(face.Value[i, j], actualFace[i][j]);
                    }
                }
            }
        }
    }
}
