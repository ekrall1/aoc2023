using Aoc2023.Days;
using Aoc2023.Input;
using Aoc2023.Utils;

public class Day1 : Day
{
    public string FilePath { get; private set; }
    public List<string> InputList { get; private set; }
    public Dictionary<string, char> DigitStrings { get; private set; }

    public Day1(string filepath)
    {
        this.FilePath = filepath;
        InputReader fileInput = new InputReader(this.FilePath);
        this.InputList = fileInput.ReadLines();
        this.DigitStrings = new Dictionary<string, char> {
            {"zero", '0'},
            {"one", '1'},
            {"two", '2'},
            {"three", '3'},
            {"four", '4'},
            {"five", '5'},
            {"six", '6'},
            {"seven", '7'},
            {"eight", '8'},
            {"nine", '9'},
        };
    }

    private char FirstDigitChar(char[] line)
    {
        foreach (char ch in line)
        {
            if (char.IsDigit(ch))
            {
                return ch;
            }
        }
        return '0';
    }

    private int FirstAndLastToInt(string line)
    {
        char[] charArray = line.ToCharArray();
        char firstChar = FirstDigitChar(charArray);
        Array.Reverse(charArray);
        char lastChar = FirstDigitChar(charArray);
        return int.Parse(firstChar.ToString() + lastChar.ToString());
    }

    private int IterInputListP1(List<string> lines)
    {
        int finalSum = 0;
        foreach (string line in lines)
        {
            finalSum += FirstAndLastToInt(line.ToLower());
        }
        return finalSum;
    }

    private Trie CreateTrie()
    {
        Trie trie = new Trie();
        foreach (string number in this.DigitStrings.Keys)
        {
            trie.Insert(number);
        }
        return trie;
    }

    private string LookupValidNumberInSubstr(Trie trie, string substring)
    {
        return trie.StartsWithWord(substring);
    }

    private char GetDigitCharForNumber(string numberName)
    {
        if (this.DigitStrings.TryGetValue(numberName, out char intChar))
        {
            return intChar;
        }
        throw new InvalidDataException($"number name {numberName} does not exist in input dictionary");
    }

    private char IterateWordForMatch(Trie trie, string word)
    {
        for (int i = 0; i <= word.Length - 1; i++)
        {
            if (char.IsDigit(word[i]))
            {
                return word[i];
            }
            string substr = word.Substring(i);
            string valid = LookupValidNumberInSubstr(trie, substr);
            if (valid.Length > 0)
            {
                if (this.DigitStrings.TryGetValue(valid, out char intChar))
                {
                    return intChar;
                }
            }
        }
        throw new InvalidDataException("there is no digit in the string");
    }

    private char IterateWordForMatchReverse(Trie trie, string word)
    {
        for (int i = word.Length - 1; i >= 0; i--)
        {
            if (char.IsDigit(word[i]))
            {
                return word[i];
            }
            string substr = word.Substring(i);
            string valid = LookupValidNumberInSubstr(trie, substr);
            if (valid.Length > 0)
            {
                if (this.DigitStrings.TryGetValue(valid, out char intChar))
                {
                    return intChar;
                }
            }
        }
        throw new InvalidDataException("there is no digit in the string");
    }

    private int FirstAndLastToIntP2(Trie trie, string line)
    {
        char firstChar = IterateWordForMatch(trie, line);
        char lastChar = IterateWordForMatchReverse(trie, line);
        return int.Parse(firstChar.ToString() + lastChar.ToString());
    }

    private int IterInputListP2(List<string> lines)
    {
        Trie trie = CreateTrie();
        int finalSum = 0;
        foreach (string line in lines)
        {
            finalSum += FirstAndLastToIntP2(trie, line.ToLower());
        }
        return finalSum;
    }

    private string SolvePart1(List<string> lines)
    {
        return IterInputListP1(lines).ToString();
    }

    private string SolvePart2(List<string> lines)
    {
        return IterInputListP2(lines).ToString();
    }

    string Day.Part1()
    {
        return SolvePart1(this.InputList);
    }

    string Day.Part2()
    {
        return SolvePart2(this.InputList);
    }
}
