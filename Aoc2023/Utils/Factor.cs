using System.Numerics;

namespace Aoc2023
{
    public static class Factor
    {
        public static T Gcf<T>(params T[] values) where T : INumber<T>
        {
            if (values == null || values.Length == 0)
                throw new ArgumentException("At least one value is required.");

            T result = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                T y = values[i];
                while (y != T.Zero)
                {
                    T tmp = y;
                    y = result % y;
                    result = tmp;
                }
            }
            return result;
        }

        public static T Lcm<T>(params T[] values) where T : INumber<T>
        {
            if (values == null || values.Length == 0)
                throw new ArgumentException("At least one value is required.");

            T result = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                result = result / Gcf(result, values[i]) * values[i];
            }
            return result;
        }
    }
}