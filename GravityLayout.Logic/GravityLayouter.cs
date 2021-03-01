﻿using Dgml;
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
    ///     - edges are ropes, that attract nodes. Attraction force is proportional to length.
    ///     - anti-gravity repels nodes. This force is proportional to 1/distance^2
    ///     - not implemented: node boundaries should repel each other with a huge force, to avoid putting nodes on top of each other
    /// </summary>
    public class GravityLayouter
    {
        private double _ropeLength;
        private double _ropeStrength;
        private Rope.Characteristics _ropeCharacteristics;
        private double _antiGravitationalConstant;

        public GravityLayouter(
            double ropeLength,
            double ropeStrength,
            Rope.Characteristics ropeCharacteristics,
            double antiGravitationalConstant)
        {
            _ropeLength = ropeLength;
            _ropeStrength = ropeStrength;
            _ropeCharacteristics = ropeCharacteristics;
            _antiGravitationalConstant = antiGravitationalConstant;
        }

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
            foreach (var subGraph in connectedSubGraphs)
                LayoutFullyConnected(subGraph, 1, 100);

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
        /// Mutates the original.
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="stopShiftThreshold">If node shifts are all below this threshold, the iteration stops.</param>
        /// <param name="maxIterationCount">The maximum iteration count, after which layouting stops even if there are big node shifts.</param>
        /// <returns></returns>
        private void LayoutFullyConnected(
            DirectedGraph graph,
            double stopShiftThreshold,
            int maxIterationCount)
        {
            bool anyBigShifts = true;
            double maxShift;  // TODO: This can be removed. It is only there to visualize the amount of wobble
            for (int i = 0; i < maxIterationCount && anyBigShifts; i++)
            {
                anyBigShifts = false;
                maxShift = 0;

                var nodeForces = CalculateNodeForces(graph);
                foreach (var node in graph.Nodes)
                {
                    if (nodeForces.ContainsKey(node))
                    {
                        Force nodeForce = nodeForces[node];
                        Shift nodeShift = (Shift)(Vector)nodeForce;  // TODO: Any multipliers to represent running longer then a second?
                        ShiftNode(node, nodeShift);

                        if (!anyBigShifts && ((Vector)nodeShift).Length > stopShiftThreshold)
                            anyBigShifts = true;

                        maxShift = Math.Max(maxShift, ((Vector)nodeShift).Length);
                    }
                }
            }
        }

        /// <summary>
        /// Calculates the resultant forces at each node.
        /// </summary>
        /// <returns>Dictionary of resultant forces by node id.</returns>
        private Dictionary<DirectedGraphNode, Force> CalculateNodeForces(
            DirectedGraph graph)
        {
            var ret = new Dictionary<DirectedGraphNode, Force>();

            if (graph.Nodes is null)
                return ret;

            IEnumerable<(DirectedGraphNode, Force)> ropeForces = RopeForces(graph);
            IEnumerable<(DirectedGraphNode, Force)> agForces = AntiGravitationalForces(graph);
            var sumRopeForces = Force.Sum(ropeForces.Select(x => x.Item2)).Magnitude;
            var sumAgForces = Force.Sum(agForces.Select(x => x.Item2)).Magnitude;
            foreach (var nodeForce in ropeForces.Union(agForces))
            {
                var node = nodeForce.Item1;
                var force = nodeForce.Item2;
                if (!ret.ContainsKey(node))
                    ret.Add(node, force);
                else
                    ret[node] += force;
            }

            return ret;
        }

        private IEnumerable<(DirectedGraphNode, Force)> AntiGravitationalForces(DirectedGraph graph)
        {
            var ret = new Dictionary<DirectedGraphNode, Force>();
            AntiGravity antiGravity = new AntiGravity(_antiGravitationalConstant);

            // Consider each node-pair exactly once
            for (int a = 0; a < graph.Nodes.Length - 1; a++)
            {
                DirectedGraphNode nodeA = graph.Nodes[a];
                double massA = GetMass(nodeA);
                Position pA = GetCenter(nodeA.GetBoundingRect());

                for (int b = a + 1; b < graph.Nodes.Length; b++)
                {
                    DirectedGraphNode nodeB = graph.Nodes[b];
                    double massB = GetMass(nodeB);
                    Position pB = GetCenter(nodeB.GetBoundingRect());
                    (Force fA, Force fB) = antiGravity.CalculateForces(massA, massB, pA, pB);
                    yield return (nodeA, fA);
                    yield return (nodeB, fB);
                }
            }
        }

        private IEnumerable<(DirectedGraphNode, Force)> RopeForces(DirectedGraph graph)
        {
            var ret = new Dictionary<DirectedGraphNode, Force>();
            var rope = new Rope(_ropeLength, _ropeStrength, _ropeCharacteristics);

            foreach (var link in graph.Links)
            {
                var nodeA = graph.Nodes.First(node => node.Id == link.Source);
                var nodeB = graph.Nodes.First(node => node.Id == link.Target);
                RectangleF? boundsA = nodeA.GetBoundingRect();
                RectangleF? boundsB = nodeB.GetBoundingRect();
                Position pA = GetCenter(boundsA);
                Position pB = GetCenter(boundsB);
                (Force fA, Force fB) = rope.CalculateForces(pA, pB);
                yield return (nodeA, fA);
                yield return (nodeB, fB);
            }
        }

        private Position GetCenter(RectangleF? rect)
        {
            if (rect is null)
                return new Position(0, 0);

            RectangleF r = (RectangleF)rect;
            return new Position((r.Top + r.Bottom) / 2.0, (r.Left + r.Right) / 2.0);
        }

        /// <summary>Mutates a node by shifting its bounding rect.</summary>
        private void ShiftNode(DirectedGraphNode node, Shift shift)
        {
            RectangleF nodeBounds = node.GetBoundingRect() ?? new RectangleF(0, 0, 0, 0);
            nodeBounds.X += (float)shift.DX;
            nodeBounds.Y += (float)shift.DY;
            node.SetBoundingRect(nodeBounds);
        }

        private double GetMass(DirectedGraphNode node) =>
            string.IsNullOrWhiteSpace(node.Label) ? 1 : node.Label.Length;
    }
}
