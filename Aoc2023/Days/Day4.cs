using Aoc2023;
using Aoc2023.Days;
using Aoc2023.Input;
using Aoc2023.Utils;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

public class Day4 : Day
{

    private string _filepath;
    private List<string> _inputList;
    public Day4(string filepath)
    {
        this._filepath = filepath;
        InputReader fileInput = new InputReader(this._filepath);
        this._inputList = fileInput.ReadLines();
    }

    private static (List<int> winning, List<int> have) ParseCard(string line)
    {

        var startIdx = line.IndexOf(':');
        if (startIdx < 0)
        {
            throw new Exception("Invalid line");
        }

        var numberParts = line
        .Substring(startIdx + 1)
        .Trim()
        .Split('|');

        if (numberParts.Length != 2)
        {
            throw new Exception("Invalid line, no | found");
        }

        List<int> ExtractNumbers(string strNums)
        {
            var hs = new List<int>(
                Regex.Matches(strNums, @"\d+").Select(num => int.Parse(num.Value))
            );
            if (hs.Count == 0)
            {
                throw new Exception("Could not extract numbers from line");
            }
            hs.Sort();
            return hs;
        }

        return (ExtractNumbers(numberParts[0]), ExtractNumbers(numberParts[1]));


    }

    private static int BuildAndSearchTree(List<int> winning, List<int> have)
    {

        var root = BinarySearchTree.BuildBalancedBST(winning, 0, winning.Count - 1);

        var score = 0;

        have.ForEach(num =>
        {
            if (BinarySearchTree.Search(root, num) != null)
            {
                score = score == 0 ? 1 : score * 2;
            }
        });

        return score;

    }

    string Day.Part1()
    {
        var totalWinning = 0;
        this._inputList.ForEach(card =>
        {
            var (winning, have) = ParseCard(card);
            totalWinning += BuildAndSearchTree(winning, have);
        });
        return totalWinning.ToString();
    }

    string Day.Part2()
    {
        return "not implemented yet";
    }

}