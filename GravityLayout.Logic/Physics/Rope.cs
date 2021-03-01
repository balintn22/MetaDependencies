using GravityLayout.Logic.Geometry;

namespace GravityLayout.Logic.Physics
{
    /// <summary>
    /// Represents a rope, that exerts a force between its endpoints,
    /// that is DeltaL * Strength or DeltaL^2 * Strength or DeltaL^3 * Strength
    /// depending on characteristics, when the rope is extended,
    /// and is 0 when the rope is shrunk.
    /// </summary>
    public class Rope
    {
        public enum Characteristics
        {
            Linear,
            Quadratic,
            Cubic,
        }

        /// <summary>Repesents the length of the rope under a force of 0.</summary>
        public double BaseLength { get; }

        /// <summary>Represents the rope's spring constant.</summary>
        public double Strength { get; }

        public Characteristics Characteristic { get; }  // I know, I know, this should be plural, but it's an instance of an enum...

        /// <summary>
        /// Creates a rope.
        /// </summary>
        /// <param name="baseLength">Specifies the length of the rope under a force of 0.</param>
        /// <param name="strength">Specifies the force-to-stretch constant.</param>
        /// <param name="characteristics"></param>
        public Rope(double baseLength, double strength, Characteristics characteristics)
        {
            BaseLength = baseLength;
            Strength = strength;
            Characteristic = characteristics;
        }

        /// <summary>
        /// Calculates the forces at either end of the rope, when extended between two points.
        /// The magnitude of the force is linearly/quadratically/cubicly proportional to the extension of the rope,
        /// in the direction of the rope, towards the other end.
        /// Force = -k * dl
        ///     where
        ///       k is the rope's spring constant (this.Strength)
        ///       dl is the difference to EquillibriumLength
        /// Force = 0 if dl<= 0
        /// </summary>
        /// <returns>Tuple, where forceA is the force exerted on the A, forceB is that on the B end of the rope.</returns>
        public (Force forceA, Force forceB) CalculateForces(Position positionA, Position positionB)
        {
            Vector aToBVector = (Vector)positionB - (Vector)positionA;

            double currentLength = aToBVector.Length;
            double dl = currentLength - BaseLength;

            double forceMagnitude =
                dl < 0 ? 0
                : Characteristic == Characteristics.Linear ? Strength * dl
                : Characteristic == Characteristics.Quadratic ? Strength * dl * dl
                : /* Characteristic == Characteristics.Cubic */ Strength * dl * dl * dl;

            Force forceA = Force.ForceUsingRad(forceMagnitude, aToBVector.FiRad);
            Force forceB = Force.ForceUsingRad(forceMagnitude, aToBVector.Reverse().FiRad);
            return (forceA, forceB);
        }
    }
}
