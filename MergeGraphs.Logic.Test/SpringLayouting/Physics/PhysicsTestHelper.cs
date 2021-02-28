using MergeGraphs.Logic.SpringLayouting.Geometry;
using System;

namespace MergeGraphs.Logic.Test.SpringLayouting.Physics
{
    public static class PhysicsTestHelper
    {
        /// <summary>
        /// Given a desired distance, generates two random points and the distance
        /// vector fram A to B.
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static (Position pA, Position pB, Vector vAB) GetRandomPoints(double distance)
        {
            var rnd = new Random();
            var pA = new Position(rnd.Next(), rnd.Next());

            // Point B can be in any direction from A.
            // The important thing is that it is distance away
            var vAB = Vector.FromPolar(distance, rnd.NextDouble());

            Position pB = pA + vAB;

            return (pA, pB, vAB);
        }
    }
}
