using System.Data;
using System.Text.RegularExpressions;
using Aoc2023.Days;
using Aoc2023.Input;

public partial class Day15 : Day
{
    private List<string> input;
    private List<string> parsedInput;
    private Dictionary<long, List<Lens>> boxesHashMap;

    public Day15(string filepath)
    {
        this.input = new InputReader(filepath).ReadLines();
        this.parsedInput = new string(string.Join(' ', this.input)
            .Where(c => !char.IsWhiteSpace(c))
            .ToArray())
            .Split(',')
            .ToList();
        this.boxesHashMap = new Dictionary<long, List<Lens>>();
        foreach (var n in Enumerable.Range(0, 256))
        {
            this.boxesHashMap[n] = new List<Lens>();
        }
    }

    [GeneratedRegex(@"([a-z]+)([-=])([0-9]?)")]
    private static partial Regex LensRegex();

    private record Lens(string Name, int Focus);

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

        var idx = this.boxesHashMap[labelHash].FindIndex(labelHash => labelHash.Name == parts[0].Value);


        if (parts[1].Value == "=")
            if (idx == -1)
                this.boxesHashMap[labelHash].Add(new Lens(parts[0].Value, int.Parse(parts[2].Value)));
            else
                this.boxesHashMap[labelHash][idx] = new Lens(parts[0].Value, int.Parse(parts[2].Value));
        else if (parts[1].Value == "-")
            this.boxesHashMap[labelHash].RemoveAll(l => l.Name == parts[0].Value);
        else
            throw new ArgumentException($"Invalid operator: {parts[1].Value}");
    }
    private string Solve(int part)
    {
        if (part == 1)
            return this.parsedInput.Aggregate(0L, (acc, code) =>
            {
                acc += HashingFunction(code);
                return acc;
            }).ToString();

        for (int i = 0; i < this.parsedInput.Count; i++)
        {
            HandleLabelPart2(this.parsedInput[i]);
        }

        var res = 0;
        for (int idx = 0; idx < this.boxesHashMap.Count; idx++)
        {
            for (int boxIdx = 0; boxIdx < this.boxesHashMap[idx].Count; boxIdx++)
            {
                var determinant = 1;
                determinant *= (idx + 1);
                determinant *= (boxIdx + 1);
                determinant *= this.boxesHashMap[idx][boxIdx].Focus;
                res += determinant;
            }
        }

        return res.ToString();
    }

    string Day.Part1() => Solve(1);
    string Day.Part2() => Solve(2);

}