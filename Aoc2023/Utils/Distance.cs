namespace Aoc2023
{
    public class Distance
    {
        public (int, int) Start { get; set; }
        public (int, int) End { get; set; }

        public Distance((int, int) start, (int, int) end)
        {
            Start = start;
            End = end;
        }

        public int Manhattan()
        {
            return Math.Abs(End.Item1 - Start.Item1) + Math.Abs(End.Item2 - Start.Item2);
        }
    }
}