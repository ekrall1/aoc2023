namespace Aoc2023
{
    class StringTreeNode
    {
        public string Name;
        public StringTreeNode? Left;
        public StringTreeNode? Right;

        public StringTreeNode(string value)
        {
            Name = value;
            Left = null;
            Right = null;
        }
    }

}