using System.Text.RegularExpressions;
using Aoc2023.Days;
using Aoc2023.Input;

public class Day12 : Day
{
    public List<string> Input { get; private set; }
    public static Dictionary<string, long> Memo { get; private set; } = new();

    public Day12(string filepath)
    {
        this.Input = new InputReader(filepath).ReadLines();
    }

    private string Solve(int part)
    {
        string springs = "";
        List<int> counts = new List<int>();
        if (part == 2)
        {
            long ctr = 0;
            foreach (var line in Input)
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
        return this.Input
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
        string key = string.Join(",", springs) + "|" + string.Join(",", counts) + "|" + string.Concat(acc);

        if (Memo.TryGetValue(key, out long cachedVal))
        {
            return cachedVal;
        }

        long res = 0;
        if (springs.Count == 0)
        {
            if (acc.Count > 0)
            {
                if (counts.Count == 1 && acc.Count == counts[0])
                    res = 1;
            }
            else if (counts.Count == 0)
            {
                res = 1;
            }
        }
        else
        {
            var hd = springs[0];
            var tl = springs.Skip(1).ToList();
            var checks = hd == '?' ? new[] { '.', '#' } : new[] { hd };

            foreach (char c in checks)
            {
                if (c == '#')
                {
                    var newAcc = new List<char>(acc) { c };
                    res += Part2Solver(tl, counts, newAcc);
                }
                else
                {
                    if (acc.Count > 0)
                    {
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
        }
        Memo[key] = res;
        return res;
    }

    string Day.Part1() => Solve(1);
    string Day.Part2() => Solve(2);
}
