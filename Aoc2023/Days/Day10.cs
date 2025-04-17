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

public class Day10 : Day
{
    private readonly List<string> input;
    private readonly Grid grid;

    public Day10(string filepath)
    {
        input = new InputReader(filepath).ReadLines();
        grid = new Grid(dxdy: [(0, 1), (0, -1), (1, 0), (-1, 0)]);
        grid.Create(input);
    }

    private static (int, int) GetStartFromGrid(Grid grid)
    {
        foreach (var (k, v) in grid.gridMap.ToList())
        {
            if (v == 'S') return k;
        }
        throw new InvalidOperationException("grid does not have a starting point");
    }

    private string Solve(int part)
    {
        if (part == 1)
        {
            var start = GetStartFromGrid(grid);
            var path = new AocGridDFS(grid, start, "Day10").Search([]);
            return (path.Count() / 2).ToString();
        }
        else
        {
            return "Not implemented";
        }

    }

    string Day.Part1() => Solve(1);
    string Day.Part2() => Solve(2);

}