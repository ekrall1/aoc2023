namespace Aoc2023

{
    public class Cycles
    {
        public record Cycle(int start, int length);

        private Cycles() { }

        public static int FloydsCycleDetection(List<string> data)
        {
            // phase 1 of floyds algorithm, detect a cycle
            int tortoise = 0;
            int hare = 0;

            do
            {
                tortoise++;
                hare += 2;

                if (hare >= data.Count || tortoise >= data.Count)
                    return -1;
            }
            while (data[tortoise] != data[hare]);

            return tortoise;
        }

        public static Cycle FloydsCycleStartLength(List<string> data, int tortoise)
        {
            // this is phase 2 of floyds algorithm,
            // needs the tortoise value from phase 1

            int start = 0;

            while (data[tortoise] != data[start])
            {
                tortoise++;
                start++;
            }

            // also find the length
            int cycleLen = 1;
            int idx = start + 1;

            while (data[idx] != data[start])
            {
                idx++;
                cycleLen++;
            }

            return new Cycle(start, cycleLen);
        }
    }
}