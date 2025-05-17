using System.Data;
using System.Text.RegularExpressions;
using Aoc2023.Days;
using Aoc2023.Input;

public partial class Day15 : Day
{
    public List<string> Input { get; private set; }
    public List<string> ParsedInput { get; private set; }
    public Dictionary<long, List<Lens>> BoxesHashMap { get; private set; }

    public Day15(string filepath)
    {
        this.Input = new InputReader(filepath).ReadLines();
        this.ParsedInput = new string(string.Join(' ', this.Input)
            .Where(c => !char.IsWhiteSpace(c))
            .ToArray())
            .Split(',')
            .ToList();
        this.BoxesHashMap = new Dictionary<long, List<Lens>>();
        foreach (var n in Enumerable.Range(0, 256))
        {
            this.BoxesHashMap[n] = new List<Lens>();
        }
    }

    [GeneratedRegex(@"([a-z]+)([-=])([0-9]?)")]
    private static partial Regex LensRegex();

    public record Lens(string Name, int Focus);

    private long HashingFunction(string data)
    {
        return data.Aggregate(0L, (acc, c) =>
        {
            acc += (long)c;
            acc = (acc << 4) + acc;
            acc = acc & 0xFF;
            return acc;
        });
    }

    private void HandleLabelPart2(string boxData)
    {
        var match = LensRegex().Match(boxData);

        if (!match.Success)
            return;
        var parts = match.Groups.Cast<Group>().Skip(1).ToList();
        var labelHash = HashingFunction(parts[0].Value);

        var idx = this.BoxesHashMap[labelHash].FindIndex(labelHash => labelHash.Name == parts[0].Value);

        if (parts[1].Value == "=")
            if (idx == -1)
                this.BoxesHashMap[labelHash].Add(new Lens(parts[0].Value, int.Parse(parts[2].Value)));
            else
                this.BoxesHashMap[labelHash][idx] = new Lens(parts[0].Value, int.Parse(parts[2].Value));
        else if (parts[1].Value == "-")
            this.BoxesHashMap[labelHash].RemoveAll(l => l.Name == parts[0].Value);
        else
            throw new ArgumentException($"Invalid operator: {parts[1].Value}");
    }

    private string Solve(int part)
    {
        if (part == 1)
            return this.ParsedInput.Aggregate(0L, (acc, code) =>
            {
                acc += HashingFunction(code);
                return acc;
            }).ToString();

        for (int i = 0; i < this.ParsedInput.Count; i++)
        {
            HandleLabelPart2(this.ParsedInput[i]);
        }

        var res = 0;
        for (int idx = 0; idx < this.BoxesHashMap.Count; idx++)
        {
            for (int boxIdx = 0; boxIdx < this.BoxesHashMap[idx].Count; boxIdx++)
            {
                var determinant = 1;
                determinant *= (idx + 1);
                determinant *= (boxIdx + 1);
                determinant *= this.BoxesHashMap[idx][boxIdx].Focus;
                res += determinant;
            }
        }

        return res.ToString();
    }

    string Day.Part1() => Solve(1);
    string Day.Part2() => Solve(2);
}
