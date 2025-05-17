using Aoc2023;
using Aoc2023.Days;
using Aoc2023.Input;

public class Day9 : Day
{
    public List<string> Input { get; private set; }

    public Day9(string filepath)
    {
        Input = new InputReader(filepath).ReadLines();
    }

    private string Solve(int part)
    {
        var totalSum = 0;
        foreach (var line in Input)
        {
            AocLinkedList<List<int>> llst = new AocLinkedList<List<int>>();
            var cur = line.Split(' ').Select(ch => int.Parse(ch)).ToList();
            llst.Append(cur);
            var tmp = llst.Head;
            while (tmp != null && !tmp.Data.All(e => e == 0))
            {
                var nxt = tmp.Data.Skip(1).Select((e, idx) => e - tmp.Data[idx]).ToList();
                tmp.Next = new LLNode<List<int>>(nxt);
                tmp = tmp.Next;
            }

            llst.Reverse();
            var diffCur = llst.Head;
            var rowSum = 0;
            var rowDiff = 0;
            while (diffCur != null)
            {
                rowSum += diffCur.Data[^1];
                rowDiff = diffCur.Data[0] - rowDiff;
                diffCur = diffCur.Next;
            }
            totalSum += part == 1 ? rowSum : rowDiff;
        }
        return totalSum.ToString();
    }

    string Day.Part1() => Solve(1);
    string Day.Part2() => Solve(2);
}
