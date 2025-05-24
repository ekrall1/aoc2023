using Aoc2023.Days;
using Aoc2023.Input;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Linq;

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

    private static IEnumerable<List<T>> Combinations<T>(List<T> list, int k)
    {
        if (k == 0) yield return new List<T>();
        else
        {
            for (int i = 0; i < list.Count; i++)
            {
                var remaining = list.Skip(i + 1).ToList();
                foreach (var combo in Combinations(remaining, k - 1))
                {
                    combo.Insert(0, list[i]);
                    yield return combo;
                }
            }
        }
    }
    private string Solve(int part)
    {
        if (part == 1)
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
        }
        else if (part == 2)
        {
            // Use 4 hailstones to set up 6 equations in 6 unknowns (rx, ry, rz, vx, vy, vz) (for the rock)
            // Each hailstone satisfies: (rx - px) / (vx - vxh) = (ry - py) / (vy - vyh) = (rz - pz) / (vz - vzh) = t
            // where px, py, pz is the hailstone's position and vxh, vyh, vzh is its velocity.

            var h0 = Hailstones[0];
            var h1 = Hailstones[1];
            var h2 = Hailstones[2];
            var h3 = Hailstones[3];

            var hs = new[] { h0, h1, h2, h3 };
            var A = new double[6, 6];
            var B = new double[6];

            for (int i = 1; i <= 3; i++)
            {
                var a = hs[0];
                var b = hs[i];

                // From: (rx - ax)/(vx - avx) = (ry - ay)/(vy - avy)
                // Cross-multiplied and rearranged:
                // rx*(vy - avy) - ry*(vx - avx) = ax*(vy - avy) - ay*(vx - avx)

                // x-y equation coefficients
                A[2 * (i - 1), 0] = (b.v.y - a.v.y); // rx
                A[2 * (i - 1), 1] = -(b.v.x - a.v.x); // ry
                A[2 * (i - 1), 3] = -(b.p.y - a.p.y); // vx
                A[2 * (i - 1), 4] = (b.p.x - a.p.x); // vy
                B[2 * (i - 1)] = (b.p.x * b.v.y - a.p.x * a.v.y) - (b.p.y * b.v.x - a.p.y * a.v.x);

                // x-z equation coefficients
                A[2 * (i - 1) + 1, 0] = (b.v.z - a.v.z); // rx
                A[2 * (i - 1) + 1, 2] = -(b.v.x - a.v.x); // rz
                A[2 * (i - 1) + 1, 3] = -(b.p.z - a.p.z); // vx
                A[2 * (i - 1) + 1, 5] = (b.p.x - a.p.x); // vz
                B[2 * (i - 1) + 1] = (b.p.x * b.v.z - a.p.x * a.v.z) - (b.p.z * b.v.x - a.p.z * a.v.x);
            }

            var matrix = Matrix<double>.Build.DenseOfArray(A);
            var vector = Vector<double>.Build.DenseOfArray(B);
            var solution = matrix.Solve(vector);

            long rx = (long)Math.Round(solution[0]);
            long ry = (long)Math.Round(solution[1]);
            long rz = (long)Math.Round(solution[2]);

            return (rx + ry + rz).ToString();
        }
        return "";
    }

    string Day.Part1() => Solve(1);
    string Day.Part2() => Solve(2);
}