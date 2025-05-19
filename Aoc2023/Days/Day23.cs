using System.Drawing;
using System.Security;
using Aoc2023;
using Aoc2023.Days;
using Aoc2023.Input;
using Aoc2023.Utils;


public partial class Day23 : Day
{
    public List<string> Input { get; private set; }
    public Grid Grid { get; private set; }
    public HashSet<(int, int)> Intersections { get; private set; }

    public Dictionary<(int, int), List<((int, int) dest, int dist)>> ReducedGraph { get; private set; }
    public (int, int) Start { get; set; }
    public (int, int) End { get; set; }
    public int MaxEdgeLength { get; set; }

    public Day23(string filepath)
    {
        Input = new InputReader(filepath).ReadLines();
        Grid = new Grid([(0, 1), (0, -1), (1, 0), (-1, 0)]);
        Grid.Create(Input);
        (Start, End) = ScanGridForStartAndEnd();
        Intersections = new();
        ReducedGraph = new();
        MaxEdgeLength = 0;
    }

    private ((int, int) start, (int, int) end) ScanGridForStartAndEnd()
    {
        int firstRow = 0;
        int lastRow = Grid.Rows - 1;
        (int, int)? start = null;
        (int, int)? end = null;

        foreach (var kvp in Grid.GridMap)
        {
            var (coord, val) = (kvp.Key, kvp.Value);
            if (val == '.')
            {
                if (coord.Item1 == firstRow)
                    start = coord;
                else if (coord.Item1 == lastRow)
                    end = coord;
            }
        }

        if (start == null || end == null)
            throw new InvalidOperationException("Start or end coordinate not found.");

        return (start.Value, end.Value);
    }

    private HashSet<(int, int)> GetIntersections(int part)
    {
        var intersections = new HashSet<(int, int)>();
        foreach (var kvp in Grid.GridMap)
        {
            var (pos, val) = (kvp.Key, kvp.Value);
            if (val == '#') continue;

            // Always add start and end
            if (pos == Start || pos == End)
            {
                intersections.Add(pos);
                continue;
            }

            // Forced direction tiles are intersections
            if (part == 1 && (val == '>' || val == '<' || val == '^' || val == 'v'))
            {
                intersections.Add(pos);
                continue;
            }

            // Find free neighbors ('.' or forced direction)
            var neighbors = new List<(int, int)>();
            foreach (var (dx, dy) in Grid.Dxdy)
            {
                var neighbor = (pos.Item1 + dx, pos.Item2 + dy);
                if (Grid.GridMap.TryGetValue(neighbor, out var nval) && nval != '#')
                {
                    neighbors.Add(neighbor);
                }
            }

            // If not exactly 2 neighbors => intersection
            if (neighbors.Count != 2)
            {
                intersections.Add(pos);
                continue;
            }

            // If exactly 2 neighbors, check if they are aligned straight
            var (n1, n2) = (neighbors[0], neighbors[1]);

            // Check if neighbors share same row or same column
            bool alignedStraight = (n1.Item1 == pos.Item1 && n2.Item1 == pos.Item1) ||
                                   (n1.Item2 == pos.Item2 && n2.Item2 == pos.Item2);

            if (!alignedStraight)
            {
                intersections.Add(pos); // corner (turn)
            }
        }
        return intersections;
    }


    private bool InBounds((int, int) p)
    {
        return p.Item1 >= 0 && p.Item1 < Grid.Rows &&
               p.Item2 >= 0 && p.Item2 < Grid.Cols.Count;
    }

    private Dictionary<(int, int), List<((int, int) dest, int dist)>> GetReducedGraph(int part)
    {
        var graph = new Dictionary<(int, int), List<((int, int) dest, int dist)>>();

        foreach (var start in Intersections)
        {
            foreach (var (dx, dy) in Grid.Dxdy)
            {
                var current = (start.Item1 + dx, start.Item2 + dy);
                if (!InBounds(current)) continue;
                if (Grid.GridMap[current] == '#') continue;

                int dist = 1;
                var dir = (dx, dy);

                // Follow path until you hit another intersection or block
                while (true)
                {
                    char tile = Grid.GridMap[current];

                    // Enforce forced direction if present
                    if (tile == '>' && dir != (0, 1) && part == 1) break;
                    if (tile == '<' && dir != (0, -1) && part == 1) break;
                    if (tile == '^' && dir != (-1, 0) && part == 1) break;
                    if (tile == 'v' && dir != (1, 0) && part == 1) break;

                    if (Intersections.Contains(current))
                    {
                        // Add edge from start to current intersection
                        if (!graph.ContainsKey(start))
                            graph[start] = new();

                        graph[start].Add((current, dist));
                        break;
                    }

                    // Move to next cell in current direction
                    var next = (current.Item1 + dir.Item1, current.Item2 + dir.Item2);
                    if (!InBounds(next)) break;
                    if (Grid.GridMap[next] == '#') break;

                    current = next;
                    dist++;
                }
            }
        }

        return graph;
    }
    int MaxRemainingEstimate((int x, int y) current)
    {
        return 1000 * Math.Abs(current.x - End.Item1) + Math.Abs(current.y - End.Item2);
    }

    private long FindAllPathsDFS()
    {
        long best = long.MinValue;

        void Dfs((int x, int y) current, HashSet<(int, int)> visited, long currentLength)
        {
            if (current == End)
            {
                if (currentLength > best)
                    best = currentLength;
                return;
            }

            if (currentLength + MaxRemainingEstimate(current) <= best)
                return;

            if (!ReducedGraph.TryGetValue(current, out var neighbors))
                return;

            foreach (var (to, edgeDist) in neighbors)
            {
                if (visited.Contains(to))
                    continue;

                visited.Add(to);
                Dfs(to, visited, currentLength + edgeDist);
                visited.Remove(to);
            }
        }

        var visited = new HashSet<(int, int)> { Start };
        Dfs(Start, visited, 0);

        return best;
    }
    private string Solve(int part)
    {
        Intersections = GetIntersections(part);
        ReducedGraph = GetReducedGraph(part);
        int max = 0;
        foreach (var list in ReducedGraph.Values)
            foreach (var (_, dist) in list)
                max = Math.Max(max, dist);
        MaxEdgeLength = max;

        return FindAllPathsDFS().ToString();
        throw new NotImplementedException($"Part {part} is an invalid part. Only parts 1 and 2 are valid.");
    }

    string Day.Part1() => Solve(1);
    string Day.Part2() => Solve(2);
}