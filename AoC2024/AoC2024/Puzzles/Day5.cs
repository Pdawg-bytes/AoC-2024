using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using static AoC2024.InputSecrets;

namespace AoC2024.Puzzles
{
    internal class Day5 : IPuzzle
    {
        public int PuzzleID => 5;

        public string FindAnswer(byte part)
        {
            string[] splitInput = DAY5_INPUT.Split("\r\n");
            int gapIndex = Array.FindIndex(splitInput, s => s == "");

            List<List<int>> rules = splitInput.Take(gapIndex)
                .Select(line => line.Split('|')
                .Select(int.Parse)
                .ToList())
                .ToList();

            List<List<int>> printingOrders = splitInput.Skip(gapIndex + 1)
                .Select(line => line.Split(',')
                .Select(int.Parse)
                .ToList())
                .ToList();

            switch (part)
            {
                case 1:
                    return printingOrders
                        .Where(order => rules.All(rule => MatchesRule(order, rule)))
                        .Sum(order => order[order.Count / 2])
                        .ToString();
                case 2:
                    return printingOrders
                        .Where(order => rules.Any(rule => !MatchesRule(order, rule)))
                        .Select(order =>
                        {
                            List<int> fixedOrder = new List<int>(order);
                            bool changed;
                            do
                            {
                                changed = false;

                                foreach (var rule in rules)
                                {
                                    if (rule.All(fixedOrder.Contains) && !MatchesRule(fixedOrder, rule))
                                    {
                                        int left = rule[0];
                                        int right = rule[1];

                                        int leftIndex = fixedOrder.IndexOf(left);
                                        int rightIndex = fixedOrder.IndexOf(right);

                                        if (leftIndex > rightIndex)
                                        {
                                            fixedOrder[leftIndex] = right;
                                            fixedOrder[rightIndex] = left;
                                            changed = true;
                                        }
                                    }
                                }

                            } while (changed);
                            return fixedOrder[fixedOrder.Count / 2];
                        }).Sum().ToString();
            }
            return "Unable to find answer!";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool MatchesRule(List<int> order, List<int> rule) => rule.All(order.Contains) ? order.IndexOf(rule[0]) < order.IndexOf(rule[1]) : true;
    }
}