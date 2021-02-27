using MergeGraphs.Logic.SpringLayouting.Geometry;

namespace MergeGraphs.Logic.SpringLayouting.Physics
{
    /// <summary>
    /// Represents a spring, that exerts a force between its endpoints,
    /// that is DeltaL * Strength or DeltaL^2 * Strength or DeltaL^3 * Strength
    /// depending on characteristics.
    /// </summary>
    public class Spring
    {
        public enum Characteristics
        {
            Linear,
            Quadratic,
            Cubic,
        }

        private double Length { get; }

        private double Strength { get; }

        private Characteristics Characteristic { get; }

        /// <summary>
        /// Creates a spring.
        /// </summary>
        /// <param name="length">Represents the default, un-elongated length of the spring, at which it exerts a force of 0.</param>
        /// <param name="strength"></param>
        /// <param name="characteristics"></param>
        public Spring(double length, double strength, Characteristics characteristics)
        {
            Length = length;
            Strength = strength;
            Characteristic = characteristics;
        }

        /// <summary>
        /// Calculates the spring forces at either end of the spring, when extended between two points.
        /// The magnitude of the force is linearly/quadratically6cubicly proportional to the extension of the spring,
        /// in the direction of the spring, towards the other end.
        /// </summary>
        /// <returns>Tuple, where forceA is the force exerted on the A, forceB is that on the B end of the spring.</returns>
        public (Force forceA, Force forceB) CalculateForces(Position positionA, Position positionB)
        {
            Vector aToBVector = (Vector)positionA - (Vector)positionB;

            double currentLength = aToBVector.Length;
            double dl = currentLength - Length;

            double springForceMagnitude =
                Characteristic == Characteristics.Linear ? Strength * dl :
                Characteristic == Characteristics.Quadratic ? Strength * dl * dl :
                /* Characteristic == Characteristics.Cubic */ Strength * dl * dl * dl;

            Force forceA = Force.ForceUsingRad(springForceMagnitude, aToBVector.FiRad);
            Force forceB = Force.ForceUsingRad(springForceMagnitude, aToBVector.Reverse().FiRad);
            return (forceA, forceB);
        }
    }
}
