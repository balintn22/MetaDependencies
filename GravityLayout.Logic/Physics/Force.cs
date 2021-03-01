using GravityLayout.Logic.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GravityLayout.Logic.Physics
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

        public static explicit operator Vector(Force force) => force.Vector;

        public static explicit operator Force(Vector vector) => ForceUsingRad(vector.Length, vector.FiRad);

        public static Force operator +(Force f1, Force f2) => (Force)(f1.Vector + f2.Vector);

        public static Force Sum(IEnumerable<Force> forces)
        {
            if (forces is null)
                throw new ArgumentNullException(nameof(forces));

            var forceVectors = forces.Select(force => (Vector)force);
            var sumVector = Vector.Sum(forceVectors);
            return (Force)sumVector;
        }
    }
}
