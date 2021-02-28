using GravityLayout.Logic.Geometry;

namespace GravityLayout.Logic.Physics
{
    public class AntiGravity
    {
        public double AntigravitationalConstant { get; }

        /// <param name="constant">
        /// Specifies the strength of the anti-gravitational force.
        /// It specifies the magnitude of force exerted by anti-gravity on
        /// two objects each with a mass of 1, being a distance of 1 apart.
        /// The antigravitational force can be calculated like this:
        ///     Fa = A * ma * mb / d^2
        ///  where
        ///     Fa is the magnitude of the anti-gravitaional force exerted on object A
        ///     A is the anti-gravtiational constant
        ///     ma and mb are the masses of object A and B respectively
        ///     d is the distance between objects A nd B
        ///  The direction of Fa is that of the vector BA.
        /// </param>
        public AntiGravity(double constant)
        {
            AntigravitationalConstant = constant;
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
            Vector aToBVector = (Vector)positionB - (Vector)positionA;

            double distance = aToBVector.Length;

            double forceMagnitude = distance == 0.0
                ? double.PositiveInfinity
                : massA * massB * AntigravitationalConstant / distance / distance;

            Force forceA = Force.ForceUsingRad(forceMagnitude, aToBVector.Reverse().FiRad);
            Force forceB = Force.ForceUsingRad(forceMagnitude, aToBVector.FiRad);
            return (forceA, forceB);
        }
    }
}
