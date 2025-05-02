namespace Aoc2023
{
    public class AocGridDFS
    {
        public (int, int) Start;
        private Grid _Grid;
        private string PuzzleDay;

        private Func<(int, int), List<(int, int)>, List<(int, int)>> _FilterFunc;

        public AocGridDFS(Grid grid, (int, int) start, string puzzleDay)
        {
            Start = start;
            _Grid = grid;
            PuzzleDay = puzzleDay;

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

    }
}