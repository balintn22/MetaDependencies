using GravityLayout.Logic.Geometry;

namespace GravityLayout.Logic.Physics
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

        /// <summary>Repesents the length of the spring under a force of 0.</summary>
        public double EquillibriumLength { get; }

        /// <summary>Represents the spring constant.</summary>
        public double Stiffness { get; }

        /// <summary>
        /// This is an extension to ideal springs, so that we can play around with layouting.
        /// </summary>
        public Characteristics Characteristic { get; }  // I know, I know, this should be plural, but it's an instance of an enum...

        /// <summary>
        /// Creates a spring.
        /// </summary>
        /// <param name="equillibriumLength">Specifies the length of the spring under a force of 0.</param>
        /// <param name="stiffness">Specifies the spring constant.</param>
        /// <param name="characteristics"></param>
        public Spring(double equillibriumLength, double stiffness, Characteristics characteristics)
        {
            EquillibriumLength = equillibriumLength;
            Stiffness = stiffness;
            Characteristic = characteristics;
        }

        /// <summary>
        /// Calculates the spring forces at either end of the spring, when extended between two points.
        /// The magnitude of the force is linearly/quadratically/cubicly proportional to the extension of the spring,
        /// in the direction of the spring, towards the other end.
        /// Force = -k * dl
        ///     where
        ///       k is the spring constant (this.Stiffness)
        ///       dl is the difference to EquillibriumLength
        /// </summary>
        /// <returns>Tuple, where forceA is the force exerted on the A, forceB is that on the B end of the spring.</returns>
        public (Force forceA, Force forceB) CalculateForces(Position positionA, Position positionB)
        {
            Vector aToBVector = (Vector)positionB - (Vector)positionA;

            double currentLength = aToBVector.Length;
            double dl = currentLength - EquillibriumLength;

            double springForceMagnitude =
                Characteristic == Characteristics.Linear ? Stiffness * dl :
                Characteristic == Characteristics.Quadratic ? Stiffness * dl * dl :
                /* Characteristic == Characteristics.Cubic */ Stiffness * dl * dl * dl;

            Force forceA = Force.ForceUsingRad(springForceMagnitude, aToBVector.FiRad);
            Force forceB = Force.ForceUsingRad(springForceMagnitude, aToBVector.Reverse().FiRad);
            return (forceA, forceB);
        }
    }
}
