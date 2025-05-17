namespace Aoc2023
{
    public class Cycles
    {
        // Added properties with getters and setters
        public required List<string> Data { get; set; }

        public Cycles() { }
        public record Cycle(int Start, int Length);

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
            // starting with the hare value from phase 1

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