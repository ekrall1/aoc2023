using Aoc2023;
using Aoc2023.Days;
using Aoc2023.Input;

public partial class Day17 : Day
{
    public List<string> Input { get; private set; }

    public Day17(string filepath)
    {
        this.Input = new InputReader(filepath).ReadLines();
    }

    private string Solve(int part)
    {
        Grid grid = new Grid([(0, 1), (0, -1), (1, 0), (-1, 0)]);
        grid.Create(this.Input);
        DijkstraGraph dijkstra = new DijkstraGraph();
        DijkstraGraph.NodeState startNode = new DijkstraGraph.NodeState(X: 0, Y: 0, DX: 0, DY: 0, 0);
        return part == 1 ? dijkstra.Dijkstra(startNode, grid, 1, 3).ToString() : dijkstra.Dijkstra(startNode, grid, 4, 10).ToString();
    }

    string Day.Part1() => Solve(1);
    string Day.Part2() => Solve(2);

}