namespace Aoc2023.Input
{
    public class InputReader
    {

        private string? _filepath;

        public InputReader(string filepath)
        {
            _filepath = filepath;
        }

        public List<string> ReadLines()
        {
            if (!File.Exists(_filepath))
            {
                throw new FileNotFoundException($"file not found {_filepath}");
            }
            return [.. File.ReadAllLines(_filepath)];
        }

        public List<List<string>> ReadTwoPartLines()
        {
            if (!File.Exists(_filepath))
            {
                throw new FileNotFoundException($"file not found {_filepath}");
            }

            var parts = File.ReadAllText(_filepath)
                            .Split(new[] { "\n\n", "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            var part1 = parts[0].Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(line => line.Trim())
                                .ToList();

            var part2 = parts[1].Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(line => line.Trim())
                                .ToList();

            return [part1, part2];
        }

        public List<string> ReadToNewLines()
        {
            if (!File.Exists(_filepath))
            {
                throw new FileNotFoundException($"file not found {_filepath}");
            }
            var allText = File.ReadAllText(_filepath);

            allText = allText.Replace("\r\n", "\n");

            string[] chunks = allText.Split(new string[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);

            List<string> result = new List<string>();
            foreach (var chunk in chunks)
            {
                result.Add(chunk.Trim());
            }
            return result;
        }

    }
}