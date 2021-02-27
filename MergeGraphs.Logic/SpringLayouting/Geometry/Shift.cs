namespace MergeGraphs.Logic.SpringLayouting.Geometry
{
    /// <summary>
    /// Represents the change of position of an object.
    /// </summary>
    public class Shift
    {
        private Vector Vector { get; set; }

        public double DX => Vector.X;

        public double DY => Vector.Y;

        public Shift(double dx, double dy)
        {
            Vector = Vector.FromXY(dx, dy);
        }

        public static explicit operator Vector(Shift shift) => shift.Vector;

        public static explicit operator Shift(Vector vector) => new Shift(vector.X, vector.Y);
    }
}
