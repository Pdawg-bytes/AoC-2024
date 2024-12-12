using AoC2024.Puzzles;

namespace AoC2024
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IPuzzle[] puzzles =
            {
                new Day1(),
                new Day2(),
                new Day3(),
                new Day4(),
                new Day5(),
                new Day6(),
                new Day7(),
                new Day8(),
                new Day9(),
                new Day10(),
                new Day11(),
                new Day12(),
            };

            Console.WriteLine("Welcome to Pdawg's AoC 2024!");
            Console.WriteLine("Enter the puzzle ID to solve (1 - 25): ");

            int puzzleId = int.Parse(Console.ReadLine());
            if (puzzleId <= 0 || puzzleId > 25) throw new ArgumentOutOfRangeException("The puzzle ID cannot be <= 0 or > 25.");
            if (puzzleId > puzzles.Length) throw new NotImplementedException("This puzzle has not been implemented yet.");

            byte part = 0;
            Console.WriteLine("Enter the part to solve.");
            part = (byte)int.Parse(Console.ReadLine());
            if (part <= 0) throw new ArgumentOutOfRangeException("The part cannot be 0. Must be >= 1.");

            Console.WriteLine($"Answer to puzzle {puzzleId}, part {part}: {puzzles[puzzleId - 1].FindAnswer(part)}");
        }
    }
}