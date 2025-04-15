using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Aoc2023;
using Aoc2023.Days;
using Aoc2023.Input;
using Aoc2023.Utils;

public class Day8 : Day
{
    private readonly string instructions;
    private Dictionary<string, StringTreeNode> graph;
    private string rootName;

    public Day8(string filepath)
    {
        var input = new InputReader(filepath).ReadLines();
        instructions = input[0];
        graph = new Dictionary<string, StringTreeNode>();
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
            if (instructions[charCtr] == 'L' && node?.Left != null)
            {
                node = graph[node.Left.Name];
            }
            ctr += 1;
            charCtr = (charCtr + 1) % instructions.Length;
        }

        return ctr.ToString();
    }

    string Day.Part1() => Solve();
    string Day.Part2() => Solve();

}