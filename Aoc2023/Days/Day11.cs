using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Aoc2023;
using Aoc2023.Days;
using Aoc2023.Input;
using Aoc2023.Utils;

public class Day11 : Day
{
    readonly private List<string> rawInput;
    private List<string> input;
    private readonly Grid grid;
    private List<((int, int), (int, int))> pairs;

    public Day11(string filepath)
    {
        rawInput = new InputReader(filepath).ReadLines();
        (this.input, this.pairs) = this.ExpandInput(rawInput);
        this.grid = new Grid(dxdy: [(0, 1), (0, -1), (1, 0), (-1, 0)]);
        this.grid.Create(input);
    }

    private (List<string>, List<((int, int), (int, int))>) ExpandInput(List<string> originalInput)
    {
        List<string> newInput = [];

        // rows
        for (int r = 0; r < originalInput.Count; r++)
        {
            newInput.Add(originalInput[r]);
            if (originalInput[r].ToHashSet<char>().Count == 1)
            {
                newInput.Add(originalInput[r]);
            }
        }

        // cols
        int addedCols = 0;
        for (int c = 0; c < originalInput.Count(); c++)
        {
            string column = "";
            for (int r = 0; r < originalInput.Count; r++)
            {
                column += originalInput[r][c];
            }
            if (column.ToHashSet<char>().Count == 1)
            {
                for (int r = 0; r < newInput.Count; r++)
                {
                    var tmpRemaining = newInput[r][(c + addedCols + 1)..];
                    newInput[r] = newInput[r][0..(c + addedCols + 1)];
                    newInput[r] += '.';
                    newInput[r] += tmpRemaining;
                }
                addedCols++;
            }
        }

        // convert galaxies to numbers and collect pairs
        int ctr = 1;
        List<char[]> mutNewInput = newInput.Select(row => row.ToCharArray()).ToList();
        List<(int, int)> points = new List<(int, int)>();

        for (int r = 0; r < mutNewInput.Count; r++)
        {
            for (int c = 0; c < mutNewInput[r].Count(); c++)
            {
                if (mutNewInput[r][c] == '#')
                {
                    mutNewInput[r][c] = (char)(ctr + '0');
                    ctr++;
                    points.Add((r, c));
                }
            }
        }

        return (mutNewInput.Select(row => new String(row)).ToList(), GetAllPairs(points));
    }

    static List<((int, int), (int, int))> GetAllPairs(List<(int, int)> list)
    {
        var result = new List<((int, int), (int, int))>();

        for (int i = 0; i < list.Count; i++)
        {
            for (int j = i + 1; j < list.Count; j++)
            {
                result.Add((list[i], list[j]));
            }
        }

        return result;
    }

    private string Solve(int part)
    {
        int totalSum = 0;
        foreach (((int, int), (int, int)) pair in this.pairs)
        {
            totalSum += Distance.Manhattan(pair.Item1, pair.Item2);
        }
        return totalSum.ToString();

    }

    string Day.Part1() => Solve(1);
    string Day.Part2() => Solve(2);

}