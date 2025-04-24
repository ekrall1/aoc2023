using Aoc2023;
using Aoc2023.Days;
using Aoc2023.Input;

public class Day8 : Day
{
    private readonly string instructions;
    private Dictionary<string, StringTreeNode> graph;
    private IEnumerable<StringTreeNode> part2Nodes;
    private string rootName;

    public Day8(string filepath)
    {
        var input = new InputReader(filepath).ReadLines();
        instructions = input[0];
        graph = new Dictionary<string, StringTreeNode>();
        part2Nodes = [];
        ParseNodes(input[1..]);
        rootName = "AAA";

    }
    private void ParseNodes(IEnumerable<string> input)
    {
        foreach (var (row, idx) in input.Select((row, idx) => (row, idx)))
        {
            if (row == "") continue;
            var parts = row.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var left = parts[2].Trim('(', ',');
            var right = parts[3].Trim(')', ',');

            StringTreeNode parent = GetOrCreateNode(parts[0]);
            parent.Left = GetOrCreateNode(left);
            parent.Right = GetOrCreateNode(right);

            if (parent.Name[^1] == 'A')
            {
                part2Nodes = part2Nodes.Append(parent);
            }
        }
    }

    private StringTreeNode GetOrCreateNode(string name)
    {
        if (!graph.TryGetValue(name, out var node))
        {
            node = new StringTreeNode(name);
            graph[name] = node;
        }
        return node;
    }

    private string Solve()
    {
        var node = graph[rootName];
        int ctr = 0;
        var charCtr = 0;
        while (true)
        {
            if (node?.Name == "ZZZ") break;

            char inst = instructions[charCtr];
            if (inst == 'R' && node?.Right != null)
            {
                node = graph[node.Right.Name];
            }
            if (inst == 'L' && node?.Left != null)
            {
                node = graph[node.Left.Name];
            }
            ctr += 1;
            charCtr = (charCtr + 1) % instructions.Length;
        }

        return ctr.ToString();
    }

    private string SolvePart2()
    {
        IEnumerable<StringTreeNode> nodes = part2Nodes;
        List<long> cycles = [];

        foreach (var node in nodes)
        {
            var tmpNode = node;
            long ctr = 0;
            var charCtr = 0;
            while (true)
            {
                if (tmpNode?.Name[^1] == 'Z')
                {
                    cycles.Add(ctr);
                    break;
                };

                char inst = instructions[charCtr];
                if (inst == 'R' && tmpNode?.Right != null)
                {
                    tmpNode = graph[tmpNode.Right.Name];
                }
                if (inst == 'L' && tmpNode?.Left != null)
                {
                    tmpNode = graph[tmpNode.Left.Name];
                }
                ctr += 1;
                charCtr = (charCtr + 1) % instructions.Length;
            }
        }

        return cycles.Aggregate((long)1, (acc, n) => Lcm(acc, n)).ToString();
    }

    static long Gcf(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    static long Lcm(long a, long b)
    {
        return a / Gcf(a, b) * b;
    }

    string Day.Part1() => Solve();
    string Day.Part2() => SolvePart2();

}