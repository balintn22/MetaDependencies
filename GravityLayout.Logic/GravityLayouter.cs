using Dgml;
using GravityLayout.Logic.Geometry;
using GravityLayout.Logic.Physics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GravityLayout.Logic
{
    /// <summary>
    /// Implements an algorithm that changes a graph's layout such, that
    /// edges do not cross each other as much as possible.
    /// Algorithm:
    ///     - edges are springs, that attract nodes. Attraction force is proportional to lentgh.
    ///     - anti-gravity repels nodes. This force is proportional to 1/distance^2
    ///     - not implemented: node boundaries should repel each other with a huge force, to avoid putting nodes on top of each other
    /// </summary>
    public class GravityLayouter
    {
        /// <summary>
        /// Lays out the graph blowing nodes apart so that edges cross as little as possible.
        /// Graph may not be fully connected. Connected sub-graphs are layed out individually and placed
        /// one below another.
        /// </summary>
        /// <param name="graph"></param>
        /// <returns>A new graph laid out such that edges cross as little as possible.</returns>
        public DirectedGraph Layout(DirectedGraph graph)
        {
            // Split to a set of fully connected sub-graphs
            var connectedSubGraphs = GraphManipulator.SplitToConnectedSubGraphs(graph).ToList();

            // Layout individual fully connected graphs
            var layoutedSubGraphs = new List<DirectedGraph>();
            foreach (var subGraph in connectedSubGraphs)
                layoutedSubGraphs.Add(LayoutFullyConnected(subGraph));

            // Place them one under another
            Stack(connectedSubGraphs.ToList());

            // Merge them
            return Merge(connectedSubGraphs);
        }


        /// <summary>
        /// Graphically stacks a set of graphs by shifting their nodes such,
        /// that they are position in a stacked fashion, starting with first on top,
        /// and subsequent ones below each other.
        /// </summary>
        /// <param name="graphs"></param>
        private void Stack(List<DirectedGraph> graphs)
        {
            if (graphs is null || graphs.Count == 0)
                return;

            float gap = 20;
            float currentBottom = graphs[0].GetBoundingRect()?.Bottom ?? 0f;

            for (int i = 1; i < graphs.Count; i++)
            {
                RectangleF? graphIRect = graphs[i].GetBoundingRect();
                float shiftY = graphIRect is null
                    ? currentBottom
                    : ((RectangleF)graphIRect).Top - currentBottom - gap;
                ShiftGraph(graphs[i], shiftY);
            }
        }

        /// <summary>
        /// Merges a set of graphs by taking meta info from the first, and merging
        /// node and link collections.
        /// </summary>
        private DirectedGraph Merge(List<DirectedGraph> graphs)
        {
            if (graphs == null || graphs.Count == 0)
                return null;

            var merged = GraphManipulator.Copy(graphs[0]);
            for (int i = 1; i < graphs.Count; i++)
            {
                merged.Nodes = merged.Nodes.Union(graphs[i].Nodes).ToArray();
                merged.Links = merged.Links.Union(graphs[i].Links).ToArray();
            }

            return merged;
        }

        private void ShiftGraph(DirectedGraph graph, float dy)
        {
            foreach (var node in graph.Nodes)
            {
                RectangleF nodeRect = node.GetBoundingRect() ?? new RectangleF(0,0,0,0);
                nodeRect.Y += dy;
                node.SetBoundingRect(nodeRect);
            }
        }

        /// <summary>
        /// Lays out a graph assuming it is fully connected.
        /// </summary>
        /// <param name="diGraph"></param>
        /// <returns></returns>
        private DirectedGraph LayoutFullyConnected(DirectedGraph diGraph)
        {
            // TODO: Use AntiGravityLayouting to arrange the graph.

            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates the resultant forces at each node.
        /// </summary>
        /// <param name="edgeSpringLength">
        /// Specifies the length of edge-springs, where spring force is 0.
        /// When edge-springs are compressed or elongated, they generate a force proportional to
        /// their change in length and opposite in direction.
        /// </param>
        /// <param name="antiGravitationalConstant"></param>
        /// <returns>Dictionary of resultant forces by node id.</returns>
        private Dictionary<DirectedGraphNode, Force> CalculateNodeForces(
            DirectedGraph graph,
            double edgeSpringLength,
            double edgeSpringStiffness,
            Spring.Characteristics edgeSprintCharacteristics,
            double antiGravitationalConstant)
        {
            var ret = new Dictionary<DirectedGraphNode, Force>();

            if (graph.Nodes is null)
                return ret;

            var nodeForces = CalculateSpringForces(graph, edgeSpringLength, edgeSpringStiffness, edgeSprintCharacteristics);
            foreach (var nodeForce in CalculateAntiGravitationalForces(graph, antiGravitationalConstant))
            {
                if (!ret.ContainsKey(nodeForce.Key))
                    ret.Add(nodeForce.Key, nodeForce.Value);
                else
                    ret[nodeForce.Key] += nodeForce.Value;
            }

            return nodeForces;
        }

        private Dictionary<DirectedGraphNode, Force> CalculateAntiGravitationalForces(
            DirectedGraph graph,
            double antiGravitationalConstant)
        {
            var ret = new Dictionary<DirectedGraphNode, Force>();
            AntiGravity antiGravity = new AntiGravity(antiGravitationalConstant);

            // Consider each node-pair exactly once
            for (int a = 0; a < graph.Nodes.Length - 1; a++)
            {
                DirectedGraphNode nodeA = graph.Nodes[a];
                double massA = GetMass(nodeA);

                for (int b = a + 1; b < graph.Nodes.Length; b++)
                {
                    DirectedGraphNode nodeB = graph.Nodes[b];
                    double massB = GetMass(nodeB);
                }
            }

            return ret;
        }

        private Dictionary<DirectedGraphNode, Force> CalculateSpringForces(
            DirectedGraph graph,
            double edgeSpringLength,
            double edgeSpringStiffness,
            Spring.Characteristics edgeSprintCharacteristics)
        {
            var ret = new Dictionary<DirectedGraphNode, Force>();
            Spring spring = new Spring(edgeSpringLength, edgeSpringStiffness, edgeSprintCharacteristics);

            foreach (var link in graph.Links)
            {
                var nodeA = graph.Nodes.First(node => node.Id == link.Source);
                var nodeB = graph.Nodes.First(node => node.Id == link.Target);
                RectangleF? boundsA = nodeA.GetBoundingRect();
                RectangleF? boundsB = nodeB.GetBoundingRect();
                Position pA = GetCenter(boundsA);
                Position pB = GetCenter(boundsB);
                (Force fA, Force fB) = spring.CalculateForces(pA, pB);

                if (!ret.ContainsKey(nodeA))
                    ret.Add(nodeA, fA);
                else
                    ret[nodeA] += fA;

                if (!ret.ContainsKey(nodeB))
                    ret.Add(nodeB, fB);
                else
                    ret[nodeB] += fB;
            }

            return ret;
        }

        private Position GetCenter(RectangleF? rect)
        {
            if (rect is null)
                return new Position(0, 0);

            RectangleF r = (RectangleF)rect;
            return new Position((r.Top + r.Bottom) / 2.0, (r.Left + r.Right) / 2.0);
        }

        private double GetMass(DirectedGraphNode node) =>
            string.IsNullOrWhiteSpace(node.Label) ? 1 : node.Label.Length;
    }
}
