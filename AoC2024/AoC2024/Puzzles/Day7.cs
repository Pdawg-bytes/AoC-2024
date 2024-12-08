using System.Data;
using static AoC2024.InputSecrets;

namespace AoC2024.Puzzles
{
    internal class Day7 : IPuzzle
    {
        public int PuzzleID => 7;

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
            ulong result = 0;
            foreach (var set in parsed)
            {
                foreach (var nums in set.Value)
                {
                    bool found = false;
                    GenerateOperatorPermutations(operators, nums.Length - 1, perm => { if (EvaluateExpression(nums, perm) == set.Key) found = true; });
                    if (found) result += set.Key;
                }
            }
            return result;
        }

        private void GenerateOperatorPermutations(string[] operators, int length, Action<string[]> resultCallback)
        {
            string[] current = new string[length];
            void RecursiveGenerate(int len)
            {
                if (len == 0) { resultCallback(current); return; }
                for (int i = 0; i < operators.Length; i++) { current[len - 1] = operators[i]; RecursiveGenerate(len - 1); }
            }
            RecursiveGenerate(length);
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