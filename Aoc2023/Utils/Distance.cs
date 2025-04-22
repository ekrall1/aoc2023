namespace Aoc2023
{
    public class Distance
    {

        public Distance()
        {
        }

        public static int Manhattan((int, int) start, (int, int) end)
        {
            return Math.Abs(end.Item1 - start.Item1) + Math.Abs(end.Item2 - start.Item2);
        }

    }
}