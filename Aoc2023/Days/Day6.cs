using System.Diagnostics;
using System.Text.RegularExpressions;
using Aoc2023.Days;
using Aoc2023.Input;

public class Day6 : Day
{
    private readonly List<Race> racesPart1;
    private readonly List<Race> racesPart2;

    public Day6(string filepath)
    {
        var input = new InputReader(filepath).ReadLines();
        (racesPart1, racesPart2) = ParseRaces(input);
    }

    private record Race(long Time, long Distance);

    private static (List<Race> part1, List<Race> part2) ParseRaces(List<string> input)
    {
        var parts = input
            .Select(line => line.Split(':')[1]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .ToList();

        var part1 = parts[0]
            .Zip(parts[1], (time, distance) => new Race(long.Parse(time), long.Parse(distance)))
            .ToList();

        var time2 = long.Parse(string.Concat(parts[0]));
        var distance2 = long.Parse(string.Concat(parts[1]));
        var part2 = new List<Race> { new(time2, distance2) };

        return (part1, part2);
    }

    private static (double Lower, double Upper) SolveRace(long time, long distance)
    {
        double a = 1, b = -time, c = distance + 1;
        double discriminant = Math.Sqrt(b * b - 4 * a * c);
        double root1 = (-b + discriminant) / (2 * a);
        double root2 = (-b - discriminant) / (2 * a);
        return (Math.Floor(root1), Math.Ceiling(root2));
    }


    private static string Solve(IEnumerable<Race> races)
    {
        double result = 1;
        foreach (var race in races)
        {
            var (low, high) = SolveRace(race.Time, race.Distance);
            result *= Math.Abs(high - low) + 1;
        }
        return result.ToString();
    }

    string Day.Part1() => Solve(racesPart1);
    string Day.Part2() => Solve(racesPart2);

}