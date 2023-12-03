using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

var part1Answer = 0;
var part2Answer = 0;
foreach (var line in lines)
{
    var pattern = @"^Game (\d+):(.*)";
    var gameNumberMatch = Regex.Match(line, pattern);
    var gameId = int.Parse(gameNumberMatch.Groups[1].Value);
    var gameLine = gameNumberMatch.Groups[2].Value;
    
    var maxColors = new Dictionary<string, int>();
    foreach (var pickLines in gameLine.TrimStart().Split("; "))
    {
        foreach (var colorLine in pickLines.Split(", "))
        {
            var splittedColorLine = colorLine.Split(" ");
            var colorCount = int.Parse(splittedColorLine[0]);
            var color = splittedColorLine[1];
            if(!maxColors.ContainsKey(color) || colorCount > maxColors[color])
            {
                maxColors[color] = colorCount;
            }
        }
    }
    
    // part 1
    if(maxColors["red"] <= 12 && maxColors["green"] <= 13 && maxColors["blue"] <= 14)
    {
        part1Answer += gameId;
    }
    
    // part 2
    part2Answer += maxColors["red"] * maxColors["green"] * maxColors["blue"];
}

Console.WriteLine($"Part 1 Total: {part1Answer}");
Console.WriteLine($"Part 2 Total: {part2Answer}");