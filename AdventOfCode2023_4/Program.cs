using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

Console.WriteLine("Part 1");
Console.WriteLine("================================");
Console.WriteLine($"Total: {Part1()}");
Console.WriteLine("\nPart 2");
Console.WriteLine("================================");
Console.WriteLine($"Total: {Part2()}");

int Part1()
{
    var points = 0;
    foreach (var line in lines)
    {
        var cardMatches = LineRegex().Match(line);
        var cardLine = cardMatches.Groups[3].Value;
    
        var splittedCardLine = cardLine.Split("|");
        var winningNumbersLine = splittedCardLine[0];
        var pickedNumbersLine = splittedCardLine[1];
    
        var winningNumbers = NumberRegex().Matches(winningNumbersLine).Select(x => int.Parse(x.Value)).ToList();
        var pickedNumbers = NumberRegex().Matches(pickedNumbersLine).Select(x => int.Parse(x.Value)).ToList();

        var cardPoints = 0;
        foreach (var pickedNumber in pickedNumbers.Where(pickedNumber => winningNumbers.Contains(pickedNumber)))
        {
            if (cardPoints == 0)
            {
                cardPoints = 1;
            }
            else
            {
                cardPoints *= 2;
            }
        }
    
        points += cardPoints;
    }

    return points;
}

int Part2()
{
    var cardLines = lines.Select(x => new CardLine { Line = x, Count = 1 }).ToArray();
    for (var cardIndex = 0 ; cardIndex < cardLines.Length ; ++cardIndex)
    {
        var cardLine = cardLines[cardIndex];
        
        var cardMatches = LineRegex().Match(cardLine.Line);
        var lineString = cardMatches.Groups[3].Value;
    
        var splittedLineString = lineString.Split("|");
        var winningNumbersLine = splittedLineString[0];
        var pickedNumbersLine = splittedLineString[1];
    
        var winningNumbers = NumberRegex().Matches(winningNumbersLine).Select(x => int.Parse(x.Value)).ToList();
        var pickedNumbers = NumberRegex().Matches(pickedNumbersLine).Select(x => int.Parse(x.Value)).ToList();

        var matchingNumbers = pickedNumbers.Count(x => winningNumbers.Contains(x));
        for (var y = 1 ; y <= matchingNumbers ; ++y)
        {
            cardLines[cardIndex + y].Count += cardLine.Count;
        }
    }
    
    return cardLines.Sum(x => x.Count);
}

class CardLine
{
    public int Count { get; set; }
    public string Line { get; set; }
}

partial class Program
{
    [GeneratedRegex("^Card(.*)(\\d+): (.*)")]
    private static partial Regex LineRegex();
    
    [GeneratedRegex("[0-9]+")]
    private static partial Regex NumberRegex();
}