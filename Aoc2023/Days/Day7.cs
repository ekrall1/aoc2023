using System.Diagnostics;
using System.Text.RegularExpressions;
using Aoc2023.Days;
using Aoc2023.Input;

public class Day7 : Day
{
    private readonly IEnumerable<Hand> handsPart1;
    private readonly Dictionary<char, int> cardOrder;
    private Dictionary<string, HandValue> handValues;

    public Day7(string filepath)
    {
        cardOrder = new Dictionary<char, int> {
            { 'A', 15 },
            { 'K', 14 },
            { 'Q', 13 },
            { 'J', 12 },
            { 'T', 11 },
            { '9', 10 },
            { '8', 9 },
            { '7', 8 },
            { '6', 7 },
            { '5', 6 },
            { '4', 5 },
            { '3', 4 },
            { '2', 3 },
        };
        var input = new InputReader(filepath).ReadLines();
        handsPart1 = ParseHands(input);
        handValues = new Dictionary<string, HandValue>();
    }

    private record Hand(string Cards, int Wager);
    private record HandValue(int Score, long HighCard, int Wager);

    private static IEnumerable<Hand> ParseHands(List<string> input)
    {
        if (input == null)
        {
            return [];
        }
        var parts = input
            .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            ).Select(parts => new Hand(parts[0], int.Parse(parts[1])));

        return parts;
    }

    private HandValue ScoreHand(Hand hand)
    {
        var cardArray = hand.Cards.ToCharArray();
        if (cardArray.Length != 5)
        {
            throw new Exception($"{hand} is an invalid hand");
        }

        Dictionary<char, int> handCounter = new Dictionary<char, int>();

        var highCardScore = 1;

        foreach (var card in cardArray)
        {
            handCounter[card] = handCounter.GetValueOrDefault(card, 0) + 1;
            highCardScore = highCardScore * 20 + cardOrder[card];
        }

        var handScore = 0;
        var handLength = handCounter.Keys.Count();

        switch (handLength)
        {
            case 1:  // five of a kind
                handScore = 7;
                break;
            case 2:
                if (handCounter.ContainsValue(4))
                { // four of a kind
                    handScore = 6;
                }
                else
                {
                    handScore = 5; // full house
                }
                break;
            case 3:
                if (handCounter.ContainsValue(3))
                { // three of a kind
                    handScore = 4;
                }
                else
                {
                    handScore = 3; // two pair
                }
                break;
            case 4:
                handScore = 2; // one pair
                break;
            case 5:
                handScore = 1;
                break;
        }
        return new HandValue(handScore, highCardScore, hand.Wager);
    }

    private string Solve(IEnumerable<Hand> hands)
    {
        foreach (Hand hand in hands)
        {
            if (!handValues.ContainsKey(hand.Cards))
            {
                handValues[hand.Cards] = ScoreHand(hand);
            };

        }

        return handValues
            .OrderBy(kv => kv.Value.Score)
            .ThenBy(kv => kv.Value.HighCard)
            .Select((kv, idx) => (idx + 1) * kv.Value.Wager)
            .Sum()
            .ToString();

    }

    string Day.Part1() => Solve(handsPart1);
    string Day.Part2() => Solve(handsPart1);

}