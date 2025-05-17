using System.Runtime.ConstrainedExecution;

namespace Aoc2023
{
    public interface IGridBFSSearch<T, S>
    {
        Grid Grid { get; set; }
        T Start { get; }
        S Search();
    }

    public class Day21BFS : IGridBFSSearch<(int, int), long>
    {
        public Grid Grid { get; set; }
        public (int, int) Start { get; set; }
        private readonly HashSet<((int, int) coord, int steps)> Visited = new();
        private readonly Queue<((int, int) coord, int steps)> BFSQueue = new();
        private readonly HashSet<(int, int)> ResultCoords = new();
        private long MaxSteps { get; set; }

        public Day21BFS(Grid grid, (int, int) start, long maxSteps)
        {
            Grid = grid;
            Start = start;
            BFSQueue.Enqueue((start, 0));
            MaxSteps = maxSteps;
        }

        public long Search()
        {
            int rows = Grid.Rows;
            int cols = Grid.Cols.Count;

            while (BFSQueue.Count > 0)
            {
                var ((y, x), steps) = BFSQueue.Dequeue();
                if (steps > MaxSteps)
                    continue;

                if (!Visited.Add(((x, y), steps)))
                    continue;

                if (steps == MaxSteps)
                {
                    ResultCoords.Add((x, y));
                    continue;
                }

                List<(int, int)> neighbors = Grid.NeighborsOfCoord((y, x));
                foreach ((int, int) neighbor in neighbors)
                {
                    if (Grid.GridMap[neighbor] != '#')
                        BFSQueue.Enqueue((neighbor, steps + 1));
                }
            }
            return ResultCoords.Count;
        }
    }
}