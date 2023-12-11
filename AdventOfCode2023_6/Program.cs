using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

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

Console.WriteLine(winPossibilities.Aggregate((x, y) => x * y));

partial class Program
{
    [GeneratedRegex("(\\d+)")]
    private static partial Regex NumberRegex();
}