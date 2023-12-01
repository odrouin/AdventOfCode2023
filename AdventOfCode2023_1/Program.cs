using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

long total = 0;
foreach (var line in lines)
{
    const string pattern = "one|two|three|four|five|six|seven|eight|nine|zero|\\d";
    var firstMatch = Regex.Matches(line, pattern).First();
    var lastMatch = Regex.Matches(line, pattern, RegexOptions.RightToLeft).First();
    var first = ParseValue(firstMatch.Value);
    var last = ParseValue(lastMatch.Value);

    var sum = $"{first}{last}";
    var sumInt = int.Parse(sum);
    total += sumInt;
    
    Console.WriteLine($"{line} - {first} + {last} = {sum} ({sumInt})");
}

Console.WriteLine(total);

int ParseValue(string value)
{
    return value switch
    {
        "one" => 1,
        "two" => 2,
        "three" => 3,
        "four" => 4,
        "five" => 5,
        "six" => 6,
        "seven" => 7,
        "eight" => 8,
        "nine" => 9,
        "zero" => 0,
        _ => int.Parse(value)
    };
}