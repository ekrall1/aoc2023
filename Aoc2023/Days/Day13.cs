using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Aoc2023;
using Aoc2023.Days;
using Aoc2023.Input;
using Aoc2023.Utils;

public class Day13 : Day
{
    private List<string> input;
    private List<List<string>> puzzlePatterns;

    public Day13(string filepath)
    {
        this.input = new InputReader(filepath).ReadLines();

        List<string> tmp = [];
        this.puzzlePatterns = [];

        this.input.Add("");
        foreach (var line in this.input)
        {
            if (line == "")
            {
                if (tmp.Count > 0)
                {
                    this.puzzlePatterns.Add(tmp);
                }
                tmp = [];
            }
            else
            {
                tmp.Add(line);
            }
        }
    }
    private record RowSymmetry(bool hasSymmetry, int i, int j);
    private record ColSymmetry(bool hasSymmetry, int i, int j);

    private static RowSymmetry CheckRows(List<string> puzzle)
    {
        var rowSymmetry = false;
        int j = 0;
        for (int i = 0; i < puzzle.Count - 1; i++)
        {
            j = i + 1;
            var top = puzzle[i];
            var bottom = puzzle[j];

            if (top == bottom)
            {
                int m = i;
                int n = j;
                while (m >= 0 && n < puzzle.Count)
                {
                    rowSymmetry = puzzle[m] == puzzle[n];
                    if (!rowSymmetry)
                        break;
                    m--;
                    n++;
                }
                if (rowSymmetry)
                    return new RowSymmetry(rowSymmetry, i, j);
            }
        }
        return new RowSymmetry(rowSymmetry, -1, -1);
    }

    private static ColSymmetry CheckCols(List<string> puzzle)
    {
        var colSymmetry = false;
        int j = 0;
        for (int i = 0; i < puzzle[0].Length - 1; i++)
        {
            j = i + 1;
            var left = puzzle.Select(r => r[i]).ToList();
            var right = puzzle.Select(r => r[j]).ToList();

            if (left.SequenceEqual(right))
            {
                int m = i;
                int n = j;
                while (m >= 0 && n < puzzle[0].Length)
                {
                    var _left = puzzle.Select(r => r[m]).ToList();
                    var _right = puzzle.Select(r => r[n]).ToList();
                    colSymmetry = _left.SequenceEqual(_right);
                    if (!colSymmetry)
                        break;
                    m--;
                    n++;
                }
                if (colSymmetry)
                    return new ColSymmetry(colSymmetry, i, j);
            }

        }
        return new ColSymmetry(colSymmetry, -1, -1);

    }

    private string Solve(int part)
    {
        int score = 0;
        foreach (var puzzle in this.puzzlePatterns)
        {
            var rowSymmetry = CheckRows(puzzle);
            score += 100 * (rowSymmetry.hasSymmetry ? rowSymmetry.i + 1 : 0);

            if (rowSymmetry.hasSymmetry)
            {
                continue;
            }

            var colSymmetry = CheckCols(puzzle);
            score += 1 * (colSymmetry.hasSymmetry ? colSymmetry.i + 1 : 0);
        }
        return score.ToString();
    }

    string Day.Part1() => Solve(1);
    string Day.Part2() => Solve(2);

}