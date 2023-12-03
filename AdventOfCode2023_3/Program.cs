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
    var total = 0;
    var lineNumber = 0;
    foreach (var line in lines)
    {
        var numbers = Regex.Matches(line, "[0-9]+");
        foreach (Match number in numbers)
        {
            var begin = number.Index;
            var end = number.Index + number.Length - 1;
    
            if (begin > 0)
            {
                if (Regex.IsMatch($"{line[begin - 1]}", @"[^\d.]"))
                {
                    total += int.Parse(number.Value);
                    Console.WriteLine(number);
                    continue;
                }
            }
            
            if (end < line.Length - 1)
            {
                if (Regex.IsMatch($"{line[end + 1]}", @"[^\d.]"))
                {
                    total += int.Parse(number.Value);
                    Console.WriteLine(number);
                    continue;
                }
            }
    
            if (lineNumber > 0)
            {
                var min = Math.Max(0, begin - 1);
                var max = Math.Min(line.Length - 1, end + 1) + 1;
                var previousLine = lines[lineNumber - 1][min..max];
                if (Regex.IsMatch(previousLine, @"[^\d.]"))
                {
                    total += int.Parse(number.Value);
                    Console.WriteLine(number);
                    continue;
                }
            }
            
            if (lineNumber < lines.Length - 1)
            {
                var min = Math.Max(0, begin - 1);
                var max = Math.Min(line.Length - 1, end + 1) + 1;
                var nextLine = lines[lineNumber + 1][min..max];
                if (Regex.IsMatch(nextLine, @"[^\d.]"))
                {
                    total += int.Parse(number.Value);
                    Console.WriteLine(number);
                }
            }
        }
    
        lineNumber++;
    }

    return total;
}

int Part2()
{
    var total = 0;
    var lineNumber = 0;
    foreach (var line in lines)
    {
        var gears = Regex.Matches(line, @"\*");
        foreach (Match gear in gears)
        {
            var adjacentNumbers = new List<int>();
            
            if (gear.Index > 0)
            {
                if (Regex.Match($"{line[..gear.Index]}", "[0-9]+$") is { Success: true } number)
                {
                    adjacentNumbers.Add(int.Parse(number.Value));
                }
            }
            
            if (gear.Index < line.Length - 1)
            {
                if (Regex.Match($"{line[(gear.Index + 1)..]}", "^[0-9]+") is { Success: true } number)
                {
                    adjacentNumbers.Add(int.Parse(number.Value));
                }
            }
    
            if (lineNumber > 0)
            {
                var previousLine = lines[lineNumber - 1];
                var numbers = Regex.Matches(previousLine, "[0-9]+");
                foreach (Match number in numbers)
                {
                    if (gear.Index >= number.Index - 1 && gear.Index <= number.Index + number.Length)
                    {
                        adjacentNumbers.Add(int.Parse(number.Value));
                    }
                }
            }
            
            if (lineNumber < lines.Length - 1)
            {
                var nextLine = lines[lineNumber + 1];
                var numbers = Regex.Matches(nextLine, "[0-9]+");
                foreach (Match number in numbers)
                {
                    if (gear.Index >= number.Index - 1 && gear.Index <= number.Index + number.Length)
                    {
                        adjacentNumbers.Add(int.Parse(number.Value));
                    }
                }
            }
            
            if (adjacentNumbers.Count == 2)
            {
                Console.WriteLine($"Part numbers: {string.Join(", ", adjacentNumbers)}");
                var gearRatio = adjacentNumbers.Aggregate((a, x) => a * x);
                total += gearRatio;
                Console.WriteLine($"Gear ratio: {gearRatio}");
            }
        }
        
        lineNumber++;
    }

    return total;
}