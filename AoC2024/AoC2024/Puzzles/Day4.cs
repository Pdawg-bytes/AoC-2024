using static AoC2024.InputSecrets;

namespace AoC2024.Puzzles
{
    internal class Day4 : IPuzzle
    {
        public int PuzzleID => 4;

        static char[][] input;

        public string FindAnswer(byte part)
        {
            input = DAY4_INPUT.Split("\r\n")
                .Select(row => row.ToCharArray())
                .ToArray();

            switch (part)
            {
                case 1:
                    string[] targets = { "XMAS", "SAMX" };

                    return ExtractAllLines(input)
                        .Sum(line => targets
                        .Sum(target => Enumerable.Range(0, Math.Max(0, line.Length - target.Length + 1))
                        .Count(i => line.Substring(i, target.Length) == target))).ToString();
                case 2:
                    int height = input.Length;
                    int width = input[0].Length;

                    return (
                        from y in Enumerable.Range(1, height - 2)
                        from x in Enumerable.Range(1, width - 2)
                        where input[y][x] == 'A'
                        where Cross(y, x)
                        select 1)
                        .Count().ToString();
            }
            return "Unable to find answer!";
        }

        char[] sortedCross = { 'M', 'M', 'S', 'S' };
        int[] verticalDirections = { -1, 1 };
        int[] horizontalDirections = { -1, 1 };
        bool Cross(int y, int x)
        {
            return verticalDirections
                .SelectMany(dy => horizontalDirections
                    .Select(dx => input[y + dx][x + dy]))
                .OrderBy(c => c)
                .SequenceEqual(sortedCross)
                && input[y - 1][x - 1] != input[y + 1][x + 1];
        }

        string[] ExtractAllLines(char[][] grid)
        {
            int rows = grid.Length;
            int cols = grid[0].Length;

            return grid.Select(row => new string(row))
                .Concat(Enumerable.Range(0, cols) 
                    .Select(col => new string(Enumerable.Range(0, rows)
                        .Select(row => grid[row][col])
                        .ToArray())))
                .Concat(Enumerable.Range(-rows + 1, rows + cols - 1)
                    .Select(d => new string(Enumerable.Range(0, rows)
                        .Where(i => i - d >= 0 && i - d < cols)
                        .Select(i => grid[i][i - d])
                        .ToArray())))
                .Concat(Enumerable.Range(0, rows + cols - 1)
                    .Select(d => new string(Enumerable.Range(0, rows)
                        .Where(i => d - i >= 0 && d - i < cols)
                        .Select(i => grid[i][d - i])
                        .ToArray())))
                .ToArray();
        }
    }
}