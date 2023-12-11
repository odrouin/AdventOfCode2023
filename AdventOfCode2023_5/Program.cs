using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

var seeds = GetSeeds(lines[0]).ToList();
var seedsToTransform = new List<SeedRange>();

for (var lineNumber = 2 ; lineNumber < lines.Length; ++lineNumber)
{
    if (GetNumbers(lines[lineNumber]) is { Count: > 0 } numbers)
    {
        var destinationRangeStart = numbers[0];
        var sourceRangeStart = numbers[1];
        var rangeLength = numbers[2];

        var seedTransformationMin = sourceRangeStart;
        var seedTransformationMax = seedTransformationMin + rangeLength - 1;
        var seedTransformationDifference = destinationRangeStart - sourceRangeStart;
        
        foreach (var seed in seeds)
        {
            if(seed.Min >= seedTransformationMin && seed.Max <= seedTransformationMax)
            {
                seedsToTransform.Add(new SeedRange(seed.Min, seed.Max, seedTransformationDifference));
            }
            else if (seed.Min <= seedTransformationMin && seed.Max >= seedTransformationMax)
            {
                seedsToTransform.Add(new SeedRange(seedTransformationMin, seedTransformationMax, seedTransformationDifference));
            }
            else if (seed.Min >= seedTransformationMin && seed.Min <= seedTransformationMax)
            {
                seedsToTransform.Add(new SeedRange(seed.Min, seedTransformationMax, seedTransformationDifference));
            }
            else if (seed.Max >= seedTransformationMin && seed.Max <= seedTransformationMax)
            {
                seedsToTransform.Add(new SeedRange(seedTransformationMin, seed.Max, seedTransformationDifference));
            }
        }
    }
    
    if (lines[lineNumber].Length == 0 || lineNumber == lines.Length - 1)
    {
        // define all possible new seed range edges
        var possibleRangeEdgesValues = 
            seedsToTransform.Concat(seeds)
            .SelectMany(x => new[] { x.Min, x.Max, x.Min - 1, x.Max - 1, x.Min + 1, x.Max + 1 })
            .Distinct()
            .OrderBy(x => x)
            .ToList();
        
        // define the transformation to apply for all possible edges
        var possibleRangeEdges = new List<RangeEdge>();
        for (var i = 0; i < possibleRangeEdgesValues.Count; i++)
        {
            if (seedsToTransform.FirstOrDefault(x => possibleRangeEdgesValues[i] >= x.Min && possibleRangeEdgesValues[i] <= x.Max) is { } nextSeed)
            {
                possibleRangeEdges.Add(new RangeEdge(possibleRangeEdgesValues[i], nextSeed.Difference));
            }
            else if(seeds.FirstOrDefault(x => possibleRangeEdgesValues[i] >= x.Min && possibleRangeEdgesValues[i] <= x.Max) is { })
            {
                possibleRangeEdges.Add(new RangeEdge(possibleRangeEdgesValues[i], 0));
            }
            else
            {
                possibleRangeEdges.Add(new RangeEdge(possibleRangeEdgesValues[i], null));
            }
        }

        // define the new seeds ranges
        var newSeeds = new List<SeedRange>();
        long? lastDifference = null;
        long? lastValue = null;
        long? begin = null;
        foreach (var filteredEdge in possibleRangeEdges)
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
        
        // 2. Apply the transformation to all seeds
        newSeeds = newSeeds.Select(x => new SeedRange(x.Min + x.Difference, x.Max + x.Difference, 0)).ToList();
        
        seeds = newSeeds;
        seedsToTransform = new List<SeedRange>();
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