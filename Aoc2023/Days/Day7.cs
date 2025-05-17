using Aoc2023.Days;
using Aoc2023.Input;

public class Day7 : Day
{
    public IEnumerable<Hand> PuzzleHands { get; private set; }
    public Dictionary<char, int> CardOrder { get; private set; }
    public Dictionary<string, HandValue> HandValues { get; private set; }

    public Day7(string filepath)
    {
        CardOrder = new Dictionary<char, int> {
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
        PuzzleHands = ParseHands(input);
        HandValues = new Dictionary<string, HandValue>();
    }

    public record Hand(string Cards, int Wager);
    public record HandValue(int Score, long HighCard, int Wager);

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

    private int ScoreUniqueCards(int uniqueCards, Dictionary<char, int> handCounter, Hand hand, int part)
    {
        if (part == 1)
        {
            return uniqueCards switch
            {
                1 => 7,
                2 => handCounter.ContainsValue(4) ? 6 : 5,
                3 => handCounter.ContainsValue(3) ? 4 : 3,
                4 => 2,
                5 => 1,
                _ => throw new InvalidOperationException($"This hand cannot be scored {hand.Cards}")
            };
        }
        else
        {
            var jokers = handCounter.GetValueOrDefault('J');
            if (jokers == 5)
            {
                return 7;  // all jokers five of a kind
            }
            handCounter.Remove('J');
            int maxVal = handCounter.Values.Max();

            return (maxVal + jokers) switch
            {
                5 => 7,
                4 => 6, // four of a kind
                3 => handCounter.Count == 2 ? 5 : 4, // full house or three of a kind // ABBJC
                2 => handCounter.Values.Count(v => v == 2) == 2 || handCounter.Values.Count(v => v == 2) == 1 && jokers == 1 ? 3 : 2, // two pairs or one pair
                1 => 1,
                _ => throw new InvalidOperationException($"This hand cannot be scored {hand.Cards}")
            };

        }
    }

    private HandValue ScoreHand(Hand hand, int part)
    {
        var cardArray = hand.Cards.ToCharArray();
        if (cardArray.Length != 5)
            throw new Exception($"{hand} is an invalid hand");

        Dictionary<char, int> handCounter = new Dictionary<char, int>();
        int highCardScore = 1;
        const int baseValue = 20;

        foreach (var card in cardArray)
        {
            handCounter[card] = handCounter.GetValueOrDefault(card, 0) + 1;
            highCardScore = highCardScore * baseValue + CardOrder[card];
        }

        int uniqueCards = handCounter.Count();

        int handScore = ScoreUniqueCards(uniqueCards, handCounter, hand, part);

        return new HandValue(handScore, highCardScore, hand.Wager);
    }

    private string Solve(IEnumerable<Hand> hands, int part)
    {

        if (part == 2)
        {
            CardOrder['J'] = 1;
        }

        foreach (Hand hand in hands)
        {
            if (!HandValues.ContainsKey(hand.Cards))
            {
                HandValues[hand.Cards] = ScoreHand(hand, part);
            }
            ;

        }

        return HandValues
            .OrderBy(kv => kv.Value.Score)
            .ThenBy(kv => kv.Value.HighCard)
            .Select((kv, idx) => (idx + 1) * kv.Value.Wager)
            .Sum()
            .ToString();

    }

    string Day.Part1() => Solve(PuzzleHands, 1);
    string Day.Part2() => Solve(PuzzleHands, 2);

}
