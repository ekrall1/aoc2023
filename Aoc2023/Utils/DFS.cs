namespace Aoc2023
{
    using CoordWithDirection = System.ValueTuple<int, int, (int, int)>;

    public interface IAocGridDFS
    {
        (int, int) Start { get; }
        Grid Grid { get; }
        HashSet<(int, int)> Search(Stack<(int, int)> stack);
    }
    public interface IAocGridDFSWithDirection
    {
        CoordWithDirection Start { get; }
        HashSet<CoordWithDirection> Search(Stack<CoordWithDirection> stack);
    }

    public abstract class AocGridDFS : IAocGridDFS
    {
        public (int, int) Start { get; }
        public Grid Grid { get; }

        protected AocGridDFS(Grid grid, (int, int) start)
        {
            Start = start;
            Grid = grid;
        }

        protected abstract List<(int, int)> Filter((int, int) cur, List<(int, int)> neighbors);

        public HashSet<(int, int)> Search(Stack<(int, int)> stack)
        {
            var visited = new HashSet<(int, int)>();
            stack.Push(Start);

            while (stack.Count > 0)
            {
                var cur = stack.Pop();

                if (visited.Contains(cur))
                {
                    if (cur == Start) break;
                    continue;
                }

                var neighbors = Grid.NeighborsOfCoord(cur);
                var filtered = Filter(cur, neighbors);

                foreach (var item in filtered)
                    if (!visited.Contains(item))
                        stack.Push(item);

                visited.Add(cur);
            }

            return visited;
        }
    }

    public class Day10GridDFS : AocGridDFS
    {
        public Day10GridDFS(Grid grid, (int, int) start) : base(grid, start) { }

        protected override List<(int, int)> Filter((int, int) cur, List<(int, int)> neighbors)
        {
            var currentChar = Grid.GridMap[cur];

            return neighbors
                .Where(n => Grid.GridMap[n] != '.')
                .Where(n =>
                {
                    return currentChar switch
                    {
                        '|' => (n.Item1 - 1, n.Item2) == cur || (n.Item1 + 1, n.Item2) == cur,
                        '-' => (n.Item1, n.Item2 - 1) == cur || (n.Item1, n.Item2 + 1) == cur,
                        'L' => (n.Item1 + 1, n.Item2) == cur || (n.Item1, n.Item2 - 1) == cur,
                        'J' => (n.Item1 + 1, n.Item2) == cur || (n.Item1, n.Item2 + 1) == cur,
                        '7' => (n.Item1 - 1, n.Item2) == cur || (n.Item1, n.Item2 + 1) == cur,
                        'F' => (n.Item1 - 1, n.Item2) == cur || (n.Item1, n.Item2 - 1) == cur,
                        'S' => Grid.GridMap[n] != '.',
                        _ => false,
                    };
                }).ToList();
        }
    }

    public class Day14GridDFS : AocGridDFS
    {
        public Day14GridDFS(Grid grid, (int, int) start) : base(grid, start) { }

        protected override List<(int, int)> Filter((int, int) cur, List<(int, int)> neighbors)
        {
            return neighbors.Where(n => Grid.GridMap[n] == 'O').ToList();
        }
    }

    public class DefaultGridDFS : AocGridDFS
    {
        public DefaultGridDFS(Grid grid, (int, int) start) : base(grid, start) { }

        protected override List<(int, int)> Filter((int, int) cur, List<(int, int)> neighbors)
        {
            // No filtering; return neighbors unchanged
            return neighbors;
        }
    }

    public class DefaultGridDFSWithDirection : IAocGridDFSWithDirection
    {
        public CoordWithDirection Start { get; private set; }
        private Grid _Grid;

        public DefaultGridDFSWithDirection(Grid grid, CoordWithDirection start)
        {
            Start = start;
            _Grid = grid;
        }

        public HashSet<CoordWithDirection> Search(Stack<CoordWithDirection> stack)
        {
            HashSet<CoordWithDirection> visited = new();

            List<CoordWithDirection> _start =
                _Grid.GridMap[(Start.Item1, Start.Item2)] == '.'
                    ? [Start]
                    : GetRotations(Start);

            foreach (var coord in _start)
            {
                stack.Push(coord);
            }

            while (stack.Count > 0)
            {
                CoordWithDirection cur = stack.Pop();
                if (visited.Contains(cur))
                {
                    continue;
                }

                List<CoordWithDirection> neighbors = _Grid.NeighborsOfCoordWithDirection(cur);
                var filteredNeighbors = DefaultFilter(cur, neighbors);

                foreach (var item in filteredNeighbors)
                {
                    if (!visited.Contains(item))
                    {
                        stack.Push(item);
                    }
                }

                visited.Add(cur);
            }

            return visited;
        }

        private List<CoordWithDirection> GetRotations(CoordWithDirection cur)
        {
            var gridChar = _Grid.GridMap[(cur.Item1, cur.Item2)];
            var nxtDirections = new List<CoordWithDirection>();

            switch (gridChar)
            {
                case '\\':
                    nxtDirections.Add(RotateClockwise(cur));
                    break;

                case '/':
                    nxtDirections.Add(RotateCounterClockwise(cur));
                    break;

                case '-':
                    if (cur.Item3 is (not 0, 0) and (int dy, 0))
                        nxtDirections.AddRange(SplitDirection(cur, (0, 1), (0, -1)));
                    else
                        nxtDirections.Add(cur);
                    break;

                case '|':
                    if (cur.Item3 is (0, int dx))
                        nxtDirections.AddRange(SplitDirection(cur, (1, 0), (-1, 0)));
                    else
                        nxtDirections.Add(cur);
                    break;
            }

            return nxtDirections;
        }

        private List<CoordWithDirection> DefaultFilter(CoordWithDirection cur, List<CoordWithDirection> neighbors)
        {
            if (neighbors.Count == 0)
                return [];

            var neighborChar = _Grid.GridMap[(neighbors[0].Item1, neighbors[0].Item2)];

            return neighborChar == '.' ? neighbors : GetRotations(neighbors[0]);
        }

        private CoordWithDirection RotateClockwise(CoordWithDirection coord)
        {
            var rotated = (coord.Item3.Item2, coord.Item3.Item1);
            return (coord.Item1, coord.Item2, rotated);
        }

        private CoordWithDirection RotateCounterClockwise(CoordWithDirection coord)
        {
            var rotated = (-coord.Item3.Item2, -coord.Item3.Item1);
            return (coord.Item1, coord.Item2, rotated);
        }

        private List<CoordWithDirection> SplitDirection(CoordWithDirection coord, (int, int) rotated1, (int, int) rotated2)
        {
            return
            [
                (coord.Item1, coord.Item2, rotated1),
                (coord.Item1, coord.Item2, rotated2)
            ];
        }
    }

    //Factories
    public static class GridDfsFactory
    {
        public static IAocGridDFS Create(string puzzleDay, Grid grid, (int, int) start) => puzzleDay switch
        {
            "Day10" => new Day10GridDFS(grid, start),
            "Day14" => new Day14GridDFS(grid, start),
            _ => new DefaultGridDFS(grid, start)
        };
    }
    public static class GridDfsWithDirectionFactory
    {
        public static IAocGridDFSWithDirection Create(Grid grid, CoordWithDirection start, string puzzleDay)
        {
            return new DefaultGridDFSWithDirection(grid, start);
        }
    }


}