using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Aoc2023
{
    public class DijkstraGraph
    {
        // Example properties with getters and setters
        public Dictionary<string, List<(string neighbor, int weight)>> AdjacencyList
        {
            get => adj;
            set => adj = value;
        }

        private Dictionary<string, List<(string neighbor, int weight)>> adj = new();

        public record struct NodeState(int X, int Y, int DX, int DY, int Steps);

        public int Dijkstra(NodeState start, Grid grid, int min, int max)
        {
            int width = grid.Cols.Count;
            int height = grid.Rows;
            int startCost = int.Parse(grid.GridMap[(0, 0)].ToString());
            int endCost = int.Parse(grid.GridMap[(height - 1, width - 1)].ToString());
            HashSet<NodeState> visited = new HashSet<NodeState>();
            Dictionary<NodeState, int> distances = new();
            PriorityQueue<NodeState, int> pq = new();

            distances[start] = 0;
            pq.Enqueue(start, 0);

            while (pq.TryDequeue(out var state, out var cost))
            {
                if (state.X == height - 1 && state.Y == width - 1)
                    return distances[state];

                if (!visited.Add(state))
                    continue;

                foreach (var next in grid.NeighborsOfCoordDay17(state, min, max))
                {
                    int newX = next.X;
                    int newY = next.Y;

                    int _gridInt = int.Parse(grid.GridMap[(newX, newY)].ToString());
                    int newCost = distances[state] + _gridInt;
                    if (!distances.TryGetValue(next, out var existingCost) || newCost < existingCost)
                    {
                        distances[next] = newCost;
                        pq.Enqueue(next, newCost);
                    }
                }
            }
            return -1;
        }
    }
}