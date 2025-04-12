using System.Text.RegularExpressions;
using Aoc2023.Days;
using Aoc2023.Input;

public class Day6 : Day
{
    private string filepath;
    private List<string> inputList;
    private Dictionary<string, List<Dictionary<string, long>>> races;
    public Day6(string filepath)
    {
        this.filepath = filepath;
        InputReader fileInput = new InputReader(this.filepath);
        this.inputList = fileInput.ReadLines();
        this.races = this.ParseRaces();
    }

    private Dictionary<string, List<Dictionary<string, long>>> ParseRaces()
    {
        List<List<string>> raceInfo = [];
        List<string> raceInfoPart2 = [];
        Dictionary<string, List<Dictionary<string, long>>> tmpRaces = new Dictionary<string, List<Dictionary<string, long>>>();
        tmpRaces["1"] = new List<Dictionary<string, long>>();
        tmpRaces["2"] = new List<Dictionary<string, long>>();

        foreach (var input in this.inputList)
        {
            List<string> parts = input.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var numPart = parts[1].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
            raceInfo.Add(numPart);
            raceInfoPart2.Add(string.Join("", numPart));
        }

        for (int i = 0; i < raceInfo[0].Count; i++)
        {
            tmpRaces["1"].Add(new Dictionary<string, long>
            {
                ["time"] = long.Parse(raceInfo[0][i].Trim()),
                ["distance"] = long.Parse(raceInfo[1][i].Trim()),
            });
        }
        tmpRaces["2"].Add(new Dictionary<string, long>
        {
            ["time"] = long.Parse(raceInfoPart2[0].Trim()),
            ["distance"] = long.Parse(raceInfoPart2[1].Trim()),
        });

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
        foreach (var race in this.races["1"])
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
        double ans = 1;
        foreach (var race in this.races["2"])
        {
            var a = 1;
            var b = -1 * race["time"];
            var c = race["distance"] + 1;
            var (root1, root2) = SolveRace(a, b, c);
            ans = ans * (Math.Abs(root1 - root2) + 1);
        }
        return ans.ToString();

    }

}