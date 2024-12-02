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
            };

            Console.WriteLine("Welcome to Pdawg's AoC 2024!");
            Console.WriteLine("Enter the puzzle ID to solve (1 - 31): ");

            int puzzleId = int.Parse(Console.ReadLine());
            if (puzzleId <= 0 || puzzleId > 31) throw new ArgumentOutOfRangeException("The puzzle ID cannot be <= 0 or > 31.");
            if (puzzleId > puzzles.Length) throw new NotImplementedException("This puzzle has not been implemented yet.");

            byte part = 0;
            if (puzzles[puzzleId - 1].RequiresMultiPart)
            {
                Console.WriteLine("This is a multi-part puzzle! Please enter the part to solve.");
                part = (byte)int.Parse(Console.ReadLine());
                if (part <= 0) throw new ArgumentOutOfRangeException("The part cannot be 0. Must be >= 1.");
            }

            Console.WriteLine($"Answer to puzzle {puzzleId}: {puzzles[puzzleId - 1].FindAnswer(part)}");
        }
    }
}