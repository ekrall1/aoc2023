using System.Data;
using Aoc2023.Days;
using Aoc2023.Input;

public class Day5 : Day
{

    private string _filepath;
    private List<string> _inputList;
    private List<long> _seeds;
    record MapRange(long DestinationStart, long SourceStart, long Range);
    record KeyRange(long RangeStart, long RangeLength);
    private Dictionary<string, List<MapRange>> _maps;
    private List<KeyRange> _seedRanges;
    public Day5(string filepath)
    {
        this._filepath = filepath;
        InputReader fileInput = new InputReader(this._filepath);
        this._inputList = fileInput.ReadToNewLines();
        this._seeds = [];
        this._seedRanges = new List<KeyRange>();
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

    private List<KeyRange> GetOverlappedValue(string label, List<KeyRange> input)
    {
        var outputList = new List<KeyRange>();
        foreach (var inputRange in input)
        {
            var remainingRange = new List<KeyRange> { inputRange };

            foreach (var range in this._maps[label])
            {
                var tmpRemaining = new List<KeyRange>();

                foreach (var remaining in remainingRange)
                {
                    var overlapStart = Math.Max(remaining.RangeStart, range.SourceStart);
                    var overlapEnd = Math.Min(remaining.RangeStart + remaining.RangeLength, range.SourceStart + range.Range);

                    if (overlapStart < overlapEnd)
                    {
                        var sourceDestOffset = range.DestinationStart - range.SourceStart;
                        var nextStart = overlapStart + sourceDestOffset;
                        outputList.Add(new KeyRange(nextStart, overlapEnd - overlapStart));

                        if (remaining.RangeStart < overlapStart)
                        {
                            tmpRemaining.Add(new KeyRange(remaining.RangeStart, overlapStart - remaining.RangeStart));
                        }
                        if (remaining.RangeStart + remaining.RangeLength > overlapEnd)
                        {
                            tmpRemaining.Add(new KeyRange(overlapEnd, remaining.RangeStart + remaining.RangeLength - overlapEnd));
                        }

                    }
                    else
                    {
                        tmpRemaining.Add(remaining);
                    }
                }

                remainingRange = tmpRemaining;
            }

            outputList.AddRange(remainingRange);
        }


        return outputList;
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
        var inputLst = string.Join(",", this._inputList);

        this._inputList.ForEach(input =>
        {
            string[] parts = input.Split(':');
            var label = parts[0];
            var values = parts[1];
            if (label == "seeds")
            {
                string[] seedPairs = values.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < seedPairs.Length; i += 2)
                {
                    this._seedRanges.Add(new KeyRange(long.Parse(seedPairs[i]), long.Parse(seedPairs[i + 1])));
                }
            }
            else if (label != null && values != null)
            {
                this.ParseMap(label, values);
            }
        });

        var soil = GetOverlappedValue("seed-to-soil map", this._seedRanges);
        var fertilizer = GetOverlappedValue("soil-to-fertilizer map", soil);
        var water = GetOverlappedValue("fertilizer-to-water map", fertilizer);
        var light = GetOverlappedValue("water-to-light map", water);
        var temp = GetOverlappedValue("light-to-temperature map", light);
        var humidity = GetOverlappedValue("temperature-to-humidity map", temp);
        var location = GetOverlappedValue("humidity-to-location map", humidity);

        return location.Min(loc => loc.RangeStart).ToString();

    }

}