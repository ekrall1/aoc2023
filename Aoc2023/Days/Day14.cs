using System.Data;
using Aoc2023;
using Aoc2023.Days;
using Aoc2023.Input;

public class Day14 : Day
{
    private List<string> input;

    public Day14(string filepath)
    {
        this.input = new InputReader(filepath).ReadLines();
    }
    private List<string> TiltNorth(List<string> data)
    {
        var sortedCols = Enumerable.Range(0, data[0].Length)
            .Select(col => new string(data.Select(row => row[col]).ToArray()))
            .Select(colStr => colStr.Split('#')
                .Select(part => new string(part.OrderByDescending(ch => ch).ToArray()))
             )
            .Select(sorted => string.Join('#', sorted)).ToList();

        return Enumerable.Range(0, sortedCols[0].Length)
            .Select(col => new string(sortedCols.Select(row => row[col])
            .ToArray())).ToList();
    }

    private List<string> TiltWest(List<string> data)
    {
        var sortedRows = data
            .Select(rowStr => rowStr.Split('#')
                .Select(part => new string(part.OrderByDescending(ch => ch).ToArray()))
            )
            .Select(sorted => string.Join('#', sorted)).ToList();

        return sortedRows;
    }

    private List<string> TiltSouth(List<string> data)
    {
        var sortedCols = Enumerable.Range(0, data[0].Length)
            .Select(col => new string(data.Select(row => row[col]).ToArray()))
            .Select(colStr => colStr.Split('#')
                .Select(part => new string(part.OrderBy(ch => ch).ToArray()))
             )
            .Select(sorted => string.Join('#', sorted)).ToList();

        return Enumerable.Range(0, sortedCols[0].Length)
            .Select(col => new string(sortedCols.Select(row => row[col])
            .ToArray())).ToList();
    }

    private List<string> TiltEast(List<string> data)
    {
        var sortedRows = data
            .Select(rowStr => rowStr.Split('#')
                .Select(part => new string(part.OrderBy(ch => ch).ToArray()))
            )
            .Select(sorted => string.Join('#', sorted)).ToList();

        return sortedRows;
    }
    private int RockConfigTotalLoad(List<string> sortedCols)
    {
        int totalSum = 0;
        for (int r = 0; r < sortedCols.Count; r++)
        {
            int rowSum = 0;
            for (int c = 0; c < sortedCols[r].Length; c++)
            {
                rowSum += sortedCols[r][c] == 'O' ? sortedCols.Count - r : 0;
            }
            totalSum += rowSum;
        }
        return totalSum;
    }

    private string Solve(int part)
    {
        var totalSum = 0;
        if (part == 1)
        {
            var sortedCols = TiltNorth(this.input);
            totalSum = RockConfigTotalLoad(sortedCols);
        }

        if (part == 2)
        {
            List<string> hashes = [];
            List<string> data = this.input;
            Dictionary<int, List<string>> rocksCache = new Dictionary<int, List<string>>();
            int cycleDetected = -1;
            int ctr = 1;

            while (cycleDetected < 0)
            {
                data = TiltEast(TiltSouth(TiltWest(TiltNorth(data))));
                rocksCache[ctr] = data;
                ctr++;
                var hash = InputHash.GetListHash(data);
                hashes.Add(hash);
                cycleDetected = Cycles.FloydsCycleDetection(hashes);
            }
            Cycles.Cycle cycle = Cycles.FloydsCycleStartLength(hashes, cycleDetected);
            int finalIdx = cycle.start + (1000000000 - cycle.start) % cycle.length;
            totalSum = RockConfigTotalLoad(rocksCache[finalIdx]);
        }
        return totalSum.ToString();
    }

    string Day.Part1() => Solve(1);
    string Day.Part2() => Solve(2);

}