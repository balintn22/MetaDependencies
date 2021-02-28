﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace MergeGraphs.Logic
{
    /// <summary>
    /// Helps to split graphs to fully connected subgraphs.
    /// </summary>
    public static class DiGraphSplitter
    {
        /// <summary>Creates a copy of the graph, without nodes and links.</summary>
        private static Dgml.DirectedGraph CopyEmpty(Dgml.DirectedGraph source)
        {
            return new Dgml.DirectedGraph
            {
                Categories = source.Categories,
                GraphDirection = source.GraphDirection,
                Links = null,
                Nodes = null,
                Properties = source.Properties,
                Styles = source.Styles
            };
        }

        /// <summary>Creates a shallow copy of a graph.</summary>
        private static Dgml.DirectedGraph Copy(Dgml.DirectedGraph source)
        {
            var ret = CopyEmpty(source);
            ret.Nodes = source.Nodes.Select(node => node).ToArray();
            ret.Links = source.Links.Select(link => link).ToArray();
            return ret;
        }

        public static IEnumerable<Dgml.DirectedGraph> SplitToConnectedSubGraphs(Dgml.DirectedGraph diGraph)
        {
            var ret = new List<Dgml.DirectedGraph>();
            var remainder = Copy(diGraph);

            while (remainder.Nodes != null || remainder.Nodes.Length > 0)
            {
                ret.Add(ExtractConnected(remainder, diGraph.Nodes.First()));
            }

            return ret;
        }

        /// <summary>
        /// Extracts a sub-graph that is fully connected with the specified nodes.
        /// Mutates the source by removing extracted nodes and links.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="nodes"></param>
        /// <returns></returns>
        private static Dgml.DirectedGraph ExtractConnected(Dgml.DirectedGraph source, Dgml.DirectedGraphNode startNode)
        {
            Dgml.DirectedGraph connectedGraph = CopyEmpty(source);
            connectedGraph.Nodes = new Dgml.DirectedGraphNode[] { startNode };
            source.Nodes = source.Nodes.Except(connectedGraph.Nodes).ToArray();

            while(true)
            {
                (List<string> firstNeighborNodeIds, List<Dgml.DirectedGraphLink> connectedLinks) = FindFirstNeighbors(source, connectedGraph.Nodes);
                if (!firstNeighborNodeIds.Any() && !connectedLinks.Any())
                    break;

                // Move first neighbor nodes from source to connected.
                var firstNeighborNodes = source.Nodes.Where(sourceNode => firstNeighborNodeIds.Contains(sourceNode.Id));
                connectedGraph.Nodes = connectedGraph.Nodes.Union(firstNeighborNodes).ToArray();
                source.Nodes = source.Nodes.Except(firstNeighborNodes).ToArray();

                // Move links from source to connected
                connectedGraph.Links = connectedGraph.Links.Union(connectedLinks).ToArray();
                source.Links = source.Links.Except(connectedLinks).ToArray();
            }

            return connectedGraph;
        }

        /// <summary>Finds nodes and links that are first neighbors to the ones in fromNodes.</summary>
        /// <returns>Tuple with 1st neighbor node ids and their connecting links.</returns>
        private static (List<string> firstNeighborNodeIds, List<Dgml.DirectedGraphLink> connectedLinks) FindFirstNeighbors(
            Dgml.DirectedGraph source, IEnumerable<Dgml.DirectedGraphNode> fromNodes)
        {
            var connectedNodeIds = new List<string>();
            var connectedLinks = new List<Dgml.DirectedGraphLink>();

            foreach (Dgml.DirectedGraphLink link in source.Links)
            {
                if (fromNodes.Any(fromNode => fromNode.Id == link.Source) || fromNodes.Any(fromNode => fromNode.Id == link.Target))
                {   // This link connects to fromNodes.

                    connectedLinks.Add(link);
                    if (!connectedNodeIds.Any(connectedNodeId => connectedNodeId == link.Source))
                        connectedNodeIds.Add(link.Source);
                    if (!connectedNodeIds.Any(connectedNodeId => connectedNodeId == link.Target))
                        connectedNodeIds.Add(link.Target);
                }
            }

            return (connectedNodeIds, connectedLinks);
        }
    }
}
