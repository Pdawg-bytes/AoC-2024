using System.Collections;
using System.Collections.Generic;
using static AoC2024.InputSecrets;

namespace AoC2024.Puzzles
{
    internal class Day5 : IPuzzle
    {
        public int PuzzleID => 4;

        public string FindAnswer(byte part)
        {
            string[] splitInput = DAY5_INPUT.Split("\r\n");
            int gapIndex = Array.FindIndex(splitInput, s => s == "");

            int[][] rules = splitInput.Take(gapIndex)
                .Select(line => line.Split('|')
                .Select(int.Parse)
                .ToArray())
                .ToArray();

            int[][] printingOrders = splitInput.Skip(gapIndex + 1)
                .Select(line => line.Split(',')
                .Select(int.Parse)
                .ToArray())
                .ToArray();

            switch (part)
            {
                case 1:
                    Dictionary<int, List<int>> map = rules
                        .GroupBy(rule => rule[1], rule => rule[0])
                        .ToDictionary(group => group.Key, group => group.ToList());

                    return printingOrders
                    .Sum(set =>
                    {
                        HashSet<int> excluded = new HashSet<int>();
                        foreach (int n in set)
                        {
                            if (excluded.Contains(n)) return 0;
                            if (map.ContainsKey(n)) excluded.UnionWith(map[n]);
                        }
                        return set[set.Length / 2]; // The arrays are all odd lengths!
                    }).ToString();
                case 2:
                    break;
            }
            return "Unable to find answer!";
        }
    }
}