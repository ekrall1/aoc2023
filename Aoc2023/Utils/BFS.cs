using System.Linq;
using System.Runtime.ConstrainedExecution;

namespace Aoc2023
{
    public interface IGridBFSSearch<T, S>
    {
        Grid Grid { get; set; }
        T Start { get; }
        S Search();
    }

    public class Day21BFS : IGridBFSSearch<(int, int), Dictionary<long, long>>
    {
        public Grid Grid { get; set; }
        public (int, int) Start { get; set; }
        private readonly HashSet<((int, int) coord, int steps)> Visited = new();
        private readonly HashSet<(int, int)> VisitedCoord = new();
        private readonly Queue<((int, int) coord, int steps)> BFSQueue = new();
        public Dictionary<long, long> Plots = new();
        public Dictionary<int, int> ResultCoordsEvenCount = new();
        public Dictionary<int, int> ResultCoordsOddCount = new();
        public HashSet<(int, int)> ResultCoordsOdd = new();
        public HashSet<(int, int)> ResultCoordsEven = new();
        private long MaxSteps { get; set; }
        private int Part { get; set; }

        public Day21BFS(Grid grid, (int, int) start, long maxSteps, int part)
        {
            Grid = grid;
            Start = start;
            BFSQueue.Enqueue((start, 0));
            MaxSteps = maxSteps;
            Part = part;
        }

        public Dictionary<long, long> Search()
        {
            int rows = Grid.Rows;
            int cols = Grid.Cols.Count;
            while (BFSQueue.Count > 0)
            {
                var ((y, x), steps) = BFSQueue.Dequeue();

                if (steps == MaxSteps + 1 || Visited.Contains(((y, x), steps)))
                    continue;
                Visited.Add(((y, x), steps));

                if (Part == 2 && VisitedCoord.Contains((y, x)))
                    continue;
                VisitedCoord.Add((y, x));

                if (!Plots.TryGetValue(steps, out var set))
                {
                    Plots[steps] = 0;
                }
                Plots[steps] += 1;

                IEnumerable<(int, int)> neighbors;
                IEnumerable<(int, int)> queueNeighbors;
                if (Part == 1)
                {
                    neighbors = Grid.NeighborsOfCoord((y, x));
                    queueNeighbors = neighbors;
                }
                else
                {
                    var infiniteNeighbors = Grid.InfiniteRepeatingNeighborsOfCoord((y, x), rows, cols);
                    queueNeighbors = infiniteNeighbors.Unwrapped;
                    neighbors = infiniteNeighbors.Wrapped;
                }

                using var neighborEnumerator = neighbors.GetEnumerator();
                using var queueEnumerator = queueNeighbors.GetEnumerator();

                while (neighborEnumerator.MoveNext() && queueEnumerator.MoveNext())
                {
                    var neighbor = neighborEnumerator.Current;
                    var queueNeighbor = queueEnumerator.Current;

                    if (Grid.GridMap[neighbor] != '#')
                    {
                        BFSQueue.Enqueue((queueNeighbor, steps + 1));
                    }
                }
            }

            return Plots;
        }
    }
}