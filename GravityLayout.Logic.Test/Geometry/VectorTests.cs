using FluentAssertions;
using GravityLayout.Logic.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace GravityLayout.Logic.Test.Geometry
{
    [TestClass]
    public class VectorTests
    {
        const double TESTPRECISION = 0.00000000001;
        const double SQRT2 = 1.414213562373095; // Sqrt(2)

        [DataTestMethod]
        [DataRow(1, 0, 1, 0)]    // East
        [DataRow(1, 90, 0, 1)]   // North
        [DataRow(1, 180, -1, 0)] // West
        [DataRow(1, 270, 0, -1)] // South
        [DataRow(1, 360, 1, 0)]  // East again
        [DataRow(2, 0, 2, 0)]    // East
        [DataRow(2, 90, 0, 2)]   // North
        [DataRow(2, 180, -2, 0)] // West
        [DataRow(2, 270, 0, -2)] // South
        [DataRow(2, 360, 2, 0)]  // East again
        public void PolarVector_ShouldProduceExpectedCoordinates(
            double length, double fiDeg, double expectedX, double expectedY)
        {
            double fiRad = fiDeg / 180.0 * Math.PI;

            Vector sut = Vector.FromPolar(length, fiRad);

            sut.X.Should().BeApproximately(expectedX, TESTPRECISION);
            sut.Y.Should().BeApproximately(expectedY, TESTPRECISION);
        }

        [DataTestMethod]
        [DataRow(-1, 0, 1, 180)]    // Counter-East       - West
        [DataRow(-1, 45, 1, 225)]   // Counter-North-East - South-West
        [DataRow(-1, 90, 1, 270)]   // Counter-North      - South
        [DataRow(-1, 135, 1, 315)]  // Counter-North-West - South-East
        [DataRow(-1, 180, 1, 0)]    // Counter-West       - East
        [DataRow(-1, 225, 1, 45)]   // Counter-South-West - North-East
        [DataRow(-1, 270, 1, 90)]   // Counter-South      - North
        [DataRow(-1, 315, 1, 135)]  // Counter-South-East - North-West
        [DataRow(-1, 360, 1, 180)]  // Counter-East       - West again
        public void PolarVector_NegativeLength_ShouldBeReversed(
            double length, double fiDeg, double expectedLength, double expectedFiDeg)
        {
            double fiRad = fiDeg / 180.0 * Math.PI;

            Vector sut = Vector.FromPolar(length, fiRad);

            sut.Length.Should().BeApproximately(expectedLength, TESTPRECISION);
            sut.FiDeg.Should().BeApproximately(expectedFiDeg, TESTPRECISION);
        }

        [DataTestMethod]
        [DataRow(1, 0, 1, 0 * Math.PI / 4, 0)]         // East
        [DataRow(1, 1, SQRT2, 1 * Math.PI / 4, 45)]    // North-East
        [DataRow(0, 1, 1, 2 * Math.PI / 4, 90)]        // North
        [DataRow(-1, 1, SQRT2, 3 * Math.PI / 4, 135)]  // North-West
        [DataRow(-1, 0, 1, 4 * Math.PI / 4, 180)]      // West
        [DataRow(-1, -1, SQRT2, 5 * Math.PI / 4, 225)] // South-West
        [DataRow(0, -1, 1, 6 * Math.PI / 4, 270)]      // South
        [DataRow(1, -1, SQRT2, 7 * Math.PI / 4, 315)]  // South-East
        public void PolarCoordinates_ShouldBeAsExpected(
            double x, double y, double expectedLength, double expectedFiRad, double expectedFiDeg)
        {
            Vector sut = Vector.FromXY(x, y);

            sut.Length.Should().BeApproximately(expectedLength, TESTPRECISION);
            sut.FiRad.Should().BeApproximately(expectedFiRad, TESTPRECISION);
            sut.FiDeg.Should().BeApproximately(expectedFiDeg, TESTPRECISION);
        }

        [DataTestMethod]
        [DataRow(-0 * Math.PI / 4, 0 * Math.PI / 4)]
        [DataRow(-1 * Math.PI / 4, 7 * Math.PI / 4)]
        [DataRow(-2 * Math.PI / 4, 6 * Math.PI / 4)]
        [DataRow(-3 * Math.PI / 4, 5 * Math.PI / 4)]
        [DataRow(-4 * Math.PI / 4, 4 * Math.PI / 4)]
        [DataRow(-5 * Math.PI / 4, 3 * Math.PI / 4)]
        [DataRow(-6 * Math.PI / 4, 2 * Math.PI / 4)]
        [DataRow(-7 * Math.PI / 4, 1 * Math.PI / 4)]
        public void PolarCoordinates_NegativeAngles_ShouldBeTranslatedIntoRangeZero2Pi(
            double fiRad, double expectedFiRad)
        {
            Vector sut = Vector.FromPolar(1, fiRad);

            sut.FiRad.Should().BeApproximately(expectedFiRad, TESTPRECISION);
        }

        [DataTestMethod]
        [DataRow(0, 0, 0, 0, 0, 0)]
        [DataRow(0, 0, 1, 0, 1, 0)]
        [DataRow(0, 0, 0, 1, 0, 1)]
        [DataRow(0, 0, 1, 1, 1, 1)]
        [DataRow(1, 1, 2, 2, 3, 3)]
        public void Add_ShouldProduceExpected(
            double x1, double y1, double x2, double y2, double expectedX, double expectedY)
        {
            Vector v1 = Vector.FromXY(x1, y1);
            Vector v2 = Vector.FromXY(x2, y2);
            var result = v1 + v2;
            result.X.Should().BeApproximately(expectedX, TESTPRECISION);
            result.Y.Should().BeApproximately(expectedY, TESTPRECISION);
        }

        [DataTestMethod]
        [DataRow(0, 0, 0, 0, 0, 0)]
        [DataRow(0, 0, 1, 0, -1, 0)]
        [DataRow(0, 0, 0, 1, 0, -1)]
        [DataRow(0, 0, 1, 1, -1, -1)]
        [DataRow(1, 1, 2, 2, -1, -1)]
        public void Subtract_ShouldProduceExpected(
            double x1, double y1, double x2, double y2, double expectedX, double expectedY)
        {
            Vector v1 = Vector.FromXY(x1, y1);
            Vector v2 = Vector.FromXY(x2, y2);
            var result = v1 - v2;
            result.X.Should().BeApproximately(expectedX, TESTPRECISION);
            result.Y.Should().BeApproximately(expectedY, TESTPRECISION);
        }

        [DataTestMethod]
        [DataRow(0, 0, 0, 0, 0, 0)]
        [DataRow(0, 0, 1, 0, 1, 0)]
        [DataRow(0, 0, 0, 1, 0, 1)]
        [DataRow(0, 0, 1, 1, 1, 1)]
        [DataRow(1, 1, 2, 2, 3, 3)]
        public void Sum_ShouldProduceExpected(
            double x1, double y1, double x2, double y2, double expectedX, double expectedY)
        {
            Vector v1 = Vector.FromXY(x1, y1);
            Vector v2 = Vector.FromXY(x2, y2);
            var vectors = new List<Vector> { v1, v2 };
            var result = Vector.Sum(vectors);
            result.X.Should().BeApproximately(expectedX, TESTPRECISION);
            result.Y.Should().BeApproximately(expectedY, TESTPRECISION);
        }

        [DataTestMethod]
        [DataRow(0, 0, 0, 0)]
        [DataRow(0, 1, 0, -1)]
        [DataRow(1, 0, -1, 0)]
        [DataRow(1, 1, -1, -1)]
        [DataRow(-1, -1, 1, 1)]
        public void Reverse_ShouldProduceExpected(
            double x, double y, double expectedX, double expectedY)
        {
            Vector sut = Vector.FromXY(x, y);
            var result = sut.Reverse();
            result.X.Should().BeApproximately(expectedX, TESTPRECISION);
            result.Y.Should().BeApproximately(expectedY, TESTPRECISION);
        }
    }
}
