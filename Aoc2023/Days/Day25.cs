using Aoc2023.Days;
using Aoc2023.Input;

public partial class Day25 : Day
{
    public class Node
    {
        public List<Node> Neighbors { get; } = new();
        public bool Used { get; set; } = false;
        public bool Visited { get; set; } = false;
    }

    private readonly List<string> _input;
    private readonly Dictionary<string, Node> _nodeMap = new();
    private Dictionary<string, HashSet<string>> _adjacencyMap = new();

    private int _countGroupA = 1;
    private int _countGroupB = 0;

    public Day25(string filepath)
    {
        _input = new InputReader(filepath).ReadLines();
        ParseInput();
        BuildGraph();
    }

    /// <summary>
    /// Parses the input lines into an adjacency map (undirected).
    /// </summary>
    private void ParseInput()
    {
        _adjacencyMap = new Dictionary<string, HashSet<string>>();

        foreach (var line in _input)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            var parts = line.Split(':');
            var node = parts[0].Trim();
            var neighbors = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (!_adjacencyMap.ContainsKey(node))
                _adjacencyMap[node] = new HashSet<string>();

            foreach (var neighbor in neighbors)
            {
                _adjacencyMap[node].Add(neighbor);

                // Ensure reverse connection for undirected graph
                if (!_adjacencyMap.ContainsKey(neighbor))
                    _adjacencyMap[neighbor] = new HashSet<string>();

                _adjacencyMap[neighbor].Add(node);
            }
        }
    }

    /// <summary>
    /// Builds Node objects from adjacency map and links neighbors.
    /// </summary>
    private void BuildGraph()
    {
        // Create all nodes
        foreach (var name in _adjacencyMap.Keys)
            _nodeMap[name] = new Node();

        // Connect nodes
        foreach (var kvp in _adjacencyMap)
        {
            var node = _nodeMap[kvp.Key];
            foreach (var neighborName in kvp.Value)
                node.Neighbors.Add(_nodeMap[neighborName]);
        }
    }

    private void ResetFlags(bool resetUsed = false, bool resetVisited = false)
    {
        foreach (var node in _nodeMap.Values)
        {
            if (resetUsed) node.Used = false;
            if (resetVisited) node.Visited = false;
        }
    }

    private void MarkUsed(IEnumerable<Node> path)
    {
        foreach (var node in path)
            node.Used = true;
    }

    /// <summary>
    /// Finds a path from root to target using BFS, avoiding Used nodes.
    /// Returns the path or null if none found.
    /// </summary>
    private List<Node>? FindPath(Node target, Node root)
    {
        ResetFlags(resetVisited: true);

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
                {
                    var fullPath = new List<Node>(path) { neighbor };
                    return fullPath;
                }

                if (neighbor.Visited || neighbor.Used)
                    continue;

                neighbor.Visited = true;
                var newPath = new List<Node>(path) { neighbor };
                queue.Enqueue(newPath);
            }
        }

        return null;
    }

    /// <summary>
    /// Classifies the node by checking connections from master node.
    /// </summary>
    private void ClassifyNode(Node target, Node master)
    {
        ResetFlags(resetUsed: true);
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
            _countGroupA++;
        else
            _countGroupB++;
    }

    private string Solve(int part)
    {
        var master = _nodeMap.Values.First();

        foreach (var node in _nodeMap.Values)
        {
            if (node != master)
            {
                ClassifyNode(node, master);
            }
        }
        return (_countGroupA * _countGroupB).ToString();
    }

    string Day.Part1() => Solve(1);
    string Day.Part2() => Solve(2);
}
