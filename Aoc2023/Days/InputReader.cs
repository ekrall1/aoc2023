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

    }
}