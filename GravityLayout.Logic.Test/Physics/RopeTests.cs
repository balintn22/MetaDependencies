using FluentAssertions;
using GravityLayout.Logic.Geometry;
using GravityLayout.Logic.Physics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GravityLayout.Logic.Test.Physics
{
    [TestClass]
    public class RopeTests
    {
        const double TESTPRECISION = 0.000001;

        [TestMethod]
        public void CalculateForces_NoExtension_ShouldProduceZeroForce()
        {
            var rnd = new Random();
            var length = rnd.NextDouble();
            var strength = rnd.NextDouble();
            (Position pA, Position pB, _) = PhysicsTestHelper.GetRandomPoints(length);

            var sut = new Rope(length, strength, Rope.Characteristics.Linear);

            (Force fA, Force fB) = sut.CalculateForces(pA, pB);

            fA.Magnitude.Should().BeApproximately(0, TESTPRECISION);
            fB.Magnitude.Should().BeApproximately(0, TESTPRECISION);
        }

        [DataTestMethod]
        [DataRow(1, 1, 1)]
        [DataRow(2, 1, 2)]
        [DataRow(1, 2, 2)]
        [DataRow(2, 2, 4)]
        public void CalculateForces_NExtensionShouldProduceNTimesStrengthForce(
            double stiffness, double extension, double expectedForceMagnitude)
        {
            var rnd = new Random();
            var springLength = rnd.NextDouble();
            (Position pA, Position pB, Vector vAB) =
                PhysicsTestHelper.GetRandomPoints(springLength + extension);

            var sut = new Rope(springLength, stiffness, Rope.Characteristics.Linear);

            (Force fA, Force fB) = sut.CalculateForces(pA, pB);

            fA.Magnitude.Should().BeApproximately(expectedForceMagnitude, TESTPRECISION);
            fB.Magnitude.Should().BeApproximately(expectedForceMagnitude, TESTPRECISION);
            fA.Magnitude.Should().Be(fB.Magnitude);
            fA.DirectionRad.Should().BeApproximately(vAB.FiRad, TESTPRECISION);
            fB.DirectionRad.Should().BeApproximately(vAB.Reverse().FiRad, TESTPRECISION);
        }
    }
}
