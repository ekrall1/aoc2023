using Aoc2023;
using Aoc2023.Days;
using Aoc2023.Input;
using CoordWithDirection = System.ValueTuple<int, int, (int, int)>;

public partial class Day16 : Day
{
    public List<string> Input { get; private set; }

    public Day16(string filepath)
    {
        this.Input = new InputReader(filepath).ReadLines();
    }

    private int SolveWithDFS(Grid grid, CoordWithDirection coord)
    {
        IAocGridDFSWithDirection dfs = GridDfsWithDirectionFactory.Create(grid, coord, "Day16");
        var allVisited = dfs.Search([]);
        return allVisited.Select(visited => (visited.Item1, visited.Item2)).ToHashSet().Count;
    }

    private string Solve(int part)
    {
        var grid = new Grid([]);
        grid.Create(this.Input);
        if (part == 1)
        {
            var startPos = (0, 0, (0, 1));
            return SolveWithDFS(grid, startPos).ToString();
        }

        // part 2
        var topRow = Enumerable.Range(0, grid.Cols.Count - 1)
            .Select(c => SolveWithDFS(grid, (0, c, (1, 0))));
        var bottomRow = Enumerable.Range(0, grid.Cols.Count - 1)
            .Select(c => SolveWithDFS(grid, (grid.Rows - 1, c, (-1, 0))));
        var leftCol = Enumerable.Range(0, grid.Rows - 1)
            .Select(c => SolveWithDFS(grid, (c, 0, (0, 1))));
        var rightCol = Enumerable.Range(0, grid.Rows - 1)
            .Select(c => SolveWithDFS(grid, (c, grid.Cols.Count - 1, (0, -1))));

        return new[] { topRow, bottomRow, leftCol, rightCol }.SelectMany(e => e).Max().ToString();

    }

    string Day.Part1() => Solve(1);
    string Day.Part2() => Solve(2);

}