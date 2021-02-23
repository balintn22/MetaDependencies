using System;
using System.Collections.Generic;
using System.Linq;

namespace MergeGraphs.Logic
{
    public class DiGraphHelper
    {
        /// <summary>
        /// Get a collection of directional neighbors.
        /// The result is a dictionary, where
        ///     key is the id of the source node
        ///     value is a list of drectional neighbor node ids.
        /// All source nodes are listed, even if they have no di-neighbors.
        /// </summary>
        public static Dictionary<string, List<string>> GetDiNeighbors(Dgml.DirectedGraph diGraph)
        {
            if (diGraph == null)
                throw new ArgumentNullException(nameof(diGraph));

            if (diGraph.Nodes == null || diGraph.Links == null)
                return new Dictionary<string, List<string>>();

            var ret = diGraph.Nodes.ToDictionary(n => n.Id, n => new List<string>());

            foreach (Dgml.DirectedGraphLink link in diGraph.Links)
            {
                if(ret.ContainsKey(link.Source))
                    ret[link.Source].Add(link.Target);
            }

            return ret;
        }

        /// <summary>
        /// Finds nodes connected by paths, longer than 1 link.
        /// </summary>
        /// <param name="diGraph"></param>
        /// <returns>A list of node-pairs, that are connected by paths, that are at least 2 links long.</returns>
        public static List<DiNodePair> FindLongPaths(Dgml.DirectedGraph diGraph)
        {
            if (diGraph == null)
                throw new ArgumentNullException(nameof(diGraph));

            if (diGraph.Nodes == null || diGraph.Links == null)
                return new List<DiNodePair>();

            var diNeighbors = GetDiNeighbors(diGraph);

            List<DiNodePair> pathsOfLength1 = diGraph.Links
                .Select(l => new DiNodePair(l.Source, l.Target)).ToList();

            // Build the initial set of long paths using paths of length 2.
            // Paths of length 2 are created from paths of length 1, extending them with each possible di-neighbor from their end.
            IEnumerable<DiNodePair> longPaths = new List<DiNodePair>();
            int i = 0;
            foreach (DiNodePair pathOfLength1 in pathsOfLength1)
            {
                // Extend this path to each di-neighbor of the endpoint.
                var extendedLongPaths = diNeighbors[pathOfLength1.EndId]
                    .Select(end => new DiNodePair(pathOfLength1.StartId, end))
                    .ToList();
                var newExtendedLongPaths = extendedLongPaths.Except(longPaths).ToList();
                longPaths = longPaths.Union(newExtendedLongPaths).ToList();
            }

            // Keep extending the long paths until no more extension can be found.
            int loopCount = 0;
            bool anyChanges = true;
            while (anyChanges && loopCount++ < 100)
            {
                anyChanges = false;
                foreach (DiNodePair longPath in longPaths)
                {
                    // Extend this path to each di-neighbor of the endpoint.
                    var extendedLongPaths = diNeighbors[longPath.EndId]
                        .Select(end => new DiNodePair(longPath.StartId, end))
                        .ToList();
                    var newExtendedLongPaths = extendedLongPaths.Except(longPaths).ToList();
                    longPaths = longPaths.Union(newExtendedLongPaths).ToList();
                    anyChanges = anyChanges || newExtendedLongPaths.Count() > 0;
                }
            }

            return longPaths.ToList();
        }

        /// <summary>
        /// A shortcut is a single link that connects two nodes that are also connected by a less direct route.
        /// This method lists those shortcuts.
        /// </summary>
        /// <returns>A list of Links that are shortcuts of longer paths.</returns>
        public static IEnumerable<Dgml.DirectedGraphLink> GetShortcuts(Dgml.DirectedGraph diGraph)
        {
            if (diGraph.Nodes == null || diGraph.Links == null)
                return new List<Dgml.DirectedGraphLink>();

            var longPaths = FindLongPaths(diGraph);
            return diGraph.Links.Where(l => longPaths
                .Any(longPath => longPath.StartId == l.Source && longPath.EndId == l.Target));
        }

        /// <summary>
        /// A shortcut is a link that connects two nodes that are also connected by a less direct route.
        /// This method finds and removes those shortcuts.
        /// Note: mutates the object in the argument. Also returns the mutated object.
        /// </summary>
        public static Dgml.DirectedGraph RemoveShortcuts(Dgml.DirectedGraph diGraph)
        {
            if (diGraph.Nodes == null || diGraph.Links == null)
                return diGraph;

            var shortcuts = GetShortcuts(diGraph);
            diGraph.Links = diGraph.Links.Except(shortcuts).ToArray();

            return diGraph;
        }
    }
}
