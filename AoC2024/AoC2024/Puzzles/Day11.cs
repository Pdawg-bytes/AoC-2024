using System.Runtime.CompilerServices;
using static AoC2024.InputSecrets;
namespace AoC2024.Puzzles
{
    internal class Day11 : IPuzzle
    {
        public int PuzzleID => 11;

        public string FindAnswer(byte part)
        {
            List<ulong> stones = DAY11_INPUT.Split(' ').Select(ulong.Parse).ToList();
            switch (part) 
            {
                case 1:
                case 2:
                    break;
            }
            return "Unable to find answer!";
        }
    }
}