namespace Aoc2023
{
    using CoordWithDirection = System.ValueTuple<int, int, (int, int)>;
    public class AocGridDFS
    {
        public (int, int) Start { get; set; }
        public Grid Grid { get; private set; }
        public Func<(int, int), List<(int, int)>, List<(int, int)>> FilterFunc { get; private set; }

        public AocGridDFS(Grid grid, (int, int) start, string puzzleDay)
        {
            Start = start;
            Grid = grid;

            FilterFunc = puzzleDay switch
            {
                "Day10" => Day10Filter,
                "Day14" => Day14Filter,
                _ => DefaultFilter
            };
        }

        public HashSet<(int, int)> Search(Stack<(int, int)> stack)
        {
            HashSet<(int, int)> visited = new HashSet<(int, int)>();

            stack.Push(Start);

            while (stack.Count > 0)
            {
                (int, int) cur = stack.Pop();
                if (visited.Contains(cur))
                {
                    if (cur == Start) break;  // prevent cycle
                    continue;
                }
                else
                {
                    List<(int, int)> neighbors = Grid.NeighborsOfCoord(cur);
                    var filteredNeighbors = FilterFunc(cur, neighbors);
                    foreach (var item in filteredNeighbors)
                    {
                        if (!visited.Contains(item))
                        {
                            stack.Push(item);
                        }
                    }
                }

                visited.Add(cur);

            }
            return visited;
        }

        public List<(int, int)> Day10Filter((int, int) cur, List<(int, int)> neighbors)
        {
            var currentChar = Grid.GridMap[cur];

            List<(int, int)> nextNeighbor = neighbors.Where(neighbor => Grid.GridMap[neighbor] != '.').Where(neighbor =>
            {
                var nxt = currentChar switch
                {
                    '.' => false,
                    '|' => (neighbor.Item1 - 1, neighbor.Item2) == cur || (neighbor.Item1 + 1, neighbor.Item2) == cur,
                    '-' => (neighbor.Item1, neighbor.Item2 - 1) == cur || (neighbor.Item1, neighbor.Item2 + 1) == cur,
                    'L' => (neighbor.Item1 + 1, neighbor.Item2) == cur || (neighbor.Item1, neighbor.Item2 - 1) == cur,
                    'J' => (neighbor.Item1 + 1, neighbor.Item2) == cur || (neighbor.Item1, neighbor.Item2 + 1) == cur,
                    '7' => (neighbor.Item1 - 1, neighbor.Item2) == cur || (neighbor.Item1, neighbor.Item2 + 1) == cur,
                    'F' => (neighbor.Item1 - 1, neighbor.Item2) == cur || (neighbor.Item1, neighbor.Item2 - 1) == cur,
                    'S' => Grid.GridMap[neighbor] != '.',
                    _ => false,
                };
                return nxt;
            }).ToList();

            return nextNeighbor;
        }

        public List<(int, int)> Day14Filter((int, int) cur, List<(int, int)> neighbors)
        {
            var currentChar = Grid.GridMap[cur];

            List<(int, int)> nextNeighbor = [.. neighbors.Where(neighbor => Grid.GridMap[neighbor] == 'O')];

            return nextNeighbor;
        }

        public List<(int, int)> DefaultFilter((int, int) cur, List<(int, int)> neighbors) { return []; }

    }
    public class AocGridDFSWithDirection
    {
        public CoordWithDirection Start;
        private Grid _Grid;

        private Func<CoordWithDirection, List<CoordWithDirection>, List<CoordWithDirection>> _FilterFunc;

        public AocGridDFSWithDirection(Grid grid, CoordWithDirection start)
        {
            Start = start;
            _Grid = grid;

            _FilterFunc = DefaultFilter;
        }

        public List<CoordWithDirection> GetRotations(CoordWithDirection cur)
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
                    // Split into two directions
                    if (cur.Item3 is (not 0, 0) and (int dy, 0))
                        nxtDirections.AddRange(SplitDirection(cur, (0, 1), (0, -1)));
                    else
                        nxtDirections.Add(cur); // continue straight
                    break;

                case '|':
                    // Split into two directions
                    if (cur.Item3 is (0, int dx))
                        nxtDirections.AddRange(SplitDirection(cur, (1, 0), (-1, 0)));
                    else
                        nxtDirections.Add(cur);
                    break;
            }

            return nxtDirections;
        }

        public List<CoordWithDirection> DefaultFilter(CoordWithDirection cur, List<CoordWithDirection> neighbors)
        {
            if (neighbors.Count == 0)
            {
                return [];
            }

            var neighborChar = _Grid.GridMap[(neighbors[0].Item1, neighbors[0].Item2)];

            if (neighborChar == '.')
                return neighbors;

            var nxtDirections = GetRotations(neighbors[0]);
            return nxtDirections;
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
            var newNeighbors = new List<CoordWithDirection>
            {
                (coord.Item1, coord.Item2, rotated1),
                (coord.Item1, coord.Item2, rotated2)
            };
            return newNeighbors;
        }

        public HashSet<CoordWithDirection> Search(Stack<CoordWithDirection> stack)
        {
            HashSet<CoordWithDirection> visited = new HashSet<CoordWithDirection>();

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
                else
                {
                    List<CoordWithDirection> neighbors = _Grid.NeighborsOfCoordWithDirection(cur);
                    var filteredNeighbors = _FilterFunc(cur, neighbors);
                    foreach (var item in filteredNeighbors)
                    {
                        if (!visited.Contains(item))
                        {
                            stack.Push(item);
                        }
                    }
                }

                visited.Add(cur);

            }
            return visited;
        }

    }
}