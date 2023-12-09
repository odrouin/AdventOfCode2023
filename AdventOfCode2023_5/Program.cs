using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

var seeds = GetNumbers(lines[0]);
var seedTransformations = seeds.ToDictionary(x => x, x => x);
var wasSeedTransformed = seeds.ToDictionary(x => x, x => false);

foreach(var line in lines.Skip(1))
{
    if (GetNumbers(line) is { Count: > 0 } numbers)
    {
        var destinationRangeStart = numbers[0];
        var sourceRangeStart = numbers[1];
        var rangeLength = numbers[2];

        foreach (var seed in seeds.Where(x => !wasSeedTransformed[x]))
        {
            var seedTransformation = seedTransformations[seed];
            if (seedTransformation >= sourceRangeStart && seedTransformation < sourceRangeStart + rangeLength)
            {
                var destination = destinationRangeStart + (seedTransformation - sourceRangeStart);
                seedTransformations[seed] = destination;
                wasSeedTransformed[seed] = true;
            }
        }
    }
    else if (line.Length == 0)
    {
        foreach (var seed in seeds)
        {
            wasSeedTransformed[seed] = false;
        }
    }
}

var seedLocations = seeds.Select(x => seedTransformations[x]).ToList();

Console.WriteLine(seedLocations.Min());

List<long> GetNumbers(string line)
{
    return NumberRegex().Matches(line).Select(x => long.Parse(x.Value)).ToList();
}

partial class Program
{
    [GeneratedRegex("(\\d+)")]
    private static partial Regex NumberRegex();
}