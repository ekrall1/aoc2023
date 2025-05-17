using System.Text;
using Aoc2023;
using Aoc2023.Days;
using Aoc2023.Input;

public class Day3 : Day
{
    public string FilePath { get; private set; }
    public List<string> InputList { get; private set; }
    public Dictionary<(int, int), (int, (int, int))> NumberMap { get; private set; }
    public List<(int, int)> Gears { get; private set; }

    public Day3(string filepath)
    {
        this.FilePath = filepath;
        InputReader fileInput = new InputReader(this.FilePath);
        this.InputList = fileInput.ReadLines();
        this.NumberMap = new Dictionary<(int, int), (int, (int, int))>();
        this.Gears = new List<(int, int)>();
    }

    private Grid MakeGrid()
    {
        var grid = new Grid(dxdy: [(0, 1), (1, 0), (-1, 0), (0, -1), (1, 1), (-1, -1), (1, -1), (-1, 1)]);
        grid.Create(this.InputList);
        return grid;
    }

    private void FillGears(Grid grid)
    {
        for (int i = 0; i < grid.Rows; i++)
        {
            for (int j = 0; j < grid.Cols[i]; j++)
            {
                if (grid.GridMap[(i, j)] == '*')
                {
                    this.Gears.Add((i, j));
                }
            }
        }
    }

    private void FillNumberMap(Grid grid)
    {
        for (int i = 0; i < grid.Rows; i++)
        {
            for (int j = 0; j < grid.Cols[i]; j++)
            {
                if (char.IsDigit(grid.GridMap[(i, j)]))
                {
                    var startCoord = (i, j);
                    var (coords, strNum) = ExtractNumber(i, ref j, grid);
                    if (int.TryParse(strNum, out var finalNum))
                    {
                        coords.ForEach(coord => this.NumberMap[coord] = (finalNum, startCoord));
                    }
                }
            }
        }
    }

    private int GearProduct(Grid grid)
    {
        var sumProduct = 0;
        for (int i = 0; i < this.Gears.Count; i++)
        {
            var neighbors = grid.NeighborsOfCoord(this.Gears[i]);
            if (neighbors == null)
            {
                continue;
            }
            var prod = 0;
            var visited = new HashSet<(int, (int, int))>();
            neighbors.ForEach(neighbor =>
            {
                if (this.NumberMap.ContainsKey(neighbor) && !visited.Contains(this.NumberMap[neighbor]))
                {
                    prod = Math.Max(prod, 1) * this.NumberMap[neighbor].Item1;
                    visited.Add(this.NumberMap[neighbor]);
                }
            });
            if (visited.Count > 1)
            {
                sumProduct += prod;
            }
        }
        return sumProduct;
    }

    private int FindSum(Grid grid)
    {
        var finalSum = 0;

        for (int i = 0; i < grid.Rows; i++)
        {
            for (int j = 0; j < grid.Cols[i]; j++)
            {
                if (char.IsDigit(grid.GridMap[(i, j)]))
                {
                    var (coords, strNum) = ExtractNumber(i, ref j, grid);
                    finalSum += SumPart(strNum, coords, grid);
                }
            }
        }
        return finalSum;
    }

    private int SumPart(string? strNum, List<(int, int)>? coords, Grid grid)
    {
        if (coords == null)
        {
            return 0;
        }
        if (int.TryParse(strNum, out var finalNum))
        {
            for (int idx = 0; idx < coords.Count; idx++)
            {
                var neighbors = grid.NeighborsOfChar(coords[idx]);
                if (neighbors.Count(c => c == '.' || char.IsDigit(c)) != neighbors.Count)
                {
                    return finalNum;
                }
            }
        }
        return 0;
    }

    private (List<(int, int)>, string) ExtractNumber(int row, ref int startCol, Grid grid)
    {
        var coords = new List<(int, int)>();
        var strNum = new StringBuilder();

        // Traverse row to find the number
        int col = startCol;
        while (col < grid.Cols[row] && char.IsDigit(grid.GridMap[(row, col)]))
        {
            coords.Add((row, col));
            strNum.Append(grid.GridMap[(row, col)]);
            col++;
        }

        // Update j to point to the column after the number
        startCol = col - 1;

        return (coords, strNum.ToString());
    }

    string Day.Part1()
    {
        var grid = MakeGrid();
        return this.FindSum(grid).ToString();
    }

    string Day.Part2()
    {
        var grid = MakeGrid();
        this.FillGears(grid);
        this.FillNumberMap(grid);
        return this.GearProduct(grid).ToString();
    }
}