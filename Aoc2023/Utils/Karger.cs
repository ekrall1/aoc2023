using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2023
{
    public interface IKargerMinCut
    {
        (int CutSize, HashSet<string> Supernode1, HashSet<string> Supernode2) FindMinCutWithPartition(
            Dictionary<string, HashSet<string>> originalGraph);
    }

    public class KargerMinCut : IKargerMinCut
    {
        private record Edge(string A, string B);

        private static Random rng = new();

        // Returns the cut size and the two supernodes (each as a set of original nodes)
        public (int CutSize, HashSet<string> Supernode1, HashSet<string> Supernode2) FindMinCutWithPartition(
            Dictionary<string, HashSet<string>> originalGraph)
        {
            var graph = CloneGraph(originalGraph);
            // Each supernode is a set of original nodes
            var supernodes = graph.Keys.ToDictionary(n => n, n => new HashSet<string> { n });

            while (graph.Count != 2)
            {
                var allEdges = GetAllEdges(graph)
                    .Where(e => graph.ContainsKey(e.A) && graph.ContainsKey(e.B))
                    .ToList();

                if (allEdges.Count == 0)
                    throw new InvalidOperationException("No valid edges left to merge.");

                var edge = allEdges[rng.Next(allEdges.Count)];
                MergeNodes(graph, supernodes, edge.A, edge.B);
            }

            var remaining = supernodes.Values.ToList();
            int cutSize = graph.First().Value.Count;
            return (cutSize, remaining[0], remaining[1]);
        }

        private void MergeNodes(
            Dictionary<string, HashSet<string>> graph,
            Dictionary<string, HashSet<string>> supernodes,
            string a, string b)
        {
            var lst = graph[b].ToList();
            foreach (var neighbor in lst)
            {
                if (neighbor != a && graph.ContainsKey(neighbor))
                {
                    graph[a].Add(neighbor);
                    graph[neighbor].Remove(b);
                    graph[neighbor].Add(a);
                }
            }

            graph.Remove(b);
            graph[a].RemoveWhere(n => n == a);

            // Merge supernodes
            supernodes[a].UnionWith(supernodes[b]);
            supernodes.Remove(b);
        }

        private List<Edge> GetAllEdges(Dictionary<string, HashSet<string>> graph)
        {
            var edges = new List<Edge>();
            foreach (var kvp in graph)
            {
                foreach (var neighbor in kvp.Value)
                {
                    if (string.Compare(kvp.Key, neighbor) < 0)
                        edges.Add(new Edge(kvp.Key, neighbor));
                }
            }
            return edges;
        }

        private Dictionary<string, HashSet<string>> CloneGraph(Dictionary<string, HashSet<string>> original)
        {
            return original.ToDictionary(
                kvp => kvp.Key,
                kvp => new HashSet<string>(kvp.Value)
            );
        }
    }
}
