using Aoc2023.Days;
using Aoc2023.Input;

public class Day13 : Day
{
    private List<string> input;
    private List<List<string>> puzzlePatterns;
    private List<List<string>> transpose;

    public Day13(string filepath)
    {
        this.input = new InputReader(filepath).ReadLines();
        this.puzzlePatterns = [];
        this.transpose = [];

        var tmp = this.input
            .Aggregate(
                new List<List<string>> { new List<string>() },
                (acc, line) =>
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        acc.Add(new List<string>());
                    }
                    else
                    {
                        acc.Last().Add(line);
                    }
                    return acc;
                });

        foreach (var puzz in tmp)
        {
            if (puzz.Count > 0)
            {
                this.puzzlePatterns.Add(puzz);
                this.transpose.Add(GetPuzzleTranspose(puzz));
            }
        }
    }
    private record RowSymmetry(bool hasSymmetry, int i, int j);
    private record ColSymmetry(bool hasSymmetry, int i, int j);

    private List<string> GetPuzzleTranspose(List<string> puzz)
    {
        if (puzz == null || puzz.Count == 0)
            return [];

        return Enumerable.Range(0, puzz[0].Length)
            .Select(col => new string(puzz.Select(row => row[col]).ToArray()))
            .ToList();
    }

    public static int HammingDistance(string a, string b)
    {
        if (a.Length != b.Length)
        {
            throw new ArgumentException("Strings must be of the same length");
        }

        return a.Zip(b, (char1, char2) => char1 != char2 ? 1 : 0).Sum();
    }

    private static RowSymmetry CheckRows(List<string> puzzle, int allowedDist, RowSymmetry? ignore = null)
    {
        RowSymmetry result = new RowSymmetry(false, -1, -1);

        for (int i = 0; i < puzzle.Count - 1; i++)
        {
            int j = i + 1;

            int mismatch = HammingDistance(puzzle[i], puzzle[j]);
            if (mismatch > allowedDist)
                continue;

            bool symmetric = true;
            for (int m = i - 1, n = j + 1; m >= 0 && n < puzzle.Count; m--, n++)
            {
                int curDist = HammingDistance(puzzle[m], puzzle[n]);

                mismatch += curDist;
                if (mismatch > allowedDist)
                {
                    symmetric = false;
                    break;
                }
            }

            if (symmetric && mismatch == allowedDist)
            {
                if (ignore is null || !(ignore.i == i )) // catch - part 1 result must be ignored
                {
                    result = new RowSymmetry(true, i, j);
                    break;
                }

            }
        }
        return result;
    }

    private string Solve(int part)
    {
        var score = 0;

        for (int idx = 0; idx < this.puzzlePatterns.Count; idx++)
        {
            var pattern = this.puzzlePatterns[idx];
            var transposed = this.transpose[idx];

            var rowSymPart1 = CheckRows(pattern, 0);
            var colSymPart1 = CheckRows(transposed, 0);

            if (part == 1)
            {
                score += rowSymPart1.hasSymmetry
                    ? 100 * (rowSymPart1.i + 1)
                    : (colSymPart1.i + 1);
            }
            else
            {
                var rowSymPart2 = CheckRows(pattern, 1, rowSymPart1);
                var colSymPart2 = CheckRows(transposed, 1, colSymPart1);

                score += rowSymPart2.hasSymmetry
                    ? 100 * (rowSymPart2.i + 1)
                    : (colSymPart2.i + 1);
            }
        }
        return score.ToString();
    }

    string Day.Part1() => Solve(1);
    string Day.Part2() => Solve(2);

}