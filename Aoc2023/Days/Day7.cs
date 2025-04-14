using System.Diagnostics;
using System.Text.RegularExpressions;
using Aoc2023.Days;
using Aoc2023.Input;

public class Day7 : Day
{
    private readonly IEnumerable<Hand> handsPart1;

    public Day7(string filepath)
    {
        var input = new InputReader(filepath).ReadLines();
        handsPart1 = ParseHands(input);
    }

    private record Hand(string Cards, long Wager);

    private static IEnumerable<Hand> ParseHands(List<string> input)
    {
        if (input == null)
        {
            return [];
        }
        var parts = input
            .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            ).Select(parts => new Hand(parts[0], long.Parse(parts[1])));

        return parts;
    }

    private static string Solve(IEnumerable<Hand> hands)
    {
        var hand0 = hands.First();
        return hand0.Cards;
    }

    string Day.Part1() => Solve(handsPart1);
    string Day.Part2() => Solve(handsPart1);

}