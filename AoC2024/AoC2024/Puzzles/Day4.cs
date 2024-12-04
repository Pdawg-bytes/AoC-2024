using System.Numerics;
using System.Text.RegularExpressions;
using static AoC2024.InputSecrets;

namespace AoC2024.Puzzles
{
    internal class Day4 : IPuzzle
    {
        public int PuzzleID => 4;

        public bool RequiresMultiPart => true;

        public string FindAnswer(byte part)
        {
            char[][] charArray = DAY4_INPUT.Split('\n')
                .Select(row => row.ToCharArray())
                .ToArray();

            Vector2[] directions = 
            { 
                new Vector2(-1, 1),  // x - 1; y + 1
                new Vector2(0, 1),   // x;     y + 1
                new Vector2(1, 1),   // x + 1; y + 1
                new Vector2(1, 0),   // x + 1; y
                new Vector2(1, -1),  // x + 1; y - 1
                new Vector2(0, -1),  // x;     y - 1
                new Vector2(-1, -1), // x - 1; y - 1
                new Vector2(-1, 0)   // x - 1; y 
            };

            char[] sequence = { 'X', 'M', 'A', 'S' };

            Vector2 position = new Vector2(0, 0);

            switch (part)
            {
                case 1:
                    break;
                case 2:
                    break;
            }
            return "Unable to find answer!";
        }
    }
}