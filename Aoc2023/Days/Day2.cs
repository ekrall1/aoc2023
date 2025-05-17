using Aoc2023.Days;
using Aoc2023.Input;

public class ColorMap
{
    public Dictionary<string, int> colorMap;
    public ColorMap()
    {
        this.colorMap = new Dictionary<string, int> {
            {"red", 0},
            {"green", 0},
            {"blue", 0},
        };
    }
}
public class Day2 : Day
{
    public string FilePath { get; private set; }
    public List<string> InputList { get; private set; }
    public static char[] Separator1 { get; } = [';'];
    public static char[] Separator2 { get; } = [','];
    public static char[] Separator3 { get; } = [' '];
    public static Dictionary<string, int> Maximums { get; } = new Dictionary<string, int> {
        {"red", 12},
        {"green", 13},
        {"blue", 14},
    };

    public Day2(string filepath)
    {
        this.FilePath = filepath;
        InputReader fileInput = new InputReader(this.FilePath);
        this.InputList = fileInput.ReadLines();
    }

    static List<(string color, int count)> ParseCubes(string game)
    {
        return game
            .Split(Separator2, StringSplitOptions.TrimEntries)
            .Select(item => item.Split(Separator3, StringSplitOptions.TrimEntries))
            .Select(parts => (parts[1].Trim(), int.Parse(parts[0].Trim())))
            .ToList();
    }

    static string[] ParseGames(string input)
    {
        return input
            .Split(Separator1, StringSplitOptions.TrimEntries);
    }

    private int GetGameId(string input)
    {
        return int.Parse(input.Split(Separator3, StringSplitOptions.TrimEntries)[1]);
    }

    private bool IsValidGame(string game)
    {
        var cubes = ParseCubes(game);
        return cubes.All(cube => cube.count <= Maximums[cube.color]);
    }

    private string IterInputListP1(List<string> lines)
    {
        int totalSum = lines
            .Select(line => line.Split(':'))
            .Select(parts => new { GameId = GetGameId(parts[0]), Games = ParseGames(parts[1]) })
            .Where(entry => entry.Games.All(IsValidGame))
            .Sum(entry => entry.GameId);
        return totalSum.ToString();
    }

    private string IterInputListP2(List<string> lines)
    {
        int totalPower = lines
            .Select(line => line.Split(':')[1])
            .Select(ParseGames)
            .Select(games =>
            {
                var maxCounts = new Dictionary<string, int> { { "red", 0 }, { "green", 0 }, { "blue", 0 } };

                foreach (var game in games)
                {
                    foreach (var (color, count) in ParseCubes(game))
                    {
                        maxCounts[color] = Math.Max(maxCounts[color], count);
                    }
                }

                return maxCounts["red"] * maxCounts["green"] * maxCounts["blue"];
            }).Sum();

        return totalPower.ToString();
    }

    private string SolvePart1(List<string> lines)
    {
        return IterInputListP1(lines);
    }

    private string SolvePart2(List<string> lines)
    {
        return IterInputListP2(lines);
    }

    string Day.Part1()
    {
        return SolvePart1(this.InputList);
    }

    string Day.Part2()
    {
        return SolvePart2(this.InputList);
    }
}