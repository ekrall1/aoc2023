using System.Data;
using Aoc2023;
using Aoc2023.Days;
using Aoc2023.Input;

public class Day15 : Day
{
    private List<string> input;
    private List<string> parsedInput;

    public Day15(string filepath)
    {
        this.input = new InputReader(filepath).ReadLines();
        this.parsedInput = new string(string.Join(' ', this.input)
            .Where(c => !char.IsWhiteSpace(c))
            .ToArray())
            .Split(',')
            .ToList();
    }

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

    private string Solve(int part)
    {
        return this.parsedInput.Aggregate(0L, (acc, code) =>
        {
            acc += HashingFunction(code);
            return acc;
        }).ToString();
    }

    string Day.Part1() => Solve(1);
    string Day.Part2() => Solve(2);

}