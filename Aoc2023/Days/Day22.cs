using System.Dynamic;
using Aoc2023.Days;
using Aoc2023.Input;

public class Brick
{
    public (int x, int y, int z) Start { get; set; }
    public (int x, int y, int z) End { get; set; }
    public Dictionary<Brick, Brick> SupportedBy { get; set; }
    public Dictionary<Brick, Brick> Supporting { get; set; }

    public Brick((int x, int y, int z) start, (int x, int y, int z) end)
    {
        Start = start;
        End = end;
        SupportedBy = new Dictionary<Brick, Brick>();
        Supporting = new Dictionary<Brick, Brick>();
    }
}


public partial class Day22 : Day
{
    public List<string> Input { get; private set; }
    public List<Brick> Bricks { get; private set; }

    public Day22(string filepath)
    {
        Input = new InputReader(filepath).ReadLines();
        Bricks = new List<Brick>();

        foreach (string line in Input)
        {
            var parts = line.Split('~', StringSplitOptions.TrimEntries);
            var start = parts[0].Split(',').Select(int.Parse).ToArray();
            var end = parts[1].Split(',').Select(int.Parse).ToArray();
            var brick = new Brick((start[0], start[1], start[2]), (end[0], end[1], end[2]));
            Bricks.Add(brick);
        }
        DropBricks();
    }

    private void DropBricks()
    {
        Bricks.Sort((a, b) => Math.Min(a.Start.z, a.End.z).CompareTo(Math.Min(b.Start.z, b.End.z)));
        var nonEmpty = new Dictionary<(int x, int y), int>();

        foreach (var brick in Bricks)
        {
            int zMin = Math.Min(brick.Start.z, brick.End.z);
            int zMax = Math.Max(brick.Start.z, brick.End.z);

            int dz = zMax - zMin;

            int xMin = Math.Min(brick.Start.x, brick.End.x);
            int xMax = Math.Max(brick.Start.x, brick.End.x);
            int yMin = Math.Min(brick.Start.y, brick.End.y);
            int yMax = Math.Max(brick.Start.y, brick.End.y);

            int finalZMin = 1;
            for (int x = xMin; x <= xMax; x++)
            {
                for (int y = yMin; y <= yMax; y++)
                {
                    if (nonEmpty.TryGetValue((x, y), out int z))
                    {
                        finalZMin = Math.Max(finalZMin, z + 1);
                    }
                }
            }

            int finalZMax = finalZMin + dz;

            // Update the Start and End tuples
            brick.Start = (brick.Start.x, brick.Start.y, finalZMin);
            brick.End = (brick.End.x, brick.End.y, finalZMax);

            for (int x = xMin; x <= xMax; x++)
            {
                for (int y = yMin; y <= yMax; y++)
                {
                    nonEmpty[(x, y)] = finalZMax;
                }
            }
        }
    }

    private string Solve(int part)
    {
        return "not implemented";
    }

    string Day.Part1() => Solve(1);
    string Day.Part2() => Solve(2);
}
