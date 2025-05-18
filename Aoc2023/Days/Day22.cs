using Aoc2023.Days;
using Aoc2023.Input;

public interface IBrick
{
    (int x, int y, int z) Start { get; set; }
    (int x, int y, int z) End { get; set; }
    int Id { get; set; }
    HashSet<Brick> SupportedBy { get; set; }
    HashSet<Brick> Supporting { get; set; }
}
public class Brick : IBrick
{
    public (int x, int y, int z) Start { get; set; }
    public (int x, int y, int z) End { get; set; }
    public HashSet<Brick> SupportedBy { get; set; } = new();
    public HashSet<Brick> Supporting { get; set; } = new();
    public int Id { get; set; } = 0;
    public Brick((int x, int y, int z) start, (int x, int y, int z) end)
    {
        Start = start;
        End = end;
    }
}


public partial class Day22 : Day
{
    public List<string> Input { get; private set; }
    public List<Brick> Bricks { get; private set; }

    public Dictionary<(int x, int y, int z), Brick> Positions { get; private set; }

    public Day22(string filepath)
    {
        Input = new InputReader(filepath).ReadLines();
        Bricks = new List<Brick>();
        Positions = new Dictionary<(int x, int y, int z), Brick>();

        for (int s = 0; s < Input.Count; s++)
        {
            string line = Input[s];
            var parts = line.Split('~', StringSplitOptions.TrimEntries);
            var start = parts[0].Split(',').Select(int.Parse).ToArray();
            var end = parts[1].Split(',').Select(int.Parse).ToArray();
            var brick = new Brick((start[0], start[1], start[2]), (end[0], end[1], end[2]));
            brick.Id = s;
            Bricks.Add(brick);
        }
        DropBricks();
        BuildSupportGraph();
    }

    private void DropBricks()
    {
        Bricks.Sort((a, b) => Math.Min(a.Start.z, a.End.z).CompareTo(Math.Min(b.Start.z, b.End.z)));
        var nonEmpty = new Dictionary<(int x, int y), int>();

        foreach (Brick brick in Bricks)
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

    private void BuildSupportGraph()
    {
        // Clear previous data
        Positions.Clear();
        foreach (var brick in Bricks)
        {
            brick.SupportedBy.Clear();
            brick.Supporting.Clear();
        }

        // 1. Add every brick's occupied positions to Positions
        foreach (var brick in Bricks)
        {
            int xMin = Math.Min(brick.Start.x, brick.End.x);
            int xMax = Math.Max(brick.Start.x, brick.End.x);
            int yMin = Math.Min(brick.Start.y, brick.End.y);
            int yMax = Math.Max(brick.Start.y, brick.End.y);
            int zMin = Math.Min(brick.Start.z, brick.End.z);
            int zMax = Math.Max(brick.Start.z, brick.End.z);

            for (int x = xMin; x <= xMax; x++)
            {
                for (int y = yMin; y <= yMax; y++)
                {
                    for (int z = zMin; z <= zMax; z++)
                    {
                        Positions[(x, y, z)] = brick;
                    }
                }
            }
        }

        // 2. For each brick, check for bricks directly below it and build support relationships
        foreach (var brick in Bricks)
        {
            int xMin = Math.Min(brick.Start.x, brick.End.x);
            int xMax = Math.Max(brick.Start.x, brick.End.x);
            int yMin = Math.Min(brick.Start.y, brick.End.y);
            int yMax = Math.Max(brick.Start.y, brick.End.y);
            int zMin = Math.Min(brick.Start.z, brick.End.z);

            for (int x = xMin; x <= xMax; x++)
            {
                for (int y = yMin; y <= yMax; y++)
                {
                    var belowPos = (x, y, zMin - 1);
                    if (Positions.TryGetValue(belowPos, out var belowBrick))
                    {
                        brick.SupportedBy.Add(belowBrick);
                        belowBrick.Supporting.Add(brick);
                    }
                }
            }
        }
    }

    private string Solve(int part)
    {
        if (part == 1)
        {
            int removable = 0;
            foreach (var brick in Bricks)
            {
                // Simulate removing this brick
                var toRemove = brick;
                bool canRemove = true;
                foreach (var supported in toRemove.Supporting)
                {
                    // If any supported brick would have no other support, can't remove
                    if (supported.SupportedBy.Count == 1)
                    {
                        canRemove = false;
                        break;
                    }
                }
                if (canRemove)
                {
                    removable++;
                }

            }
            return removable.ToString();
        }
        else if (part == 2)
        {
            int total = 0;
            foreach (var brick in Bricks)
            {
                HashSet<Brick> fallenBricks = new HashSet<Brick>();

                var queue = new Queue<Brick>();

                fallenBricks.Add(brick);
                queue.Enqueue(brick);

                while (queue.Count > 0)
                {
                    var currentBrick = queue.Dequeue();
                    foreach (Brick candidateBrick in currentBrick.Supporting)
                    {
                        if (fallenBricks.Contains(candidateBrick))
                            continue;

                        if (candidateBrick.SupportedBy.All(br => fallenBricks.Contains(br)))
                        {
                            fallenBricks.Add(candidateBrick);
                            queue.Enqueue(candidateBrick);
                        }
                    }
                }
                total += fallenBricks.Count - 1;
            }
            return total.ToString();
        }
        else
            throw new NotImplementedException($"Part {part} is an invalid part. Only parts 1 and 2 are valid.");
    }

    string Day.Part1() => Solve(1);
    string Day.Part2() => Solve(2);
}