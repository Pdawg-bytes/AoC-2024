using System.Text.RegularExpressions;
using static AoC2024.InputSecrets;

namespace AoC2024.Puzzles
{
    internal class Day3 : IPuzzle
    {
        public int PuzzleID => 3;

        private static readonly Regex part1Regex = new(pattern: @"mul\((\d{1,3}),(\d{1,3})\)", options: RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex part2Regex = new(pattern: @"do\(\)|don't\(\)|mul\((\d{1,3}),(\d{1,3})\)", options: RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public string FindAnswer(byte part)
        {
            switch (part)
            {
                case 1:
                    return part1Regex.Matches(DAY3_INPUT)
                        .Cast<Match>()
                        .Select(match => int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value))
                        .Sum()
                        .ToString();
                case 2:
                    int sum = 0;
                    bool flag = true;
                    foreach (Match match in part2Regex.Matches(DAY3_INPUT))
                    {
                        switch (match.Value)
                        {
                            case "do()": flag = true; break;
                            case "don't()": flag = false; break;
                            default: sum += flag ? int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value) : 0; break;
                        }
                    }
                    return sum.ToString();
            }
            return "Unable to find answer!";
        }
    }
}