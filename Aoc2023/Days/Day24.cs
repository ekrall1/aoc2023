using System.Drawing;
using Aoc2023.Days;
using Aoc2023.Input;
public partial class Day24 : Day
{
    public record Position(long x, long y, long z);
    public record Velocity(long x, long y, long z);
    public record Hailstone(Position p, Velocity v);
    public List<string> Input { get; private set; }

    private List<Hailstone> Hailstones { get; set; } = new();

    public Day24(string filepath)
    {
        Input = new InputReader(filepath).ReadLines();
        for (int s = 0; s < Input.Count; s++)
        {
            string line = Input[s];
            var parts = line.Split('@', StringSplitOptions.TrimEntries);
            var position = parts[0].Split(',').Select(long.Parse).ToArray();
            var velocity = parts[1].Split(',').Select(long.Parse).ToArray();
            var hailstone = new Hailstone(
                new Position(position[0], position[1], position[2]),
                new Velocity(velocity[0], velocity[1], velocity[2])
            );
            Hailstones.Add(hailstone);
        }
    }

    bool TryIntersection(Hailstone a, Hailstone b, out decimal x, out decimal y)
    {
        x = -1;
        y = -1;

        decimal x1 = a.p.x, y1 = a.p.y;
        decimal vx1 = a.v.x, vy1 = a.v.y;
        decimal x2 = b.p.x, y2 = b.p.y;
        decimal vx2 = b.v.x, vy2 = b.v.y;

        // Solve: x1 + vx1 * t1 == x2 + vx2 * t2
        //        y1 + vy1 * t1 == y2 + vy2 * t2
        // Rearranged: 
        //     x1 - x2 + vx1*t1 - vx2*t2 = 0
        //     y1 - y2 + vy1*t1 - vy2*t2 = 0

        // Let A = vx1, B = -vx2, C = x2 - x1
        //     D = vy1, E = -vy2, F = y2 - y1
        // Solve this linear system:
        //     A*t1 + B*t2 = C
        //     D*t1 + E*t2 = F

        decimal A = vx1, B = -vx2, C = x2 - x1;
        decimal D = vy1, E = -vy2, F = y2 - y1;

        decimal det = A * E - B * D;

        if (Math.Abs(det) < 1e-5m)
            return false;

        // Cramer's Rule
        decimal t1 = (C * E - B * F) / det;
        decimal t2 = (A * F - C * D) / det;

        if (t1 < 0 || t2 < 0)
            return false; // Intersection is in the past for one or both

        x = x1 + vx1 * t1;
        y = y1 + vy1 * t1;
        return true;
    }

    private string Solve(int part)
    {
        var intersections = 0;
        decimal min = 200000000000000m;
        decimal max = 400000000000000m;
        for (int i = 0; i < Hailstones.Count; i++)
        {
            for (int j = i + 1; j < Hailstones.Count; j++)
            {
                if (TryIntersection(Hailstones[i], Hailstones[j], out decimal x, out decimal y))
                {
                    if (x >= min && x <= max && y >= min && y <= max)
                    {
                        intersections++;
                    }
                }
            }
        }
        return intersections.ToString();
        throw new NotImplementedException($"Part {part} is an invalid part.");
    }

    string Day.Part1() => Solve(1);
    string Day.Part2() => Solve(2);
}