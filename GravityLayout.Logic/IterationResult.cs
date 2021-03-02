using Dgml;
using System.Drawing;

namespace GravityLayout.Logic
{
    /// <summary>
    /// Contains information about a layout iteration.
    /// </summary>
    public class IterationResult
    {
        /// <summary>Sequence number of the iteration.</summary>
        public int Count { get; set; }

        /// <summary>
        /// Contains the state of the graph after the iteration step.
        /// May contain a not fully connected graph.
        /// </summary>
        public DirectedGraph Graph { get; set; }

        /// <summary>
        /// Contains the maximum shift distance that a node has made
        /// during this iteration.
        /// </summary>
        public double MaxShift { get; set; }

        /// <summary>
        /// Contains the maximum force that was experienced
        /// during this iteration, before shifts were applied.
        /// </summary>
        public double MaxForce { get; set; }

        public override string ToString()
        {
            RectangleF? bounds = Graph?.GetBoundingRect();
            return $"{Count}. Size:{bounds?.Width}x{bounds?.Height} MaxShift:{MaxShift} MaxForce:{MaxForce}";
        }
    }
}
