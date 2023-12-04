using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt").ToDictionary(x => x);

var points = 0;
foreach (var line in lines)
{
    var pattern = @"^Card(.*)(\d+): (.*)";
    var cardMatches = Regex.Match(line, pattern);
    var cardNumber = cardMatches.Groups[2].Value;
    var cardLine = cardMatches.Groups[3].Value;
    
    var splittedCardLine = cardLine.Split("|");
    var winningNumbersLine = splittedCardLine[0];
    var pickedNumbersLine = splittedCardLine[1];
    
    var winningNumbers = Regex.Matches(winningNumbersLine, "[0-9]+").Select(x => int.Parse(x.Value)).ToList();
    var pickedNumbers = Regex.Matches(pickedNumbersLine, "[0-9]+").Select(x => int.Parse(x.Value)).ToList();

    var cardPoints = 0;
    foreach(var pickedNumber in pickedNumbers)
    {
        if (winningNumbers.Contains(pickedNumber))
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
    }
    
    points += cardPoints;
}

Console.WriteLine($"Points: {points}");
