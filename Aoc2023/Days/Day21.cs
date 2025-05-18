using System.Numerics;
using Aoc2023;
using Aoc2023.Days;
using Aoc2023.Input;
using Aoc2023.Utils;


public partial class Day21 : Day
{
    public List<string> Input { get; private set; }
    public bool IsTest { get; set; }
    public Day21(string filepath)
    {
        Input = new InputReader(filepath).ReadLines();
        IsTest = filepath.Contains("Test");
    }
    private string Solve(int part)

    {
        var grid = new Grid(dxdy: [(0, 1), (1, 0), (0, -1), (-1, 0)]);
        grid.Create(Input);
        var startPos = (0, 0);
        for (int i = 0; i < grid.Rows; i++)
        {
            for (int j = 0; j < grid.Cols[i]; j++)
            {
                if (grid.GridMap[(i, j)] == 'S')
                {
                    startPos = (i, j);
                    break;
                }
            }
        }

        BigInteger numVisitedPlots = 0;
        if (part == 1)
        {
            var maxSteps = IsTest ? 6L : 64L;
            var counts = new Day21BFS(grid, startPos, maxSteps, part).Search();
            numVisitedPlots = counts[maxSteps];
        }
        if (part == 2)
        {
            long gridSize = grid.Rows;
            long midPoint = gridSize / 2;
            long targetSteps = 26501365;

            List<long> points = new();
            for (int i = 0; i < 3; i++)
            {
                long steps = midPoint + (i * gridSize);
                var counts = new Day21BFS(grid, startPos, steps, part).Search();
                long sum = counts
                    .Where(kv => kv.Key % 2 == steps % 2)
                    .Sum(kv => kv.Value);
                points.Add(sum);
            }

            long y0 = points[0];
            long y1 = points[1];
            long y2 = points[2];

            long a = (y2 - 2 * y1 + y0) / 2;
            long b = y1 - y0 - a;
            long c = y0;

            long N = (targetSteps - midPoint) / gridSize;
            long result = a * N * N + b * N + c;

            numVisitedPlots = (long)result;
        }
        return numVisitedPlots.ToString();
    }

    string Day.Part1() => Solve(1);
    string Day.Part2() => Solve(2);
}