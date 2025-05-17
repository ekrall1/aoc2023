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

        long numVisitedPlots = new Day21BFS(grid, startPos, IsTest ? 6L : 64L).Search();
        return numVisitedPlots.ToString();

    }

    string Day.Part1() => Solve(1);
    string Day.Part2() => Solve(2);
}