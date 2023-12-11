using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

Console.WriteLine("Part 1");
Console.WriteLine("================================");
Console.WriteLine($"Total: {Part1()}");
Console.WriteLine("\nPart 2");
Console.WriteLine("================================");
Console.WriteLine($"Total: {Part2()}");

long Part1()
{
    var times = NumberRegex().Matches(lines[0]).Select(x => int.Parse(x.Value)).ToArray();
    var distances = NumberRegex().Matches(lines[1]).Select(x => int.Parse(x.Value)).ToArray();

    var winPossibilities = new List<int>();
    for (var raceNumber = 0 ; raceNumber < times.Length ; ++raceNumber)
    {
        var numberOfWaysToWin = 0;
        var time = times[raceNumber];
        var distanceToBeat = distances[raceNumber];

        for (var pressTime = 2; pressTime < time; ++pressTime)
        {
            var releaseTime = time - pressTime;
            var speed = pressTime;
            var distance = speed * releaseTime;
            if (distance > distanceToBeat)
            {
                numberOfWaysToWin++;
            }

            // optimization
            if (numberOfWaysToWin > 0 && distance < distanceToBeat)
            {
                break;
            }
        }
    
        winPossibilities.Add(numberOfWaysToWin);
    }

    return winPossibilities.Aggregate((x, y) => x * y);
}

long Part2()
{
    var time = long.Parse(NumberRegex().Match(lines[0].Replace(" ", "")).Value);
    var distanceToBeat = long.Parse(NumberRegex().Match(lines[1].Replace(" ", "")).Value);

    var numberOfWaysToWin = 0;
    for (var pressTime = 2L; pressTime < time; ++pressTime)
    {
        var releaseTime = time - pressTime;
        var speed = pressTime;
        var distance = speed * releaseTime;
        if (distance > distanceToBeat)
        {
            numberOfWaysToWin++;
        }

        // optimization
        if (numberOfWaysToWin > 0 && distance < distanceToBeat)
        {
            break;
        }
    }

    return numberOfWaysToWin;
}

partial class Program
{
    [GeneratedRegex("(\\d+)")]
    private static partial Regex NumberRegex();
}