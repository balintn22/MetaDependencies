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
    ///     - edges are ropes, that attract nodes. Attraction force is proportional to length.
    ///     - anti-gravity repels nodes. This force is proportional to 1/distance^2
    ///     - not implemented: node boundaries should repel each other with a huge force, to avoid putting nodes on top of each other
    /// </summary>
    public class GravityLayouter
    {
        private GraphPhysics _physics;
        /// <summary>Used to slow shifts down, regardless of large forces.</summary>
        private const double _maxNodeShift = 500;

        public GravityLayouter(
            double ropeLength,
            double ropeStrength,
            Rope.Characteristics ropeCharacteristics,
            double antiGravitationalConstant)
        {
            _physics = new GraphPhysics(
                ropeLength,
                ropeStrength,
                ropeCharacteristics,
                antiGravitationalConstant);
        }

        /// <summary>
        /// Lays out the graph blowing nodes apart so that edges cross as little as possible.
        /// Graph may not be fully connected. Connected sub-graphs are layed out individually and placed
        /// one below another.
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="stopShiftThreshold">If node shifts are all below this threshold, the iteration stops.</param>
        /// <param name="maxIterationCount">The maximum iteration count, after which layouting stops even if there are big node shifts.</param>
        /// <param name="iterationCallBack">Optional. An action called after each iteration.</param>
        /// <returns>A new graph laid out such that edges cross as little as possible.</returns>
        public DirectedGraph Layout(
            DirectedGraph graph,
            double stopShiftThreshold,
            int maxIterationCount,
            Action<IterationResult> iterationCallBack)
        {
            // Split to a set of fully connected sub-graphs
            var parts = GraphManipulator.SplitToConnectedSubGraphs(graph).ToList();
            return LayoutFullyConnectedGraphs(
                parts, stopShiftThreshold, maxIterationCount, iterationCallBack);
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
            float currentBottom = graphs[0].GetBoundingRect().Bottom;

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
                node.Shift(0, dy);
        }

        /// <summary>
        /// Lays out a set of graphs, where each part must be fully connected,
        /// but parts are not connected.
        /// </summary>
        /// <param name="parts">A collection of individually fully connected sub-graphs.</param>
        /// <param name="stopShiftThreshold">If node shifts are all below this threshold, the iteration stops.</param>
        /// <param name="maxIterationCount">The maximum iteration count, after which layouting stops even if there are big node shifts.</param>
        /// <param name="iterationCallback">Optional. An action called after each iteration.</param>
        private DirectedGraph LayoutFullyConnectedGraphs(
            IEnumerable<DirectedGraph> parts,
            double stopShiftThreshold,
            int maxIterationCount,
            Action<IterationResult> iterationCallback)
        {
            DirectedGraph merged = null;

            for (int i = 1; i < maxIterationCount; i++)
            {
                var iResult = new IterationResult { Count = i };

                // Layout individual fully connected graphs
                foreach (var subGraph in parts)
                {
                    var partIResult = FullyConnectedGraphIterationStep(subGraph);
                    iResult.MaxShift = Math.Max(iResult.MaxShift, partIResult.MaxShift);
                    iResult.MaxForce = Math.Max(iResult.MaxForce, partIResult.MaxForce);
                }

                // Place them one under another
                Stack(parts.ToList());

                // Merge them into a single graph
                merged = Merge(parts.ToList());

                iResult.Graph = merged;
                if(iterationCallback != null)
                    iterationCallback(iResult);

                if (iResult.MaxShift < stopShiftThreshold)
                    break;
            }

            if (merged != null)
            {
                foreach (var link in merged.Links)
                    link.Label = null;
            }
            return merged;
        }

        /// <summary>
        /// Performs one iteration of layouting of a graph assuming it is fully connected.
        /// Mutates the original.
        /// </summary>
        private IterationResult FullyConnectedGraphIterationStep(DirectedGraph graph)
        {
            var ret = new IterationResult();
            var nodeForces = _physics.NodeForces(graph);
            foreach (var node in graph.Nodes)
            {
                if (nodeForces.ContainsKey(node))
                {
                    Force nodeForce = nodeForces[node];
                    Vector shift = (Vector)nodeForce;  // TODO: Any multipliers to represent running longer then a second?
                    // TEST: Limit node shift
                    shift.Length = Math.Min(_maxNodeShift, shift.Length);
                    node.Shift(shift.X, shift.Y);

                    ret.MaxShift = Math.Max(ret.MaxShift, shift.Length);
                    ret.MaxForce = Math.Max(ret.MaxForce, nodeForce.Magnitude);
                }
            }
            ret.Graph = graph;
            return ret;
        }
    }
}
