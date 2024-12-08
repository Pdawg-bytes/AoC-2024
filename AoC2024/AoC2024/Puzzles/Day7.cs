using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using static AoC2024.InputSecrets;

namespace AoC2024.Puzzles
{
    internal class Day7 : IPuzzle
    {
        public int PuzzleID => 7;

        static DataTable dataTable = new DataTable();

        public string FindAnswer(byte part)
        {
            Dictionary<ulong, List<ulong[]>> parsed = DAY7_INPUT.Split("\n").Select(line => line.Split(':'))
                .GroupBy(parts => ulong.Parse(parts[0].Trim()))
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(parts => parts[1].Trim().Split(' ').Select(ulong.Parse).ToArray()).ToList()
                );

            switch (part)
            {
                case 1:
                    return GetResult(["+", "*"], ref parsed).ToString();
                case 2:
                    return GetResult(["+", "*", "||"], ref parsed).ToString();
            }
            return "Unable to find answer!";
        }

        private ulong GetResult(string[] operators, ref Dictionary<ulong, List<ulong[]>> parsed)
        {
            return parsed
                .SelectMany(set => set.Value
                    .Where(nums =>
                        GenerateOperatorPermutations(operators, nums.Length - 1)
                            .Any(perm => EvaluateExpression(nums.Select(n => n).ToArray(), perm) == set.Key)
                    )
                    .Select(_ => set.Key)
                )
                .Aggregate(0UL, (acc, key) => acc + key);
        }

        private List<string[]> res = new List<string[]>();
        private List<string[]> GenerateOperatorPermutations(string[] operators, int length)
        {
            res.Clear();
            void RecursiveGenerate(int len, string[] current)
            {
                if (len == 0) { res.Add((string[])current.Clone()); return; }
                foreach (var op in operators) { current[len - 1] = op; RecursiveGenerate(len - 1, current); }
            }
            RecursiveGenerate(length, new string[length]);
            return res;
        }
        private ulong EvaluateExpression(ulong[] numbers, string[] operators)
        {
            ulong result = numbers[0];
            for (int i = 0; i < operators.Length; i++)
            {
                if (operators[i] == "+") result += numbers[i + 1];
                else if (operators[i] == "*") result *= numbers[i + 1];
                else if (operators[i] == "||") result = ulong.Parse($"{result}{numbers[i + 1]}");
            }
            return result;
        }
    }
}