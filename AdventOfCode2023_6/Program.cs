using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

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
    

Console.WriteLine(numberOfWaysToWin);

partial class Program
{
    [GeneratedRegex("(\\d+)")]
    private static partial Regex NumberRegex();
}