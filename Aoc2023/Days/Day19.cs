using System.Text.RegularExpressions;
using Aoc2023.Days;
using Aoc2023.Input;


public partial class Day19 : Day
{
    public List<List<string>> Input { get; private set; }
    public Dictionary<string, List<Rule>> Workflows { get; private set; }
    public List<Part> Parts { get; private set; }

    public record Part(int x, int m, int s, int a);

    public class Rule
    {
        public string? Field { get; set; }
        public int Value { get; set; }
        public char Operator { get; set; }
        public required string Next { get; set; }
        public bool IsFallback { get; set; }
    }

    public Day19(string filepath)
    {
        this.Input = new InputReader(filepath).ReadTwoPartLines();
        this.Workflows = SetupWorkflows();
        this.Parts = SetupParts();
    }

    private List<Part> SetupParts()
    {
        return Input[1]
           .Where(l => !string.IsNullOrWhiteSpace(l))
           .Select(line =>
           {
               var matches = Regex.Matches(line, @"(\w)=(\d+)");
               var dict = matches.ToDictionary(m => m.Groups[1].Value, m => int.Parse(m.Groups[2].Value));
               return new Part(dict["x"], dict["m"], dict["s"], dict["a"]);
           }).ToList();
    }

    private Dictionary<string, List<Rule>> SetupWorkflows()
    {
        var rulez = new Dictionary<string, List<Rule>>();
        foreach (string line in Input[0])
        {
            string name = line[..line.IndexOf('{')];
            string body = line[(line.IndexOf('{') + 1)..^1];

            var rules = body.Split(',').Select(rule =>
            {
                if (!rule.Contains(':'))
                {
                    return new Rule { Next = rule, IsFallback = true };
                }

                string field = rule[0..1];
                char op = rule[1];
                var rest = rule[2..].Split(':');
                return new Rule
                {
                    Field = field,
                    Operator = op,
                    Value = int.Parse(rest[0]),
                    Next = rest[1],
                    IsFallback = false,
                };
            }).ToList();

            rulez[name] = rules;
        }

        return rulez;
    }

    private bool WorkflowSucceeds(Part part)
    {
        string current = "in";
        while (current != "A" && current != "R")
        {
            var rules = Workflows[current];
            foreach (var rule in rules)
            {
                if (rule.IsFallback)
                {
                    current = rule.Next;
                    break;
                }

                int value = rule.Field switch
                {
                    "x" => part.x,
                    "m" => part.m,
                    "a" => part.a,
                    "s" => part.s,
                    _ => throw new InvalidOperationException($"Unknown field: {rule.Field}")
                };

                bool passes = rule.Operator switch
                {
                    '<' => value < rule.Value,
                    '>' => value > rule.Value,
                    _ => throw new InvalidOperationException($"Invalid operator: {rule.Operator}")
                };

                if (passes)
                {
                    current = rule.Next;
                    break;
                }
            }
        }

        return current == "A";
    }

    private string SolvePart2()
    {
        var stack = new Stack<(string workflowId, (long xStart, long xEnd, long mStart, long mEnd, long sStart, long sEnd, long aStart, long aEnd))>();
        stack.Push(("in", (1L, 4000L, 1L, 4000L, 1L, 4000L, 1L, 4000L)));

        long totalCombinations = 0;

        while (stack.Count > 0)
        {
            var (workflowId, ranges) = stack.Pop();
            var (xStart, xEnd, mStart, mEnd, sStart, sEnd, aStart, aEnd) = ranges;

            if (workflowId == "A")
            {
                long xRange = xEnd - xStart + 1;
                long mRange = mEnd - mStart + 1;
                long sRange = sEnd - sStart + 1;
                long aRange = aEnd - aStart + 1;
                totalCombinations += xRange * mRange * sRange * aRange;
                continue;
            }

            if (workflowId == "R")
            {
                continue;
            }

            var currentRanges = ranges;

            foreach (var rule in Workflows[workflowId])
            {
                if (rule.IsFallback)
                {
                    // Fallback rule always applies to remaining range
                    if (IsValid(currentRanges))
                    {
                        stack.Push((rule.Next, currentRanges));
                    }
                    break;
                }

                (long passStart, long passEnd, long failStart, long failEnd) = rule.Field switch
                {
                    "x" => SplitRange(xStart, xEnd, rule.Operator, rule.Value),
                    "m" => SplitRange(mStart, mEnd, rule.Operator, rule.Value),
                    "s" => SplitRange(sStart, sEnd, rule.Operator, rule.Value),
                    "a" => SplitRange(aStart, aEnd, rule.Operator, rule.Value),
                    _ => throw new InvalidOperationException($"Unknown field: {rule.Field}")
                };

                var passRange = rule.Field switch
                {
                    "x" => (passStart, passEnd, mStart, mEnd, sStart, sEnd, aStart, aEnd),
                    "m" => (xStart, xEnd, passStart, passEnd, sStart, sEnd, aStart, aEnd),
                    "s" => (xStart, xEnd, mStart, mEnd, passStart, passEnd, aStart, aEnd),
                    "a" => (xStart, xEnd, mStart, mEnd, sStart, sEnd, passStart, passEnd),
                    _ => throw new InvalidOperationException()
                };

                var failRange = rule.Field switch
                {
                    "x" => (failStart, failEnd, mStart, mEnd, sStart, sEnd, aStart, aEnd),
                    "m" => (xStart, xEnd, failStart, failEnd, sStart, sEnd, aStart, aEnd),
                    "s" => (xStart, xEnd, mStart, mEnd, failStart, failEnd, aStart, aEnd),
                    "a" => (xStart, xEnd, mStart, mEnd, sStart, sEnd, failStart, failEnd),
                    _ => throw new InvalidOperationException()
                };

                currentRanges = failRange;

                if (IsValid(passRange))
                    stack.Push((rule.Next, passRange));
                else
                    break;

                // Update working values for next iteration
                (xStart, xEnd, mStart, mEnd, sStart, sEnd, aStart, aEnd) = currentRanges;
            }
        }

        return totalCombinations.ToString();
    }

    private static (long passStart, long passEnd, long failStart, long failEnd) SplitRange(long start, long end, char op, long value)
    {
        return op switch
        {
            '<' => (start, Math.Min(end, value - 1), Math.Max(start, value), end),
            '>' => (Math.Max(start, value + 1), end, start, Math.Min(end, value)),
            _ => throw new InvalidOperationException($"Unsupported operator: {op}")
        };
    }

    private static bool IsValid((long xStart, long xEnd, long mStart, long mEnd, long sStart, long sEnd, long aStart, long aEnd) r) =>
        r.xStart <= r.xEnd && r.mStart <= r.mEnd && r.sStart <= r.sEnd && r.aStart <= r.aEnd;


    private string Solve(int part)
    {
        if (part == 1)
            return Parts
                .Where(WorkflowSucceeds)
                .Select(part => part.x + part.m + part.a + part.s)
                .Sum()
                .ToString();

        return SolvePart2();
    }

    string Day.Part1() => Solve(1);
    string Day.Part2() => Solve(2);
}