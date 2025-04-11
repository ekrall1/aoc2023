using System.Text;
using Aoc2023;
using Aoc2023.Days;
using Aoc2023.Input;

public class Day3 : Day
{

    private string _filepath;
    private List<string> _inputList;
    private Dictionary<(int, int), (int, (int, int))> _numberMap;
    private List<(int, int)> _gears;

    public Day3(string filepath)
    {
        this._filepath = filepath;
        InputReader fileInput = new InputReader(this._filepath);
        this._inputList = fileInput.ReadLines();
        this._numberMap = new Dictionary<(int, int), (int, (int, int))> { };
        this._gears = [];
    }

    private Grid MakeGrid()
    {
        var grid = new Grid();
        grid.Create(this._inputList);
        return grid;
    }

    private void FillGears(Grid grid)
    {
        {
            for (int i = 0; i < grid.rows; i++)
            {
                for (int j = 0; j < grid.cols[i]; j++)
                {
                    if (grid.gridMap[(i, j)] == '*')
                    {
                        this._gears.Add((i, j));
                    }
                }
            }
        }
    }

    private void FillNumberMap(Grid grid)
    {
        {
            for (int i = 0; i < grid.rows; i++)
            {
                for (int j = 0; j < grid.cols[i]; j++)
                {
                    if (char.IsDigit(grid.gridMap[(i, j)]))
                    {
                        var startCoord = (i, j);
                        var (coords, strNum) = ExtractNumber(i, ref j, grid);
                        if (int.TryParse(strNum, out var finalNum))
                        {
                            coords.ForEach(coord => this._numberMap[coord] = (finalNum, startCoord));
                        }
                    }
                }
            }
        }
    }

    private int GearProduct(Grid grid)
    {
        var sumProduct = 0;
        for (int i = 0; i < this._gears.Count; i++)
        {
            var neighbors = grid.NeighborsOfCoord(this._gears[i]);
            if (neighbors == null)
            {
                continue;
            }
            var prod = 0;
            var visited = new HashSet<(int, (int, int))> { };
            neighbors.ForEach(neighbor =>
            {
                if (this._numberMap.ContainsKey(neighbor) && !visited.Contains(this._numberMap[neighbor]))
                {
                    prod = Math.Max(prod, 1) * this._numberMap[neighbor].Item1;
                    visited.Add(this._numberMap[neighbor]);
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

        for (int i = 0; i < grid.rows; i++)
        {
            for (int j = 0; j < grid.cols[i]; j++)
            {
                if (char.IsDigit(grid.gridMap[(i, j)]))
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
        while (col < grid.cols[row] && char.IsDigit(grid.gridMap[(row, col)]))
        {
            coords.Add((row, col));
            strNum.Append(grid.gridMap[(row, col)]);
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