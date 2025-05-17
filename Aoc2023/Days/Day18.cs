using Aoc2023;
using Aoc2023.Days;
using Aoc2023.Input;

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

    public GridBuilder FillGridColorMap(int part)
    {
        foreach (var line in input)
        {
            var parts = line.Split(' ');
            if (parts.Length != 3)
            {
                throw new InvalidOperationException($"Malformed line: '{line}'");
            }

            (int, int) direction;
            int steps;
            string color;

            if (part == 1)
            {
                direction = parts[0].Trim().ToLower() switch
                {
                    "u" => (-1, 0),
                    "d" => (1, 0),
                    "l" => (0, -1),
                    "r" => (0, 1),
                    _ => throw new InvalidOperationException($"Unknown direction: {parts[0]}")
                };
                steps = int.Parse(parts[1]);
                color = parts[2].Trim('(', ')');
            }
            else
            {
                string hex = parts[2].Trim('(', ')', '#');
                steps = Convert.ToInt32(hex[..5], 16);
                direction = hex[5] switch
                {
                    '0' => (0, 1),
                    '1' => (1, 0),
                    '2' => (0, -1),
                    '3' => (-1, 0),
                    _ => throw new InvalidOperationException($"Unknown direction from hex: {hex}")
                };
                color = hex;
            }

            var start = coord;
            for (int i = 0; i < steps; i++)
            {
                coord = (coord.row + direction.Item1, coord.col + direction.Item2);
                uniqueRows.Add(coord.row);
                uniqueCols.Add(coord.col);
                grid.StringGridMap[coord] = color;
            }
            var end = coord;
            grid.edges.Add((start, end));
        }
        return this;
    }

    public GridBuilder FinalizeDimensions()
    {
        grid.Rows = uniqueRows.Max();
        int maxCol = uniqueCols.Max();
        grid.Cols = Enumerable.Repeat(maxCol, grid.Rows + 1).ToList();
        grid.uniqueRows = uniqueRows;
        grid.uniqueCols = uniqueCols;
        return this;
    }

    public Day18Grid Build() => grid;

}

public partial class Day18 : Day
{
    public List<string> Input { get; private set; }

    public Day18(string filepath)
    {
        this.Input = new InputReader(filepath).ReadLines();
    }

    private string Solve(int part)
    {
        Day18Grid grid = new GridBuilder(Input)
            .FillGridColorMap(part)
            .FinalizeDimensions()
            .Build();

        long dugOut = 0;
        long area = 0;
        long perimeter = 0;

        foreach (var ((r1, c1), (r2, c2)) in grid.edges)
        {
            // Shoelace formula part: area += x1*y2 - x2*y1
            area += (long)c1 * (long)r2 - (long)c2 * (long)r1;

            // Perimeter contribution (Manhattan distance)
            perimeter += Math.Abs(r2 - r1) + Math.Abs(c2 - c1);
        }

        area = Math.Abs(area) / 2;

        dugOut = area + (perimeter / 2) + 1;

        return dugOut.ToString();
    }

    string Day.Part1() => Solve(1);
    string Day.Part2() => Solve(2);

}
