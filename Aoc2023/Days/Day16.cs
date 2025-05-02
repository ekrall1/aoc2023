using Aoc2023.Days;
using Aoc2023.Input;

public partial class Day16 : Day
{
    private List<string> input;

    public Day16(string filepath)
    {
        this.input = new InputReader(filepath).ReadLines();
    }

    private string Solve(int part)
    {
        return "not implemented yet";
    }

    string Day.Part1() => Solve(1);
    string Day.Part2() => Solve(2);

}