namespace Aoc2023
{
    class Node
    {
        public int Value;
        public Node? Left;
        public Node? Right;

        public Node(int value)
        {
            Value = value;
            Left = null;
            Right = null;
        }
    }

    class BinarySearchTree
    {
        public static Node? BuildBalancedBST(List<int> sorted, int start, int end)
        {
            if (start > end)
            {
                return null;
            }

            int mid = (start + end) / 2;
            Node node = new Node(sorted[mid]);

            node.Left = BuildBalancedBST(sorted, start, mid - 1);
            node.Right = BuildBalancedBST(sorted, mid + 1, end);

            return node;
        }

        public static Node? Search(Node? root, int target)
        {
            if (root == null)
            {
                return null;
            }

            if (root.Value == target)
            {
                return root;
            }
            else if (target < root.Value)
            {
                return Search(root.Left, target);
            }
            return Search(root.Right, target);
        }
    }
}