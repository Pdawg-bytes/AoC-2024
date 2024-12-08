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
            Dictionary<long, List<int[]>> parsed = DAY7_INPUT.Split("\n").Select(line => line.Split(':'))
                .GroupBy(parts => long.Parse(parts[0].Trim()))
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(parts => parts[1].Trim().Split(' ').Select(int.Parse).ToArray()).ToList()
                );

            switch (part)
            {
                case 1:
                    List<string> permutatedSequences = new();
                    string[] operators = ["+", "*"];

                    return parsed
                        .Select(set =>
                        {
                            foreach (int[] nums in set.Value)
                            {
                                var permutatedSequences = GenerateOperatorPermutations(operators, nums.Length - 1)
                                .Select(perm =>
                                {
                                    var combined = new List<string> { nums[0].ToString() };
                                    for (int i = 1; i < nums.Length; i++)
                                    {
                                        combined.Add(perm[i - 1]);
                                        combined.Add(nums[i].ToString());
                                    }
                                    return string.Join("", combined);
                                }).ToList();

                                return permutatedSequences.Where(exp => EvaluateExpression(exp) == set.Key).Select(exp => set.Key).FirstOrDefault();
                            }
                            return 0;
                        })
                        .Sum(result => result).ToString();
                case 2:
                    break;
            }
            return "Unable to find answer!";
        }

        private List<string[]> GenerateOperatorPermutations(string[] operators, int length)
        {
            var result = new List<string[]>();
            void RecursiveGenerate(int len, string[] current)
            {
                if (len == 0) { result.Add((string[])current.Clone()); return; }

                foreach (var op in operators)
                {
                    current[len - 1] = op;
                    RecursiveGenerate(len - 1, current);
                }
            }
            RecursiveGenerate(length, new string[length]);
            return result;
        }
        private  long EvaluateExpression(string expression)
        {
            var tokens = Regex.Split(expression, @"([+*])").Where(t => !string.IsNullOrWhiteSpace(t)).ToArray();

            long result = long.Parse(tokens[0]);
            for (int i = 1; i < tokens.Length; i += 2)
            {
                string operatorSymbol = tokens[i];
                long number = int.Parse(tokens[i + 1]);

                if (operatorSymbol == "+")
                {
                    result += number;
                }
                else if (operatorSymbol == "*")
                {
                    result *= number;
                }
            }

            return result;
        }
    }
}
