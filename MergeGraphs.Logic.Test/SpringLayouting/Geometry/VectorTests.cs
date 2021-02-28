using FluentAssertions;
using MergeGraphs.Logic.SpringLayouting.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace MergeGraphs.Logic.Test.SpringLayouting.Geometry
{
    [TestClass]
    public class VectorTests
    {
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
        public void PolarVector_ShouldProduceExpectedCoordinates(double r, double fiDeg, double expectedX, double expectedY)
        {
            double fiRad = fiDeg / 180.0 * Math.PI;

            Vector sut = Vector.FromPolar(r, fiRad);

            sut.X.Should().BeApproximately(expectedX, 0.00000000001);
            sut.Y.Should().BeApproximately(expectedY, 0.00000000001);
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
            result.X.Should().BeApproximately(expectedX, 0.00000000001);
            result.Y.Should().BeApproximately(expectedY, 0.00000000001);
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
            result.X.Should().BeApproximately(expectedX, 0.00000000001);
            result.Y.Should().BeApproximately(expectedY, 0.00000000001);
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
            result.X.Should().BeApproximately(expectedX, 0.00000000001);
            result.Y.Should().BeApproximately(expectedY, 0.00000000001);
        }
    }
}
