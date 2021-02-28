using FluentAssertions;
using MergeGraphs.Logic.SpringLayouting.Geometry;
using MergeGraphs.Logic.SpringLayouting.Physics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MergeGraphs.Logic.Test.SpringLayouting.Physics
{
    [TestClass]
    public class SpringTests
    {
        const double TESTPRECISION = 0.000001;

        [TestMethod]
        public void CalculateForces_NoExtension_ShouldProduceZeroForce()
        {
            var rnd = new Random();

            // Test with any random spring length
            var springLength = rnd.NextDouble();
            var springStiffness = rnd.NextDouble();

            // Point A can be anywhere
            var pA = new Position(rnd.Next(), rnd.Next());

            // Point B can be in any direction from A.
            // The important thing is that it is springLength away
            var vAB = Vector.FromPolar(springLength, rnd.NextDouble());

            Position pB = pA + vAB;

            Spring sut = new Spring(springLength, springStiffness, Spring.Characteristics.Linear);

            (Force fA, Force fB) = sut.CalculateForces(pA, pB);

            fA.Magnitude.Should().BeApproximately(0, TESTPRECISION);
            fB.Magnitude.Should().BeApproximately(0, TESTPRECISION);
        }

        [DataTestMethod]
        [DataRow(1, 1, 1)]
        [DataRow(2, 1, 2)]
        [DataRow(1, 2, 2)]
        [DataRow(2, 2, 4)]
        public void CalculateForces_NExtensionShouldProduceNTimesStiffnessForce(
            double stiffness, double extension, double expectedForceMagnitude)
        {
            var rnd = new Random();

            // Test with any random spring length
            var springLength = rnd.NextDouble();

            // Point A can be anywhere
            var pA = new Position(rnd.Next(), rnd.Next());

            // Point B can be in any direction from A.
            // The important thing is that it is N away
            var vAB = Vector.FromPolar(springLength + extension, rnd.NextDouble());

            Position pB = pA + vAB;

            Spring sut = new Spring(springLength, stiffness, Spring.Characteristics.Linear);

            (Force fA, Force fB) = sut.CalculateForces(pA, pB);

            fA.Magnitude.Should().BeApproximately(expectedForceMagnitude, TESTPRECISION);
            fB.Magnitude.Should().BeApproximately(expectedForceMagnitude, TESTPRECISION);
            fA.Magnitude.Should().Be(fB.Magnitude);
            fA.DirectionRad.Should().BeApproximately(vAB.FiRad, TESTPRECISION);
            fB.DirectionRad.Should().BeApproximately(vAB.Reverse().FiRad, TESTPRECISION);
        }
    }
}
