using Aoc2023;
using Aoc2023.Days;
using Aoc2023.Input;

public class Day10 : Day
{
    private readonly List<string> input;
    private readonly Grid grid;

    public Day10(string filepath)
    {
        input = new InputReader(filepath).ReadLines();
        grid = new Grid(dxdy: [(0, 1), (0, -1), (1, 0), (-1, 0)]);
        grid.Create(input);
    }

    private static (int, int) GetStartFromGrid(Grid grid)
    {
        foreach (var (k, v) in grid.gridMap.ToList())
        {
            if (v == 'S') return k;
        }
        throw new InvalidOperationException("grid does not have a starting point");
    }

    private static List<(int, int)> GetNonPathPoints(Grid grid, HashSet<(int, int)> path)
    {
        return grid.gridMap.Select(grid => grid.Key)
             .Where(k => !path.Contains(k))
             .ToList();
    }

    private string Solve(int part)
    {
        if (part == 1)
        {
            (int, int) start = GetStartFromGrid(grid);
            HashSet<(int, int)> path = new AocGridDFS(grid, start, "Day10").Search([]);
            return (path.Count() / 2).ToString();
        }
        else
        {
            (int, int) start = GetStartFromGrid(grid);
            HashSet<(int, int)> path = new AocGridDFS(grid, start, "Day10").Search([]);
            Polygon polygon = new Polygon(path.ToList());
            List<(int, int)> points = GetNonPathPoints(grid, path);

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