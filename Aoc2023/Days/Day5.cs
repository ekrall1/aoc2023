using System.Data;
using Aoc2023.Days;
using Aoc2023.Input;

public class Day5 : Day
{

    private string _filepath;
    private List<string> _inputList;
    private List<long> _seeds;
    record MapRange(long DestinationStart, long SourceStart, long Range);
    private Dictionary<string, List<MapRange>> _maps;
    public Day5(string filepath)
    {
        this._filepath = filepath;
        InputReader fileInput = new InputReader(this._filepath);
        this._inputList = fileInput.ReadToNewLines();
        this._seeds = [];
        this._maps = new Dictionary<string, List<MapRange>>();
    }

    private void ParseMap(string label, string values)
    {
        this._maps[label] = new List<MapRange>();
        var mapLines = values
            .Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(e => long.Parse(e.Trim()))
                .ToList())
            .ToList();

        foreach (var line in mapLines)
        {
            this._maps[label].Add(new MapRange(line[0], line[1], line[2]));
        }
    }

    private long GetMappedValue(string label, long input)
    {

        foreach (var range in this._maps[label])
        {
            if (input >= range.SourceStart && input < range.SourceStart + range.Range)
            {
                return range.DestinationStart + (input - range.SourceStart);
            }
        }

        return input;
    }


    string Day.Part1()
    {
        var inputLst = string.Join(",", this._inputList);
        var minLocation = long.MaxValue;

        this._inputList.ForEach(input =>
        {
            string[] parts = input.Split(':');
            var label = parts[0];
            var values = parts[1];
            if (label == "seeds")
            {
                this._seeds = values
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => long.Parse(x.Trim()))
                .ToList();
            }
            else if (label != null && values != null)
            {
                this.ParseMap(label, values);
            }
        });

        this._seeds.ForEach(seed =>
        {
            var soil = GetMappedValue("seed-to-soil map", seed);
            var fertilizer = GetMappedValue("soil-to-fertilizer map", soil);
            var water = GetMappedValue("fertilizer-to-water map", fertilizer);
            var light = GetMappedValue("water-to-light map", water);
            var temp = GetMappedValue("light-to-temperature map", light);
            var humidity = GetMappedValue("temperature-to-humidity map", temp);
            var location = GetMappedValue("humidity-to-location map", humidity);

            minLocation = long.Min(minLocation, location);
        });

        return minLocation.ToString();
    }

    string Day.Part2()
    {
        return "not implemented";

    }

}