
using Aoc2023.Days;
public class RunDayReceiver
{

    private readonly Dictionary<string, Func<string, Day>> _days = new Dictionary<string, Func<string, Day>> {
        {"1", filepath => new Day1(filepath)},
        {"2", filepath => new Day2(filepath)},
        {"3", filepath => new Day3(filepath)},
        {"4", filepath => new Day4(filepath)},
        {"5", filepath => new Day5(filepath) },
        {"6", filepath => new Day6(filepath) },
        {"7", filepath => new Day7(filepath) },
        {"8", filepath => new Day8(filepath) },
        {"9", filepath => new Day9(filepath) },
        {"10", filepath => new Day10(filepath) },
        {"11", filepath => new Day11(filepath) },
        {"12", filepath => new Day12(filepath) },
        {"13", filepath => new Day13(filepath) },

    };
    public void RunDay(string filepath, string day, string part)
    {
        if (_days.TryGetValue(day, out Func<string, Day>? dayRunner))
        {
            Day runner = dayRunner(filepath);
            if (part == "1")
            {
                Console.WriteLine($"part 1 solution is: {runner.Part1()}");
            }
            if (part == "2")
            {
                Console.WriteLine($"part 2 solution is: {runner.Part2()}");
            }
        }
    }
}