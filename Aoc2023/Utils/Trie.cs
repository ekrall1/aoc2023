namespace Aoc2023.Utils
{
    public class TrieNode
    {
        public Dictionary<char, TrieNode> Children
        {
            get; set;
        }
        public bool EndOfWord
        {
            get; set;
        }

        public TrieNode()
        {
            Children = new Dictionary<char, TrieNode>();
            EndOfWord = false;
        }

    }

    public class Trie
    {
        private TrieNode root;

        public Trie()
        {
            root = new TrieNode();
        }

        public void Insert(string word)
        {
            TrieNode cur = root;

            foreach (char c in word)
            {
                if (!cur.Children.ContainsKey(c))
                {
                    cur.Children[c] = new TrieNode();
                }
                cur = cur.Children[c];
            }
            cur.EndOfWord = true;
        }

        public string StartsWithWord(string prefix)
        {
            TrieNode cur = root;
            string word = "";

            foreach (char c in prefix)
            {
                if (!cur.Children.ContainsKey(c))
                {
                    return word;
                }
                word += c;
                cur = cur.Children[c];
            }

            return word;
        }
    }
}