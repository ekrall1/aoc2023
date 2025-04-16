namespace Aoc2023
{
    class Factor
    {
        public static long Gcf(long x, long y)
        {
            while (y != 0)
            {
                long tmp = y;
                y = x % y;
                x = tmp;
            }

            return x;
        }

        public static long Lcm(long x, long y)
        {
            return x / Gcf(x, y) * y;
        }
    }
}