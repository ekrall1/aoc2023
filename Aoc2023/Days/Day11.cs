using Aoc2023;
using Aoc2023.Days;
using Aoc2023.Input;

public class Day11 : Day
{
    public List<string> RawInput { get; private set; }

    public Day11(string filepath)
    {
        this.RawInput = new InputReader(filepath).ReadLines();
    }

    private List<((int, int), (int, int))> ExpandInput(List<string> input, int part)
    {
        int factor = part == 1 ? 2 : 1_000_000;
        List<char[]> originalInput = input.Select(row => row.ToCharArray()).ToList();

        List<(int, int)> points = new List<(int, int)>();

        for (int r = 0; r < originalInput.Count; r++)
        {
            for (int c = 0; c < originalInput[r].Count(); c++)
            {
                if (originalInput[r][c] == '#')
                {
                    points.Add((r, c));
                }
            }
        }

        // rows
        int rowsAdded = 0;
        for (int r = 0; r < originalInput.Count; r++)
        {
            if (originalInput[r].ToHashSet<char>().Count == 1)
            {
                points = points.Select(point =>
                {
                    if (point.Item1 >= r + rowsAdded)
                    {
                        point.Item1 += factor - 1;
                    }
                    return point;
                }).ToList();
                rowsAdded += factor - 1;
            }
        }

        // cols
        int colsAdded = 0;
        for (int c = 0; c < originalInput.Count; c++)
        {
            string column = "";
            for (int r = 0; r < originalInput.Count; r++)
            {
                column += originalInput[r][c];
            }
            if (column.ToHashSet<char>().Count == 1)
            {
                points = points.Select(point =>
                {
                    if (point.Item2 >= c + colsAdded)
                    {
                        point.Item2 += factor - 1;
                    }
                    return point;
                }).ToList();
                colsAdded += factor - 1;
            }
        }

        return GetAllPairs(points);
    }

    static List<((int, int), (int, int))> GetAllPairs(List<(int, int)> list)
    {
        var result = new List<((int, int), (int, int))>();

        for (int i = 0; i < list.Count; i++)
        {
            for (int j = i + 1; j < list.Count; j++)
            {
                result.Add((list[i], list[j]));
            }
        }

        return result;
    }

    private string Solve(int part)
    {
        var pairs = this.ExpandInput(this.RawInput, part);
        long totalSum = 0;
        foreach (((int, int), (int, int)) pair in pairs)
        {
            var distance = new Distance(pair.Item1, pair.Item2);
            totalSum += distance.Manhattan();
        }

        return totalSum.ToString();
    }

    string Day.Part1() => Solve(1);
    string Day.Part2() => Solve(2);
}