using GravityLayout.Logic.Physics;
using System;
using System.Collections.Generic;

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
        /// <param name="diGraph"></param>
        /// <returns>A new graph laid out such that edges cross as little as possible.</returns>
        public Dgml.DirectedGraph Layout(Dgml.DirectedGraph diGraph)
        {
            // TODO: Split to a set of fully connected sub-graphs
            // TODO: Layout individual fully connected graphs
            // TODO: Place them one under another

            throw new NotImplementedException();
        }

        /// <summary>
        /// Lays out a graph assuming it is fully connected.
        /// </summary>
        /// <param name="diGraph"></param>
        /// <returns></returns>
        private Dgml.DirectedGraph LayoutFullyConnected(Dgml.DirectedGraph diGraph)
        {
            // TODO: Use SpringLayouting to arrange the graph.

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
        private Dictionary<string, Force> CalculateNodeForces(
            Dgml.DirectedGraph diGraph,
            double edgeSpringLength,
            double edgeSpringStiffness,
            Spring.Characteristics edgeSprintCharacteristics,
            double antiGravitationalConstant)
        {
            var ret = new Dictionary<string, Force>();

            if (diGraph.Nodes is null)
                return ret;

            // TODO: Calculate spring forces by node
            // TODO: Calculate anti-gravity forces by node
            // TODO: Aggregate them
            throw new NotImplementedException();
        }

        private Dictionary<Dgml.DirectedGraphNode, Force> CalculateAntiGravitationalForcesByNode(
            Dgml.DirectedGraph graph,
            double antiGravitationalConstant)
        {
            var ret = new Dictionary<Dgml.DirectedGraphNode, Force>();
            AntiGravity antiGravity = new AntiGravity(antiGravitationalConstant);

            // Consider each node-pair exactly once
            for (int a = 0; a < graph.Nodes.Length - 1; a++)
            {
                Dgml.DirectedGraphNode nodeA = graph.Nodes[a];
                double massA = GetMass(nodeA);

                for (int b = a + 1; b < graph.Nodes.Length; b++)
                {
                    Dgml.DirectedGraphNode nodeB = graph.Nodes[b];
                    double massB = GetMass(nodeB);
                }
            }

            return ret;
        }

        private double GetMass(Dgml.DirectedGraphNode node) =>
            string.IsNullOrWhiteSpace(node.Label) ? 1 : node.Label.Length;
    }
}
