using System.Text.RegularExpressions;
using Aoc2023.Days;
using Aoc2023.Input;

public class Day6 : Day
{
    private string filepath;
    private List<string> inputList;
    private List<Dictionary<string, int>> races;
    public Day6(string filepath)
    {
        this.filepath = filepath;
        InputReader fileInput = new InputReader(this.filepath);
        this.inputList = fileInput.ReadLines();
        this.races = this.ParseRaces();
    }

    private List<Dictionary<string, int>> ParseRaces()
    {
        List<List<string>> raceInfo = [];
        List<Dictionary<string, int>> tmpRaces = [];
        foreach (var input in this.inputList)
        {
            List<string> parts = input.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            raceInfo.Add(parts[1].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList());
        }
        for (int i = 0; i < raceInfo[0].Count; i++)
        {
            tmpRaces.Add(new Dictionary<string, int>
            {
                ["time"] = int.Parse(raceInfo[0][i].Trim()),
                ["distance"] = int.Parse(raceInfo[1][i].Trim())
            });

        }

        return tmpRaces;

    }

    private (double, double) SolveRace(long a, long b, long c)
    {
        var det1 = Math.Sqrt(b * b - (4 * a * c));
        var det2 = 2 * a;
        var root1 = (-1 * b + det1) / det2;
        var root2 = (-1 * b - det1) / det2;

        return (Math.Floor(root1), Math.Ceiling(root2));
    }


    string Day.Part1()
    {
        double ans = 1;
        foreach (var race in this.races)
        {
            var a = 1;
            var b = -1 * race["time"];
            var c = race["distance"] + 1;
            var (root1, root2) = SolveRace(a, b, c);
            ans = ans * (Math.Abs(root1 - root2) + 1);
        }
        return ans.ToString();
    }

    string Day.Part2()
    {
        return "not implemented";

    }

}