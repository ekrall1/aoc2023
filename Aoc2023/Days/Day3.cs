using Aoc2023.Days;
using Aoc2023.Input;

public class Grid {

    public Dictionary<(int, int), char> gridMap;
    public Grid() {
        this.gridMap = new Dictionary<(int, int), char> {};
    }
}
public class Day3 : Day
{

    private string _filepath;
    private List<string> _inputList;

    public Day3(string filepath)
    {
        this._filepath = filepath;
        InputReader fileInput = new InputReader(this._filepath);
        this._inputList = fileInput.ReadLines();
    }
    string Day.Part1()
    {
        return string.Join(",", this._inputList);
    }

    string Day.Part2()
    {
        return string.Join(",", this._inputList);
    }

}