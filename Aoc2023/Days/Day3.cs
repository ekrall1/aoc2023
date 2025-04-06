using Aoc2023;
using Aoc2023.Days;
using Aoc2023.Input;
using Aoc2023.Utils;
using System.Text;

public class Day3 : Day
{

    private string _filepath;
    private List<string> _inputList;
    private Dictionary<(int, int), int> _numberMap;

    public Day3(string filepath)
    {
        this._filepath = filepath;
        InputReader fileInput = new InputReader(this._filepath);
        this._inputList = fileInput.ReadLines();
        this._numberMap = new Dictionary<(int, int), int> { };
    }
    private int FindSum()
    {
        var grid = new Grid();
        var finalSum = 0;
        grid.Create(this._inputList);

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
                var neighbors = grid.NeighborsOf(coords[idx]);
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
        return this.FindSum().ToString();
    }

    string Day.Part2()
    {
        return string.Join(",", this._inputList);
    }

}