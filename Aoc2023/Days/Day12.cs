using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
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

    private static Dictionary<string, long> memo = new();
    private string Solve(int part)
    {
        string springs = "";
        List<int> counts = new List<int>();
        if (part == 2)
        {
            long ctr = 0;
            foreach (var line in input)
            {
                var parts = line.Split(' ');
                springs = string.Join("?", Enumerable.Repeat(parts[0], 5));
                var springsArray = springs.ToCharArray().ToList();
                counts = string.Join(",", Enumerable.Repeat(parts[1], 5))
                    .Split(',')
                    .Select(int.Parse)
                    .ToList();
                ctr += Part2Solver(springsArray, counts, []);
            }
            return ctr.ToString();
        }
        return this.input
            .SelectMany(line =>
            {
                var parts = line.Split(' ').ToList();
                counts = parts[1].Split(',').Select(int.Parse).ToList();
                springs = parts[0];

                var candidateChars = springs.Select(spring => spring == '?'
                    ? new List<char> { '.', '#' }
                    : new List<char> { spring })
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

    private static long Part2Solver(List<char> springs, List<int> counts, List<char> acc)
    {

        string key = string.Join(",", springs) + string.Join(",", counts) + string.Join("", acc);

        if (memo.TryGetValue(key, out long cachedVal))
        {
            return cachedVal;
        }

        if (springs.Count == 0)
        {
            // exhausted all counts or only one count left and it is equal to the accumulated #s
            if (acc.Count > 0)
            {
                return (counts.Count == 1 && acc.Count == counts[0]) ? 1 : 0;
            }
            return (counts.Count == 0) ? 1 : 0;
        }

        var hd = springs[0];
        var tl = springs.Skip(1).ToList();

        var checks = hd == '?' ? new[] { '.', '#' } : new[] { hd };
        long res = 0;
        foreach (char c in checks)
        {
            if (c == '#')
            {
                var newAcc = new List<char>(acc) { c };
                res += Part2Solver(tl, counts, newAcc);
            }
            else  // if it's not a #, start a new accumulator
            {
                if (acc.Count > 0)
                {
                    // If a group just ended and it's a 'fit'
                    if (counts.Count > 0 && acc.Count == counts[0])
                    {
                        res += Part2Solver(tl, counts.Skip(1).ToList(), new List<char>());
                    }
                }
                else
                {
                    res += Part2Solver(tl, counts, new List<char>());
                }
            }

        }
        memo[key] = res;
        return res;
    }

    string Day.Part1() => Solve(1);
    string Day.Part2() => Solve(2);

}