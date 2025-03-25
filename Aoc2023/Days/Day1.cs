using Aoc2023.Days;
using Aoc2023.Input;
using Aoc2023.Utils;

public class Day1 : Day
{

    private string _filepath;
    private List<string> _inputList;

    private Dictionary<string, char> _digitStrings;

    public Day1(string filepath)
    {
        this._filepath = filepath;
        InputReader fileInput = new InputReader(this._filepath);
        this._inputList = fileInput.ReadLines();
        this._digitStrings = new Dictionary<string, char> {
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

    private char firstDigitChar(char[] line)
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
    private int firstAndLastToInt(string line)
    {
        char[] charArray = line.ToCharArray();
        char firstChar = firstDigitChar(charArray);
        Array.Reverse(charArray);
        char lastChar = firstDigitChar(charArray);
        return int.Parse(firstChar.ToString() + lastChar.ToString());
    }

    private int iterInputListP1(List<string> lines)
    {
        int finalSum = 0;
        foreach (string line in lines)
        {
            finalSum += firstAndLastToInt(line.ToLower());
        }
        return finalSum;
    }

    private Trie createTrie()
    {
        Trie trie = new Trie();
        foreach (string number in this._digitStrings.Keys)
        {
            trie.Insert(number);
        }
        return trie;
    }

    private string lookupValidNumberInSubstr(Trie trie, string substring)
    {
        return trie.StartsWithWord(substring);
    }

    private char getDigitCharForNumber(string numberName)
    {
        if (this._digitStrings.TryGetValue(numberName, out char intChar))
        {
            return intChar;
        }
        throw new InvalidDataException($"number name {numberName} does not exist in input dictionary");
    }

    private char iterateWordForMatch(Trie trie, string word)
    {
        for (int i = 0; i <= word.Length - 1; i++)
        {
            if (char.IsDigit(word[i]))
            {
                return word[i];
            }
            string substr = word.Substring(i);
            string valid = lookupValidNumberInSubstr(trie, substr);
            if (valid.Length > 0)
            {
                if (this._digitStrings.TryGetValue(valid, out char intChar))
                {
                    return intChar;
                }
            }
        }
        throw new InvalidDataException("there is no digit in the string");
    }

    private char iterateWordForMatchReverse(Trie trie, string word)
    {
        for (int i = word.Length - 1; i >= 0; i--)
        {
            if (char.IsDigit(word[i]))
            {
                return word[i];
            }
            string substr = word.Substring(i);
            string valid = lookupValidNumberInSubstr(trie, substr);
            if (valid.Length > 0)
            {
                if (this._digitStrings.TryGetValue(valid, out char intChar))
                {
                    return intChar;
                }
            }
        }
        throw new InvalidDataException("there is no digit in the string");
    }

    private int firstAndLastToIntP2(Trie trie, string line)
    {
        char firstChar = iterateWordForMatch(trie, line);
        char lastChar = iterateWordForMatchReverse(trie, line);
        return int.Parse(firstChar.ToString() + lastChar.ToString());
    }

    private int iterInputListP2(List<string> lines)
    {
        Trie trie = createTrie();
        int finalSum = 0;
        foreach (string line in lines)
        {
            finalSum += firstAndLastToIntP2(trie, line.ToLower());
        }
        return finalSum;
    }


    private string solvePart1(List<string> lines)
    {
        return iterInputListP1(lines).ToString();
    }

    private string solvePart2(List<string> lines)
    {
        return iterInputListP2(lines).ToString();
    }

    string Day.Part1()
    {
        return solvePart1(this._inputList);
    }

    string Day.Part2()
    {
        return solvePart2(this._inputList);
    }

}