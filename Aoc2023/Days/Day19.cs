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

    private string Solve(int part)
    {
        if (part == 1)
            return Parts
                .Where(WorkflowSucceeds)
                .Select(part => part.x + part.m + part.a + part.s)
                .Sum()
                .ToString();

        return "Part not implemented";
    }

    string Day.Part1() => Solve(1);
    string Day.Part2() => Solve(2);
}
