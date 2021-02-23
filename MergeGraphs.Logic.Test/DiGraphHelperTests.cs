using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MergeGraphs.Logic.Test
{
    [TestClass]
    public class DiGraphHelperTests
    {
        private Dgml.DirectedGraphNode BuildNode(string id) =>
            new Dgml.DirectedGraphNode { Id = id, Label = id };

        private Dgml.DirectedGraphLink BuildLink(string source, string target) =>
            new Dgml.DirectedGraphLink { Source = source, Target = target };

        private Dgml.DirectedGraph BuildEmptyGraph() =>
            new Dgml.DirectedGraph();

        private Dgml.DirectedGraph BuildGraph()
        {
            return new Dgml.DirectedGraph
            {
                Nodes = new Dgml.DirectedGraphNode[]
                {
                    BuildNode("a"),
                    BuildNode("b"),
                    BuildNode("c"),
                },
                Links = new Dgml.DirectedGraphLink[]
                {
                    BuildLink("a", "b"),
                    BuildLink("b", "c"),
                }
            };
        }

        #region GetNeighbors tests

        [TestMethod]
        public void GetNeighbors_WhenGraphHasNoLinks_ShouldReturnExpected()
        {
            var graph = BuildEmptyGraph();
            var result = DiGraphHelper.GetDiNeighbors(graph);

            result.Should().BeEmpty();
        }

        [TestMethod]
        public void GetNeighbors_ShouldReturnExpected()
        {
            var graph = BuildGraph();
            var result = DiGraphHelper.GetDiNeighbors(graph);

            result["a"].Should().BeEquivalentTo("b");
            result["b"].Should().BeEquivalentTo("c");
            result["c"].Should().BeEquivalentTo();
        }

        #endregion GetNeighbors tests

        #region FindLongPaths tests

        [TestMethod]
        public void FindLongPaths_WhenGraphHasNoLinks_ShouldReturnExpected()
        {
            var graph = BuildEmptyGraph();
            var result = DiGraphHelper.FindLongPaths(graph);

            result.Should().BeEmpty();
        }

        [TestMethod]
        public void FindLongPaths_ShouldReturnExpected()
        {
            var graph = BuildGraph();
            var result = DiGraphHelper.FindLongPaths(graph);

            result.Should().BeEquivalentTo((new DiNodePair("a", "c")));
        }

        [TestMethod]
        public void FindLongPaths_ShouldReturnDistinct()
        {
            // This graph has two paths between a and c
            var graph = new Dgml.DirectedGraph
            {
                Nodes = new Dgml.DirectedGraphNode[]
                {
                    BuildNode("a"),
                    BuildNode("b"),
                    BuildNode("c"),
                    BuildNode("x"),
                },
                Links = new Dgml.DirectedGraphLink[]
                {
                    BuildLink("a", "b"),
                    BuildLink("b", "c"),
                    BuildLink("a", "x"),
                    BuildLink("x", "c"),
                }
            };
            var result = DiGraphHelper.FindLongPaths(graph);

            result.Should().BeEquivalentTo(new DiNodePair("a", "c"));
        }

        [TestMethod]
        public void FindLongPaths_ShouldFindAllCombinations()
        {
            // This graph has two paths between a and c
            var graph = new Dgml.DirectedGraph
            {
                Nodes = new Dgml.DirectedGraphNode[]
                {
                    BuildNode("a"),
                    BuildNode("b"),
                    BuildNode("c"),
                    BuildNode("d"),
                },
                Links = new Dgml.DirectedGraphLink[]
                {
                    BuildLink("a", "b"),
                    BuildLink("b", "c"),
                    BuildLink("c", "d"),
                }
            };
            var result = DiGraphHelper.FindLongPaths(graph);

            result.Should().BeEquivalentTo(
                new DiNodePair("a", "c"),
                new DiNodePair("b", "d"),
                new DiNodePair("a", "d")
            );
        }

        #endregion FindLongPaths tests

        #region FindLongPaths tests

        [TestMethod]
        public void GetShortcuts_WhenGraphHasNoLinks_ShouldReturnExpected()
        {
            var graph = BuildEmptyGraph();
            var result = DiGraphHelper.GetShortcuts(graph);

            result.Should().BeEmpty();
        }

        [TestMethod]
        public void GetShortcuts_ShouldBeEmpty_IfThereAreNoShortcuts()
        {
            var graph = BuildGraph();
            var result = DiGraphHelper.GetShortcuts(graph);

            result.Should().BeEmpty();
        }

        [TestMethod]
        public void GetShortcuts_ShouldFindAllCombinations()
        {
            // This graph has two paths between a and c
            var graph = new Dgml.DirectedGraph
            {
                Nodes = new Dgml.DirectedGraphNode[]
                {
                    BuildNode("a"),
                    BuildNode("b"),
                    BuildNode("c"),
                },
                Links = new Dgml.DirectedGraphLink[]
                {
                    BuildLink("a", "b"),
                    BuildLink("b", "c"),
                    BuildLink("a", "c"),  // Shortcut
                }
            };
            var result = DiGraphHelper.GetShortcuts(graph);

            result.Should().BeEquivalentTo(
                BuildLink("a", "c")
            );
        }

        #endregion FindLongPaths tests
    }
}
