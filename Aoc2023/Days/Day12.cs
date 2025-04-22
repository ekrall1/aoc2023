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

public class Day12 : Day
{
    private List<string> input;

    public Day12(string filepath)
    {
        this.input = new InputReader(filepath).ReadLines();
    }
    private string Solve(int part)
    {
        return this.input
            .SelectMany(line =>
            {
                var parts = line.Split(' ').ToList();
                var counts = parts[1].Split(',').Select(int.Parse).ToList();
                var gears = parts[0];

                var candidateChars = gears.Select(gear => gear == '?'
                    ? new List<char> { '.', '#' }
                    : new List<char> { gear })
                    .ToList();

                IEnumerable<IEnumerable<char>> product = new[] { Enumerable.Empty<char>() };
                product = candidateChars.Aggregate(
                    product,
                    (acc, choices) => acc.SelectMany(l => choices, (l, r) => l.Append(r)));

                return product.Select(p =>
                {
                    var stringVal = string.Join("", p);
                    var matches = Regex.Matches(stringVal, @"#+").Select(m => m.Length).ToList();
                    return matches.SequenceEqual(counts) ? stringVal : null;
                }).Where(str => str != null);
            }).ToList().Count.ToString();
    }

    string Day.Part1() => Solve(1);
    string Day.Part2() => Solve(2);

}