using System.Runtime.CompilerServices;
using Aoc2023.Days;
using Aoc2023.Input;

public class ColorMap {

    public Dictionary<string, int> colorMap;
    public ColorMap() {
        this.colorMap = new Dictionary<string, int> {
            {"red", 0},
            {"green", 0},
            {"blue", 0},
        };
    }
}
public class Day2 : Day
{

    private string _filepath;
    private List<string> _inputList;
    private static readonly char[] separator1 = [';'];
    private static readonly char[] separator2 = [','];

    private static readonly char[] separator3 = [' '];

    private static readonly Dictionary<string, int> maximums = new Dictionary<string, int> {
            {"red", 12},
            {"green", 13},
            {"blue", 14},
    };

    public Day2(string filepath)
    {
        this._filepath = filepath;
        InputReader fileInput = new InputReader(this._filepath);
        this._inputList = fileInput.ReadLines();
    }

    static List<(string color, int count)> ParseCubes(string game) {
        return game
            .Split(separator2, StringSplitOptions.TrimEntries)
            .Select(item => item.Split(separator3, StringSplitOptions.TrimEntries))
            .Select(parts => (parts[1].Trim(), int.Parse(parts[0].Trim())))
            .ToList();
    }

    static string[] ParseGames(string input) {
        return input
            .Split(separator1, StringSplitOptions.TrimEntries);
    }

    private int GetGameId(string input)
    {
        return int.Parse(input.Split(separator3, StringSplitOptions.TrimEntries)[1]);
    }

    private string IterInputListP1(List<string> lines) {
        int totalSum = 0;
        foreach (string line in lines) {
            var colorCounts = new ColorMap().colorMap;
            string[] parts = line.Split(':');
            int gameId = GetGameId(parts[0]);

            var games = ParseGames(parts[1]);

            bool isValid = false;
            foreach(string game in games) {
                var cubes = ParseCubes(game);
                isValid = cubes.All(cube => cube.count <= maximums[cube.color]);
                if (!isValid) {
                    break;
                }
            }
            if (isValid) {
                totalSum += gameId;
            }
        }
        return totalSum.ToString();
    }


    private string SolvePart1(List<string> lines)
    {
        return IterInputListP1(lines).ToString();
    }

    private string SolvePart2(List<string> lines)
    {
        return "not implemented";
    }

    string Day.Part1()
    {
        return SolvePart1(this._inputList);
    }

    string Day.Part2()
    {
        return SolvePart2(this._inputList);
    }

}