using FluentAssertions;
using MergeGraphs.Logic.SpringLayouting.Geometry;
using MergeGraphs.Logic.SpringLayouting.Physics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MergeGraphs.Logic.Test.SpringLayouting.Physics
{
    [TestClass]
    public class AntiGravityTests
    {
        const double TESTPRECISION = 0.000001;

        private AntiGravity _sut;


        [TestInitialize]
        public void TestInitialize()
        {
            _sut = new AntiGravity(1);
        }

        [DataTestMethod]
        [DataRow(0, 0, 0)]
        [DataRow(0, 1, 0)]
        [DataRow(1, 0, 0)]
        [DataRow(1, 1, 1)]
        [DataRow(2, 1, 2)]
        [DataRow(1, 2, 2)]
        [DataRow(2, 2, 4)]
        public void CalculateForces_ShouldBeProportionalToMasses(
            double massA, double massB, double expectedForceMagnitude)
        {
            double distance = 1;
            (Position pA, Position pB, Vector vAB) = PhysicsTestHelper.GetRandomPoints(distance);

            (Force fA, Force fB) = _sut.CalculateForces(massA, massB, pA, pB);

            fA.Magnitude.Should().BeApproximately(expectedForceMagnitude, TESTPRECISION);
            fB.Magnitude.Should().BeApproximately(expectedForceMagnitude, TESTPRECISION);
            fA.Magnitude.Should().Be(fB.Magnitude);
            fA.DirectionRad.Should().BeApproximately(vAB.Reverse().FiRad, TESTPRECISION);
            fB.DirectionRad.Should().BeApproximately(vAB.FiRad, TESTPRECISION);
        }

        // Test masses are both 1.
        [DataTestMethod]
        //[DataRow(0, double.NaN)]
        [DataRow(1, 1)]
        [DataRow(2, 1.0 / 4.0)]
        [DataRow(3, 1.0 / 9.0)]
        public void CalculateForces_ShouldBeReverseProportionalToDistanceSquare(
            double distance, double expectedForceMagnitude)
        {
            double massA = 1;
            double massB = 1;
            (Position pA, Position pB, Vector vAB) = PhysicsTestHelper.GetRandomPoints(distance);

            (Force fA, Force fB) = _sut.CalculateForces(massA, massB, pA, pB);

            fA.Magnitude.Should().BeApproximately(expectedForceMagnitude, TESTPRECISION);
            fB.Magnitude.Should().BeApproximately(expectedForceMagnitude, TESTPRECISION);
            fA.Magnitude.Should().Be(fB.Magnitude);
            fA.DirectionRad.Should().BeApproximately(vAB.Reverse().FiRad, TESTPRECISION);
            fB.DirectionRad.Should().BeApproximately(vAB.FiRad, TESTPRECISION);
        }
    }
}
