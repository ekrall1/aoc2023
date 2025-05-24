using System.Drawing;
using System.Security;
using Aoc2023;
using Aoc2023.Days;
using Aoc2023.Input;
using Aoc2023.Utils;


public partial class Day25 : Day
{
    public class Node
    {
        public List<Node> Neighbors = new();
        public bool Used = false;
        public bool Visited = false;
    }
    public List<string> Input { get; private set; }
    public List<Node> Nodes = new();
    public Dictionary<string, Node> NodeMap = new();

    public int CountGroupA = 1;
    public int CountGroupB = 0;

    // Undirected graph: node name -> set of connected node names
    public Dictionary<string, HashSet<string>> ParsedMap { get; private set; }
    public Day25(string filepath)
    {
        Input = new InputReader(filepath).ReadLines();
        ParsedMap = new Dictionary<string, HashSet<string>>();

        foreach (var line in Input)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var parts = line.Split(':');
            var node = parts[0].Trim();
            var neighbors = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (!ParsedMap.ContainsKey(node))
                ParsedMap[node] = new HashSet<string>();

            foreach (var neighbor in neighbors)
            {
                ParsedMap[node].Add(neighbor);

                // Add the reverse link for undirected edges
                if (!ParsedMap.ContainsKey(neighbor))
                    ParsedMap[neighbor] = new HashSet<string>();
                ParsedMap[neighbor].Add(node);
            }
        }

        BuildGraph(ParsedMap);
    }

    // Helper struct to represent an undirected edge
    public void BuildGraph(Dictionary<string, HashSet<string>> data)
    {
        // Create nodes
        foreach (var name in data.Keys)
        {
            NodeMap[name] = new Node();
        }

        // Connect nodes
        foreach (var kvp in data)
        {
            var node = NodeMap[kvp.Key];
            foreach (var child in kvp.Value)
            {
                node.Neighbors.Add(NodeMap[child]);
            }
        }
    }

    public void ResetUsed()
    {
        foreach (var node in NodeMap.Values)
            node.Used = false;
    }

    public void ResetVisited()
    {
        foreach (var node in NodeMap.Values)
            node.Visited = false;
    }

    public void MarkUsed(List<Node> path)
    {
        foreach (var node in path)
            node.Used = true;
    }

    public List<Node>? FindPath(Node target, Node root)
    {
        ResetVisited();

        root.Visited = true;
        var queue = new Queue<List<Node>>();
        queue.Enqueue(new List<Node> { root });

        while (queue.Count > 0)
        {
            var path = queue.Dequeue();
            var last = path[^1];

            foreach (var neighbor in last.Neighbors)
            {
                if (neighbor == target)
                    return path;

                if (neighbor.Visited || neighbor.Used)
                    continue;

                neighbor.Visited = true;
                var newPath = new List<Node>(path) { neighbor };
                queue.Enqueue(newPath);
            }
        }

        return null;
    }

    public void ClassifyNode(Node target, Node master)
    {
        ResetUsed();

        master.Used = true;

        int connections = 0;

        foreach (var neighbor in master.Neighbors)
        {
            if (connections > 3)
                break;

            if (neighbor == target)
            {
                connections++;
                continue;
            }

            var path = FindPath(target, neighbor);
            if (path != null)
            {
                connections++;
                MarkUsed(path);
            }
        }

        if (connections > 3)
            CountGroupA++;
        else
            CountGroupB++;
    }


    public string Solve(int part)
    {
        var master = NodeMap.Values.First();

        foreach (var node in NodeMap.Values)
        {
            if (node != master)
            {
                ClassifyNode(node, master);
            }
        }
        return (CountGroupA * CountGroupB).ToString();
    }


    string Day.Part1() => Solve(1);
    string Day.Part2() => Solve(2);
}