namespace Aoc2023
{
    using CoordWithDirection = System.ValueTuple<int, int, (int, int)>;
    public class AocGridDFS
    {
        public (int, int) Start;
        private Grid _Grid;

        private Func<(int, int), List<(int, int)>, List<(int, int)>> _FilterFunc;

        public AocGridDFS(Grid grid, (int, int) start, string puzzleDay)
        {
            Start = start;
            _Grid = grid;

            _FilterFunc = puzzleDay switch
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
                    List<(int, int)> neighbors = _Grid.NeighborsOfCoord(cur);
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

        public List<(int, int)> Day10Filter((int, int) cur, List<(int, int)> neighbors)
        {
            var currentChar = _Grid.gridMap[cur];

            List<(int, int)> nextNeighbor = neighbors.Where(neighbor => _Grid.gridMap[neighbor] != '.').Where(neighbor =>
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
                    'S' => _Grid.gridMap[neighbor] != '.',
                    _ => false,
                };
                return nxt;
            }).ToList();

            return nextNeighbor;
        }

        public List<(int, int)> Day14Filter((int, int) cur, List<(int, int)> neighbors)
        {
            var currentChar = _Grid.gridMap[cur];

            List<(int, int)> nextNeighbor = [.. neighbors.Where(neighbor => _Grid.gridMap[neighbor] == 'O')];

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

        public List<CoordWithDirection> DefaultFilter(CoordWithDirection cur, List<CoordWithDirection> neighbors)
        {
            var neighborChar = _Grid.gridMap[(neighbors[0].Item1, neighbors[0].Item2)];
            if (neighborChar == '.')
                return neighbors;

            var updatedNeighbors = new List<CoordWithDirection>();

            switch (neighborChar)
            {
                case '\\':
                    // Rotate clockwise
                    updatedNeighbors.Add(RotateClockwise(neighbors[0]));
                    break;

                case '/':
                    // Rotate counter-clockwise
                    updatedNeighbors.Add(RotateCounterClockwise(neighbors[0]));
                    break;

                case '-':
                    // Split into two directions
                    updatedNeighbors.AddRange(SplitDirection(neighbors[0], (0, 1), (0, -1)));
                    break;

                case '|':
                    // Split into two directions
                    updatedNeighbors.AddRange(SplitDirection(neighbors[0], (1, 0), (-1, 0)));
                    break;
            }

            return updatedNeighbors;
        }

        private CoordWithDirection RotateClockwise(CoordWithDirection coord)
        {
            var rotated = (-coord.Item3.Item2, coord.Item3.Item1);
            return (coord.Item1, coord.Item2, rotated);
        }

        private CoordWithDirection RotateCounterClockwise(CoordWithDirection coord)
        {
            var rotated = (coord.Item3.Item2, -coord.Item3.Item1);
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

            stack.Push(Start);

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