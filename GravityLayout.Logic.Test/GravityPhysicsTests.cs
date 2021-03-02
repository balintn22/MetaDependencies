using Dgml;
using FluentAssertions;
using GravityLayout.Logic.Geometry;
using GravityLayout.Logic.Physics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Linq;

namespace GravityLayout.Logic.Test.Physics
{
    [TestClass]
    public class GraphPhysicsTests
    {
        double ropeLength = 10.0;
        double ropeStrength = 1;
        double ag = 100000;
        const double TESTPRECISION = 0.000001;

        private GraphPhysics _sut;

        [TestInitialize]
        public void TestInitialize()
        {
            _sut = new GraphPhysics(
                ropeLength: ropeLength,
                ropeStrength: ropeStrength,
                ropeCharacteristics: Rope.Characteristics.Linear,
                antiGravitationalConstant: ag);
        }

        private DirectedGraphNode BuildNode(string id, int x, int y) =>
            new DirectedGraphNode { Id = id, Label = id, Bounds = $"{x}, {y}, 0, 0" };

        private DirectedGraphLink BuildLink(string id1, string id2) =>
            new DirectedGraphLink { Source = id1, Target = id2 };

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(10)]
        [DataRow(-10)]
        [DataRow(100)]
        [DataRow(-100)]
        public void RopeForces_OppositeComponents_ShouldCancelOut(int centerNodeYOffset)
        {
            var westNode = BuildNode("W", -20, 0);
            var centerNode = BuildNode("C", 0, centerNodeYOffset);
            var eastNode = BuildNode("E", 20, 0);
            var graph = new DirectedGraph
            {
                Nodes = new DirectedGraphNode[]
                {
                    westNode,
                    centerNode,
                    eastNode,
                },
                Links = new DirectedGraphLink[]
                {
                    BuildLink("C", "W"),
                    BuildLink("C", "E"),
                }
            };

            var nodeForces = _sut.NodeForces(graph).ToList();
            var westNodeForce = nodeForces.First(nf => nf.Key.Equals(westNode)).Value;
            var centerNodeForce = nodeForces.First(nf => nf.Key.Equals(centerNode)).Value;
            var eastNodeForce = nodeForces.First(nf => nf.Key.Equals(eastNode)).Value;

            ((Vector)centerNodeForce).X.Should().BeApproximately(0, TESTPRECISION, "the opposing forces from west and east nodes should cancel each other out.");
            ((Vector)westNodeForce).X.Should().BeApproximately(-((Vector)eastNodeForce).X, TESTPRECISION);
            ((Vector)westNodeForce).Y.Should().BeApproximately(((Vector)eastNodeForce).Y, TESTPRECISION);
            westNodeForce.Magnitude.Should().BeApproximately(eastNodeForce.Magnitude, TESTPRECISION);
        }
    }
}
