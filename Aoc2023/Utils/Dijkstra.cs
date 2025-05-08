using System;
using System.Collections.Generic;

namespace Aoc2023
{
    public class DijkstraGraph
    {
        private Dictionary<string, List<(string neighbor, int weight)>> adj = new();

        public void AddEdge(string from, string to, int weight)
        {
            if (!adj.ContainsKey(from)) adj[from] = new();
            if (!adj.ContainsKey(to)) adj[to] = new();

            adj[from].Add((to, weight));
            // adj[to].Add((from, weight));
        }

        public Dictionary<string, int> Dikstra(string start)
        {
            Dictionary<string, int> distances = new();
            PriorityQueue<string, int> pq = new();

            foreach (var node in adj.Keys)
                distances[node] = int.MaxValue;

            distances[start] = 0;
            pq.Enqueue(start, 0);

            while (pq.Count > 0 {
                var cur = pq.Dequeue();

                foreach (var (neighbor, weight) in adj[cur])
                {
                    int newDist = distances[cur] + weight;
                    if (newDist < distances[neighbor])
                    {
                        distances[neighbor] = newDist;
                        pq.Enqueue(neighbor, newDist);
                    }
                }
            }
            return distances;
        }
    }
}