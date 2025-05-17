using Aoc2023;
using Aoc2023.Days;
using Aoc2023.Input;

public class Day10 : Day
{
    public List<string> Input { get; private set; }
    public Grid Grid { get; private set; }

    public Day10(string filepath)
    {
        Input = new InputReader(filepath).ReadLines();
        Grid = new Grid(dxdy: [(0, 1), (0, -1), (1, 0), (-1, 0)]);
        Grid.Create(Input);
    }

    private static (int, int) GetStartFromGrid(Grid grid)
    {
        foreach (var (k, v) in grid.GridMap.ToList())
        {
            if (v == 'S') return k;
        }
        throw new InvalidOperationException("grid does not have a starting point");
    }

    private static List<(int, int)> GetNonPathPoints(Grid grid, HashSet<(int, int)> path)
    {
        return grid.GridMap.Select(grid => grid.Key)
             .Where(k => !path.Contains(k))
             .ToList();
    }

    private string Solve(int part)
    {
        if (part == 1)
        {
            (int, int) start = GetStartFromGrid(Grid);
            HashSet<(int, int)> path = new AocGridDFS(Grid, start, "Day10").Search([]);
            return (path.Count() / 2).ToString();
        }
        else
        {
            (int, int) start = GetStartFromGrid(Grid);
            HashSet<(int, int)> path = new AocGridDFS(Grid, start, "Day10").Search([]);
            Polygon polygon = new Polygon(path.ToList());
            List<(int, int)> points = GetNonPathPoints(Grid, path);

            int insideCount = 0;
            foreach (var point in points)
            {
                if (polygon.ContainsPoint(point))
                {
                    insideCount += 1;
                }
            }
            return insideCount.ToString();
        }
    }

    string Day.Part1() => Solve(1);
    string Day.Part2() => Solve(2);
}