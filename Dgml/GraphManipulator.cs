using System.Collections.Generic;
using System.Linq;

namespace Dgml
{
    // TODO: Implement unit tests

    public class GraphManipulator
    {
        /// <summary>
        /// Floods a graph from an initial set of flooded nodes and links.
        /// Mutates floodedNodes and floodedLinks to contain all nodes and links to
        /// contain the nodes and links that are connected to the inital set.
        /// Flooding is not direction-specific, floods links both ways.
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="floodedNodes"></param>
        /// <param name="floodedLinks"></param>
        /// <returns></returns>
        public static bool Flood(
            DirectedGraph graph,
            HashSet<DirectedGraphNode> floodedNodes,
            HashSet<DirectedGraphLink> floodedLinks)
        {
            var newConnectedLinks = graph.Links
                .Where(link => floodedNodes.Any(floodedNode => AreConnected(floodedNode, link)));

            var newConnectedNodes = graph.Nodes
                .Where(node => newConnectedLinks.Any(newConnectedLink => AreConnected(node, newConnectedLink)));

            bool anyChanges = newConnectedLinks.Any() || newConnectedNodes.Any();
            if (!anyChanges)
                return anyChanges;

            floodedLinks.UnionWith(newConnectedLinks);
            floodedNodes.UnionWith(newConnectedNodes);
            return Flood(graph, floodedNodes, floodedLinks);
        }

        private static bool AreConnected(DirectedGraphNode node, DirectedGraphLink link) =>
            link.Source == node.Id || link.Target == node.Id;

        /// <summary>Creates a copy of the graph, without nodes and links.</summary>
        private static DirectedGraph CopyEmpty(DirectedGraph source)
        {
            return new DirectedGraph
            {
                Categories = source.Categories,
                GraphDirection = source.GraphDirection,
                Links = null,
                Nodes = null,
                Properties = source.Properties,
                Styles = source.Styles
            };
        }

        /// <summary>Creates a shallow copy of a graph including nodes and links.</summary>
        public static DirectedGraph Copy(DirectedGraph source)
        {
            var ret = CopyEmpty(source);
            ret.Nodes = source.Nodes.Select(node => node).ToArray();
            ret.Links = source.Links.Select(link => link).ToArray();
            return ret;
        }

        /// <summary>
        /// Splits the source graph into a set of fully connected subgraphs.
        /// If source is fully connected, returns that as a single result.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<DirectedGraph> SplitToConnectedSubGraphs(
            DirectedGraph source)
        {
            var ret = new List<DirectedGraph>();
            var remainder = Copy(source);

            if (remainder.Nodes is null || remainder.Nodes.Count() == 0)
            {
                ret.Add(remainder);
            }
            else
            {
                while (remainder.Nodes != null || remainder.Nodes.Length > 0)
                {
                    DirectedGraphNode anyRemainingNode = remainder.Nodes.First();
                    var connectedNodes = new HashSet<DirectedGraphNode> { anyRemainingNode };
                    var connectedLinks = new HashSet<DirectedGraphLink>();
                    Flood(remainder, connectedNodes, connectedLinks);

                    var connectedSubGraph = CopyEmpty(remainder);
                    connectedSubGraph.Nodes = connectedNodes.ToArray();
                    connectedSubGraph.Links = connectedLinks.ToArray();
                    ret.Add(connectedSubGraph);

                    remainder.Nodes = remainder.Nodes.Except(connectedNodes).ToArray();
                    remainder.Links = remainder.Links.Except(connectedLinks).ToArray();
                }
            }

            return ret;
        }
    }
}
