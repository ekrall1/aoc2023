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