using System.Data;
using System.Text.RegularExpressions;
using Aoc2023;
using Aoc2023.Days;
using Aoc2023.Input;

public class Day4 : Day
{

    private string _filepath;
    private List<string> _inputList;
    private Dictionary<int, int> _cardCounts;
    public Day4(string filepath)
    {
        this._filepath = filepath;
        InputReader fileInput = new InputReader(this._filepath);
        this._inputList = fileInput.ReadLines();
        this._cardCounts = new Dictionary<int, int>();
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

    private static int BuildAndSearchTreePart1(List<int> winning, List<int> have)
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

    private static int BuildAndSearchTreePart2(List<int> winning, List<int> have)
    {

        var root = BinarySearchTree.BuildBalancedBST(winning, 0, winning.Count - 1);

        var score = 0;

        have.ForEach(num =>
        {
            if (BinarySearchTree.Search(root, num) != null)
            {
                score += 1;
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
            totalWinning += BuildAndSearchTreePart1(winning, have);
        });
        return totalWinning.ToString();
    }

    string Day.Part2()
    {
        foreach (var (card, idx) in this._inputList.Select((card, idx) => (card, idx)))
        {
            this._cardCounts[idx + 1] = this._cardCounts.GetValueOrDefault(idx + 1, 0) + 1;
            var (winning, have) = ParseCard(card);
            var cardsWon = BuildAndSearchTreePart2(winning, have);
            for (var i = 0; i < cardsWon; i++)
            {
                if (idx + 2 + i <= this._inputList.Count)
                {
                    this._cardCounts[idx + i + 2] = this._cardCounts.GetValueOrDefault(idx + i + 2, 0) + 1 * this._cardCounts[idx + 1];
                }
            }

        }
        return this._cardCounts.Values.Sum().ToString();

    }

}