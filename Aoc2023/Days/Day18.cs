using Aoc2023;
using Aoc2023.Days;
using Aoc2023.Input;
using Aoc2023.Utils;

public class Day18Grid : Grid
{
    public HashSet<int> uniqueRows = new();
    public HashSet<int> uniqueCols = new();
    public List<((int row, int col) start, (int row, int col) end)> edges = new();
    public Day18Grid((int, int)[] dxdy) : base(dxdy)
    {
    }
}

public class GridBuilder
{
    private readonly List<string> input;
    private readonly Day18Grid grid;
    public readonly HashSet<int> uniqueRows = new();
    public readonly HashSet<int> uniqueCols = new();
    private (int row, int col) coord = (0, 0);

    public GridBuilder(List<string> input)
    {
        this.input = input;
        this.grid = new Day18Grid([(0, 1), (0, -1), (1, 0), (-1, 0)]);
    }

    public GridBuilder FillGridColorMap()
    {
        foreach (var line in input)
        {
            var parts = line.Split(' ');
            if (parts.Length != 3)
            {
                throw new InvalidOperationException($"Malformed line: '{line}'");
            }
            var direction = parts[0].Trim().ToLower() switch
            {
                "u" => (-1, 0),
                "d" => (1, 0),
                "l" => (0, -1),
                "r" => (0, 1),
                _ => throw new InvalidOperationException($"Unknown direction: {parts[0]}")
            };
            int steps = int.Parse(parts[1]);
            string color = parts[2].Trim('(', ')');
            var start = coord;
            for (int i = 0; i < steps; i++)
            {
                coord = (coord.row + direction.Item1, coord.col + direction.Item2);
                uniqueRows.Add(coord.row);
                uniqueCols.Add(coord.col);
                grid.stringGridMap[coord] = color;
            }
            var end = coord;
            grid.edges.Add((start, end));
        }
        return this;
    }

    public GridBuilder FinalizeDimensions()
    {
        grid.rows = uniqueRows.Max();
        int maxCol = uniqueCols.Max();
        grid.cols = Enumerable.Repeat(maxCol, grid.rows + 1).ToList();
        grid.uniqueRows = uniqueRows;
        grid.uniqueCols = uniqueCols;
        return this;
    }

    public Day18Grid Build() => grid;

}

public partial class Day18 : Day
{
    private List<string> input;
    private Day18Grid grid;


    public Day18(string filepath)
    {
        this.input = new InputReader(filepath).ReadLines();
        this.grid = new GridBuilder(input)
                        .FillGridColorMap()
                        .FinalizeDimensions()
                        .Build();
    }

    private string Solve(int part)
    {
        int dugOut = 0;

        for (int row = grid.uniqueRows.Min(); row <= grid.uniqueRows.Max(); row++)
        {
            for (int col = grid.uniqueCols.Min(); col <= grid.uniqueCols.Max(); col++)
            {
                if (grid.stringGridMap.GetValueOrDefault((row, col), "") == "")
                {
                    int crossings = 0;

                    foreach (var (start, end) in grid.edges)
                    {
                        int minRow = Math.Min(start.row, end.row);
                        int maxRow = Math.Max(start.row, end.row);

                        if (row >= minRow && row < maxRow && start.col > col)
                        {
                            crossings++;
                        }

                    }

                    dugOut += crossings % 2;
                }
                else
                    dugOut++;
            }
        }
        return dugOut.ToString();
    }

    string Day.Part1() => Solve(1);
    string Day.Part2() => Solve(2);

}