using MergeGraphs.Logic.SpringLayouting.Geometry;
using System;

namespace MergeGraphs.Logic.SpringLayouting.Physics
{
    /// <summary>
    /// Represents a force acting on an object.
    /// </summary>
    public class Force
    {
        private Vector Vector { get; set; }

        public double Magnitude => Vector.Length;

        public double DirectionRad => Vector.FiRad;

        public double DirectionDeg => Vector.FiDeg;

        public static Force ForceUsingRad(double magnitude, double directionRad)
        {
            return new Force { Vector = Vector.FromPolar(magnitude, directionRad) };
        }

        public static Force ForceUsingDeg(double magnitude, double directionRad)
        {
            return new Force { Vector = Vector.FromPolar(magnitude, directionRad / 360.0 * Math.PI) };
        }
    }
}
