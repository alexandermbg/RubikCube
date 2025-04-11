using RubikCube.Server.Models.Enums;

namespace RubikCube.Server.Models
{
    public record Color
    {
        public Color(Colors value)
        {
            Value = value;
        }

        public Colors Value { get; set; }
    }
}
