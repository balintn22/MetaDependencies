using Dgml;
using GravityLayout.Logic.Geometry;
using GravityLayout.Logic.Physics;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GravityLayout.Logic
{
    public class GraphPhysics
    {
        private double _ropeLength;
        private double _ropeStrength;
        private Rope.Characteristics _ropeCharacteristics;
        private double _antiGravitationalConstant;

        public GraphPhysics(
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

        public IEnumerable<(DirectedGraphNode, Force)> AntiGravitationalForces(DirectedGraph graph)
        {
            var ret = new Dictionary<DirectedGraphNode, Force>();
            AntiGravity antiGravity = new AntiGravity(_antiGravitationalConstant);

            // Consider each node-pair exactly once
            for (int a = 0; a < graph.Nodes.Length - 1; a++)
            {
                DirectedGraphNode nodeA = graph.Nodes[a];
                double massA = NodeMass(nodeA);
                Position pA = NodeCenter(nodeA);

                for (int b = a + 1; b < graph.Nodes.Length; b++)
                {
                    DirectedGraphNode nodeB = graph.Nodes[b];
                    double massB = NodeMass(nodeB);
                    Position pB = NodeCenter(nodeB);
                    (Force fA, Force fB) = antiGravity.CalculateForces(massA, massB, pA, pB);
                    yield return (nodeA, fA);
                    yield return (nodeB, fB);
                }
            }
        }

        public IEnumerable<(DirectedGraphNode, Force)> RopeForces(DirectedGraph graph)
        {
            var ret = new Dictionary<DirectedGraphNode, Force>();
            var rope = new Rope(_ropeLength, _ropeStrength, _ropeCharacteristics);

            foreach (var link in graph.Links)
            {
                var nodeA = graph.Nodes.First(node => node.Id == link.Source);
                var nodeB = graph.Nodes.First(node => node.Id == link.Target);
                Position pA = NodeCenter(nodeA);
                Position pB = NodeCenter(nodeB);
                (Force fA, Force fB) = rope.CalculateForces(pA, pB);
                link.Label = $"F={fA.Magnitude}";   // Debug: display rope force on link
                yield return (nodeA, fA);
                yield return (nodeB, fB);
            }
        }

        /// <summary>
        /// Calculates the resultant forces at each node.
        /// </summary>
        /// <returns>Dictionary of resultant forces by node id.</returns>
        public Dictionary<DirectedGraphNode, Force> NodeForces(DirectedGraph graph)
        {
            var ret = new Dictionary<DirectedGraphNode, Force>();

            if (graph.Nodes is null)
                return ret;

            IEnumerable<(DirectedGraphNode, Force)> ropeForces = RopeForces(graph);
            IEnumerable<(DirectedGraphNode, Force)> agForces = AntiGravitationalForces(graph);
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

        private Position NodeCenter(DirectedGraphNode node)
        {
            RectangleF? rect = node?.GetBoundingRect();
            if (rect is null)
                return new Position(0, 0);

            RectangleF r = (RectangleF)rect;
            return new Position((r.Left + r.Right) / 2.0, (r.Top + r.Bottom) / 2.0);
        }

        private double NodeMass(DirectedGraphNode node) =>
            string.IsNullOrWhiteSpace(node.Label) ? 1 : node.Label.Length;
    }
}
