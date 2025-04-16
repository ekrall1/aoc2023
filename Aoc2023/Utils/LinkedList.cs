namespace Aoc2023
{
    class LLNode<T>
    {
        public T Data;
        public LLNode<T>? Next;
        public LLNode(T value)
        {
            this.Data = value;
            this.Next = null;
        }
    }

    class AocLinkedList<T>
    {
        public LLNode<T>? Head;
        public AocLinkedList()
        {
            this.Head = null;
        }
        public void Append(T value)
        {
            if (this.Head == null)
            {
                this.Head = new LLNode<T>(value);
                return;
            }

            LLNode<T> tmp = this.Head;
            while (tmp.Next != null)
            {
                tmp = tmp.Next;
            }
            tmp.Next = new LLNode<T>(value);
        }
        public void Reverse()
        {
            if (this.Head == null)
            {
                return;
            }
            var cur = this.Head;
            LLNode<T>? prev = null;

            while (cur != null)
            {
                LLNode<T>? nxt = cur.Next;
                cur.Next = prev;
                prev = cur;
                cur = nxt;
            }

            this.Head = prev;
        }
    }

}