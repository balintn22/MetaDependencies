using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using MergeGraphs.Logic.SpringLayouting.Physics;

namespace MergeGraphs.Logic
{
    /// <summary>
    /// Implements an algorithm that changes a graph's layout such, that
    /// edges do not cross each other as much as possible.
    /// Algorithm:
    ///     - edges are springs, that attract nodes. Attraction force is proportional to lentgh.
    ///     - anti-gravity repels nodes. This force is proportional to 1/distance^2
    ///     - not implemented: node boundaries should repel each other with a huge force, to avoid putting nodes on top of each other
    /// </summary>
    public class GraphSpringLayouting
    {
        private void Iterate(Dgml.DirectedGraph diGraph)
        {
        }

        /// <summary>
        /// Calculates the resultant forces at each node.
        /// </summary>
        /// <param name="uncompressedEdgeSpringLength">
        /// Specifies the length of edge-springs, where spring force is 0.
        /// When edge-springs are compressed or elongated, they generate a force proportional to
        /// their change in length and opposite in direction.
        /// </param>
        /// <param name="antiGravityStrength"></param>
        /// <returns>Dictionary of resultant forces by node id.</returns>
        private Dictionary<string, Force> CalculateNodeForces(
            Dgml.DirectedGraph diGraph
            double uncompressedEdgeSpringLength,
            double antiGravityStrength)
        {
            var ret = new Dictionary<string, Force>();

            if (diGraph.Nodes is null)
                return ret;

            // Unlinked nodes would be blown away by anti-gravity, so we need to handle them separately.
            IEnumerable<Dgml.DirectedGraphNode> unlinkedNodes = diGraph.Nodes
                .Where(node => )
        }
    }
}
