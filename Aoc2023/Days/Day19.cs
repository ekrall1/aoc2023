using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Aoc2023;
using Aoc2023.Days;
using Aoc2023.Input;
using Aoc2023.Utils;


public partial class Day19 : Day
{
    private List<List<string>> input;
    private record Part(int x, int m, int s, int a);

    private Dictionary<string, List<Rule>> workflows;

    private List<Part> parts;

    private class Rule
    {
        public string? Field { get; set; }
        public int Value { get; set; }
        public char Operator { get; set; }
        public required string Next { get; set; }
        public bool IsFallback { get; set; }

    }

    public Day19(string filepath)
    {
        this.input = new InputReader(filepath).ReadTwoPartLines();
        this.workflows = SetupWorkflows();
        this.parts = SetupParts();
    }

    private List<Part> SetupParts()
    {
        return input[1]
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
        foreach (string line in input[0])
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
            var rules = workflows[current];
            foreach (var rule in rules)
            {
                // If the rule has no condition, it's the default jump
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



    private string Solve(int part)
    {
        if (part == 1)
            return parts
                .Where(WorkflowSucceeds)
                .Select(part => part.x + part.m + part.a + part.s)
                .Sum()
                .ToString();

        return "Part not implemented";
    }

    string Day.Part1() => Solve(1);
    string Day.Part2() => Solve(2);

}