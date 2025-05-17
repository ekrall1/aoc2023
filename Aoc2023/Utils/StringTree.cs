namespace Aoc2023
{
    public class StringTreeNode
    {
        public string Name { get; set; }
        public StringTreeNode? Left { get; set; }
        public StringTreeNode? Right { get; set; }

        public StringTreeNode(string value)
        {
            Name = value;
            Left = null;
            Right = null;
        }
    }

}