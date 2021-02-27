using MergeGraphs.Logic.SpringLayouting.Geometry;

namespace MergeGraphs.Logic.SpringLayouting.Physics
{
    public class AntiGravity
    {
        private double _strength;

        public AntiGravity(double strength)
        {
            _strength = strength;
        }

        /// <summary>
        /// Calculates the anti-gravitational push force of objects A and B.
        /// The value of the force is MassA * MassB * Strength / Distance^2
        /// </summary>
        /// <param name="massA">Mass of object A.</param>
        /// <param name="massB">Mass of object B.</param>
        /// <param name="positionA">Position of object A.</param>
        /// <param name="positionB">Position of object B.</param>
        /// <returns>Tuple, where forceA is the force exerted on object A, forceB is that on object B.</returns>
        public (Force forceA, Force forceB) CalculateForces(
            double massA,
            double massB,
            Position positionA,
            Position positionB)
        {
            Vector aToBVector = (Vector)positionA - (Vector)positionB;

            double distance = aToBVector.Length;

            double forceMagnitude = massA * massB * _strength / distance / distance;

            Force forceA = Force.ForceUsingRad(forceMagnitude, aToBVector.FiRad);
            Force forceB = Force.ForceUsingRad(forceMagnitude, aToBVector.Reverse().FiRad);
            return (forceA, forceB);
        }
    }
}
