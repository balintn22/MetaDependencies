namespace MergeGraphs.Logic.SpringLayouting.Geometry
{
    /// <summary>
    /// Represents the position of an object.
    /// </summary>
    public class Position
    {
        private Vector Vector { get; set; }

        public double X => Vector.X;

        public double Y => Vector.Y;

        public Position(double x, double y)
        {
            Vector = Vector.FromXY(x, y);
        }

        public static explicit operator Vector(Position position) => position.Vector;

        public static explicit operator Position(Vector vector) => new Position(vector.X, vector.Y);

        public static Position operator +(Position p, Vector v) => new Position(p.X + v.X, p.Y + v.Y);

        public static Position operator -(Position p, Vector v) => new Position(p.X - v.X, p.Y - v.Y);
    }
}
