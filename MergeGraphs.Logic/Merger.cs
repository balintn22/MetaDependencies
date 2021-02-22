using System;
using System.Collections.Generic;
using System.Linq;

namespace MergeGraphs.Logic
{
    /// <summary>
    /// Merges one or more Dgml.DirectedGraphs into a single one.
    /// </summary>
    public interface IMerger
    {
        Dgml.DirectedGraph Merge(IEnumerable<Dgml.DirectedGraph> graphs);
    }

    public class Merger : IMerger
    {
        public Dgml.DirectedGraph Merge(IEnumerable<Dgml.DirectedGraph> graphs)
        {
            if (graphs == null || graphs.Count() == 0)
                throw new ArgumentException("At least one source graph is needed for merging.");

            Dgml.DirectedGraph result = new Dgml.DirectedGraph();

            var firstGraph = graphs.First();
            result.Categories = firstGraph.Categories;
            result.GraphDirection = firstGraph.GraphDirection;
            result.Styles = firstGraph.Styles;
            result.Properties = firstGraph.Properties;

            foreach (var graph in graphs)
            {
                // LINQ Union removes duplicates
                result.Nodes = result.Nodes == null ? graph.Nodes : result.Nodes.Union(graph.Nodes).ToArray();
                result.Links = result.Links == null ? graph.Links : result.Links.Union(graph.Links).ToArray();
            }

            // Remove not needed nodes.
            result.Nodes = result.Nodes
                .Where(n => n.Category != "Comment")
                .Where(n => !n.Label.ToLower().StartsWith("azure."))
                .Where(n => !n.Label.ToLower().StartsWith("microsoft."))
                .Where(n => !n.Label.ToLower().StartsWith("newtonsoft."))
                //.Where(n => !n.Label.ToLower().StartsWith("serilog"))
                .Where(n => !n.Label.ToLower().StartsWith("system."))
                .Where(n => n.Label != "Unused assemblies?")
                .Where(n => n.Label != "netstandard")
                .ToArray();

            var nodeIds = result.Nodes.Select(n => n.Id);

            // Remove duplicate and detached links
            result.Links = result.Links
                .Select(l => new Dgml.DirectedGraphLink { Source = l.Source, Target = l.Target })
                .Distinct()
                .Where(l => nodeIds.Contains(l.Source) && nodeIds.Contains(l.Target))
                .ToArray();

            return result;
        }
    }
}
