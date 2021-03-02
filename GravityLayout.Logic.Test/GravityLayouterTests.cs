using Dgml;
using FluentAssertions;
using GravityLayout.Logic.Physics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace GravityLayout.Logic.Test.Physics
{
    [TestClass]
    public class GravityLayouterTests
    {
        private DirectedGraphNode BuildNode(string id, int x, int y) =>
            new DirectedGraphNode { Id = id, Label = id, Bounds = $"{x}, {y}, 0, 0" };

        private DirectedGraphLink BuildLink(string id1, string id2) =>
            new DirectedGraphLink { Source = id1, Target = id2 };

        [TestMethod]
        public void Layout_2Nodes1Edge_ShouldConverge()
        {
            double ropeLength = 200.0;
            double ropeStrength = 1;
            double ag = 100000;
            double stopShiftThreshold = 0.00000001;
            int maxIterationCount = 20;
            var repo = new DgmlRepo();

            var node1 = BuildNode("1", -50, 0);
            var node2 = BuildNode("2", 50, 0);
            var graph = new DirectedGraph
            {
                Nodes = new DirectedGraphNode[]
                {
                    node1,
                    node2,
                },
                Links = new DirectedGraphLink[]
                {
                    BuildLink("1", "2"),
                }
            };

            repo.Save(graph, $@"c:\balint\waste\output.00.dgml");

            var layouter = new GravityLayouter(
                ropeLength, ropeStrength, Rope.Characteristics.Linear, ag);
            layouter.Layout(graph, stopShiftThreshold, maxIterationCount, (i) =>
            {
                Trace.WriteLine(i);
                repo.Save(i.Graph, $@"c:\balint\waste\output.{i.Count:00}.dgml");
            });

            node1.GetBoundingRect().Value.X.Should().BeLessThan(-100, "AG should blow this node down negative X, and rope should extend a bit.");
            node2.GetBoundingRect().Value.X.Should().BeGreaterThan(100, "AG should blow this node up positive X, and rope should extend a bit.");
        }

        [TestMethod]
        public void Layout_3Nodes2Edges_ShouldConverge()
        {
            double ropeLength = 200.0;
            double ropeStrength = 1;
            double ag = 100000;
            double stopShiftThreshold = 0.00000001;
            int maxIterationCount = 20;
            var repo = new DgmlRepo();

            var node1 = BuildNode("1", -50, 0);
            var node2 = BuildNode("2", 0, 20);
            var node3 = BuildNode("3", 50, 0);
            var graph = new DirectedGraph
            {
                Nodes = new DirectedGraphNode[]
                {
                    node1,
                    node2,
                    node3,
                },
                Links = new DirectedGraphLink[]
                {
                    BuildLink("1", "2"),
                    BuildLink("2", "3"),
                }
            };

            var layouter = new GravityLayouter(
                ropeLength, ropeStrength, Rope.Characteristics.Linear, ag);
            layouter.Layout(graph, stopShiftThreshold, maxIterationCount, (i) =>
            {
                Trace.WriteLine(i);
                repo.Save(i.Graph, $@"c:\balint\waste\output.{i.Count:00}.dgml");
            });
        }

        #region Complex Tests

        [TestMethod]
        [DeploymentItem("DgmlWithBounds.dgml")]
        public void Layout_ForDgmlWithBounds_RunsWithoutErrors()
        {
            double ropeLength = 50.0;
            double ropeStrength = 1;
            double ag = 0.00000000000000000001;
            double stopShiftThreshold = 0.00000001;
            int maxIterationCount = 1000;

            var repo = new DgmlRepo();
            DirectedGraph graph = repo.Load("DgmlWithBounds.dgml");

            var layouter = new GravityLayouter(ropeLength, ropeStrength, Rope.Characteristics.Linear, ag);
            layouter.Layout(graph, stopShiftThreshold, maxIterationCount, (i) =>
            {
                Trace.WriteLine(i);
                //repo.Save(i.Graph, $@"c:\balint\waste\output.{i.Count:00}.dgml");
            });
            
            repo.Save(graph, @"c:\balint\waste\output.dgml");
        }

        [TestMethod]
        [DeploymentItem("DgmlWithoutBounds.dgml")]
        public void Layout_ForDgmlWithoutBounds_RunsWithoutErrors()
        {
            var repo = new DgmlRepo();
            DirectedGraph graph = repo.Load("DgmlWithoutBounds.dgml");
        }

        #endregion Complex Tests
    }
}
