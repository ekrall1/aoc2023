using System.Data;
using Aoc2023;
using Aoc2023.Days;
using Aoc2023.Input;
using Aoc2023.Utils;
using Microsoft.VisualBasic;

public class Day14 : Day
{
    private List<string> input;

    public Day14(string filepath)
    {
        this.input = new InputReader(filepath).ReadLines();
    }

    private List<string> TiltPart1()
    {
        var sortedCols = Enumerable.Range(0, this.input[0].Length)
            .Select(col => new string(this.input.Select(row => row[col]).ToArray()))
            .Select(colStr => colStr.Split('#')
                .Select(part => new string(part.OrderByDescending(ch => ch).ToArray()))
             )
            .Select(sorted => string.Join('#', sorted)).ToList();

        return Enumerable.Range(0, sortedCols[0].Length)
            .Select(col => new string(sortedCols.Select(row => row[col])
            .ToArray())).ToList();
    }

    private string Solve(int part)
    {
        var sortedCols = TiltPart1();

        int totalSum = 0;
        for (int r = 0; r < sortedCols.Count; r++)
        {
            int rowSum = 0;
            for (int c = 0; c < sortedCols[r].Length; c++)
            {
                rowSum += sortedCols[r][c] == 'O' ? sortedCols.Count - r : 0;
            }
            totalSum += rowSum;
        }
        return part == 1 ? totalSum.ToString() : "";
    }

    string Day.Part1() => Solve(1);
    string Day.Part2() => Solve(2);

}