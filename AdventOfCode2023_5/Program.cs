using System.Text.RegularExpressions;

var lines = File.ReadAllLines("test.txt");

var seeds = GetSeeds(lines[0]).ToList();
var nextSeeds = new List<SeedRange>();

foreach(var line in lines.Skip(2))
{
    if (GetNumbers(line) is { Count: > 0 } numbers)
    {
        var destinationRangeStart = numbers[0];
        var sourceRangeStart = numbers[1];
        var rangeLength = numbers[2];

        var seedTransformationMin = sourceRangeStart;
        var seedTransformationMax = seedTransformationMin + rangeLength - 1;
        var seedTransformationDifference = destinationRangeStart - sourceRangeStart;
        
        foreach (var seed in seeds)
        {
            if (seed.Min >= seedTransformationMin && seed.Min <= seedTransformationMax && seed.Max > seedTransformationMax)
            {
                nextSeeds.Add(new SeedRange(seed.Min, Math.Min(seedTransformationMax, seed.Max), seedTransformationDifference));
            }
            else if (seed.Min < seedTransformationMin && seed.Max >= seedTransformationMin && seed.Max <= seedTransformationMax)
            {
                nextSeeds.Add(new SeedRange(Math.Max(seed.Min, seedTransformationMin), seed.Max, seedTransformationDifference));
            }
            else if(seed.Min > seedTransformationMin && seed.Max < seedTransformationMax)
            {
                nextSeeds.Add(new SeedRange(seed.Min, seed.Max, seedTransformationDifference));
            }
            else if (seed.Min < seedTransformationMin && seed.Max > seedTransformationMax)
            {
                nextSeeds.Add(new SeedRange(seedTransformationMin, seedTransformationMax, seedTransformationDifference));
            }
        }
    }
    else if (line.Length == 0)
    {
        // 1. Définir les nouveaux ranges 
        var rangeEdges = 
            nextSeeds.Concat(seeds)
            .SelectMany(x => new[] { x.Min, x.Max, x.Min - 1, x.Max - 1, x.Min + 1, x.Max + 1 })
            .Distinct()
            .OrderBy(x => x)
            .ToList();
        
        var filteredEdges = new List<RangeEdge>();
        for (var i = 0; i < rangeEdges.Count; i++)
        {
            if (nextSeeds.FirstOrDefault(x => rangeEdges[i] >= x.Min && rangeEdges[i] <= x.Max) is { } nextSeed)
            {
                filteredEdges.Add(new RangeEdge(rangeEdges[i], nextSeed.Difference));
            }
            else if(seeds.FirstOrDefault(x => rangeEdges[i] >= x.Min && rangeEdges[i] <= x.Max) is { })
            {
                filteredEdges.Add(new RangeEdge(rangeEdges[i], 0));
            }
            else
            {
                filteredEdges.Add(new RangeEdge(rangeEdges[i], null));
            }
        }

        var newSeeds = new List<SeedRange>();
        long? lastDifference = null;
        long? lastValue = null;
        long? begin = null;
        foreach (var filteredEdge in filteredEdges)
        {
            if (lastDifference != filteredEdge.Difference)
            {
                if(begin != null && lastDifference != null)
                {
                    newSeeds.Add(new SeedRange(begin.Value, lastValue!.Value, lastDifference.Value));
                }
                
                begin = filteredEdge.Value;
            }
            
            lastValue = filteredEdge.Value;
            lastDifference = filteredEdge.Difference;
        }
        
        // 2. Apply the differences
        newSeeds = newSeeds.Select(x => new SeedRange(x.Min + x.Difference, x.Max + x.Difference, 0)).ToList();
        
        seeds = newSeeds;
        nextSeeds = new List<SeedRange>();
    }
}

Console.WriteLine(seeds.Min(x => x.Min));

IEnumerable<SeedRange> GetSeeds(string line)
{
    var numbers = GetNumbers(line);
    for (var i = 0; i < numbers.Count; i += 2)
    {
        yield return new SeedRange(numbers[i], numbers[i] + numbers[i + 1] - 1, 0);
    }
}

List<long> GetNumbers(string line)
{
    return NumberRegex().Matches(line).Select(x => long.Parse(x.Value)).ToList();
}

class SeedRange
{
    public SeedRange(long min, long max, long difference)
    {
        this.Min = min;
        this.Max = max;
        this.Difference = difference;
    }

    public long Min { get; set; }

    public long Max { get; set; }

    public long Difference { get; set; }
}

class RangeEdge
{
    public RangeEdge(long value, long? difference)
    {
        this.Value = value;
        this.Difference = difference;
    }

    public long Value { get; set; }

    public long? Difference { get; set; }
}

partial class Program
{
    [GeneratedRegex("(\\d+)")]
    private static partial Regex NumberRegex();
}