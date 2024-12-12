using System.Runtime.CompilerServices;
using static AoC2024.InputSecrets;
namespace AoC2024.Puzzles
{
    internal class Day12 : IPuzzle
    {
        public int PuzzleID => 12;

        static char[][] grid;
        static int rows = 0;
        static int cols = 0;

        public string FindAnswer(byte part)
        {
            grid = DAY12_INPUT.Split("\r\n")
                .Select(row => row.ToCharArray())
                .ToArray();
            rows = grid.Length;
            cols = grid[0].Length;

            switch (part)
            {
                case 1:
                    return FindAllRegions().Sum(region => region.Count * GetPerimeter(region)).ToString();
                case 2:
                    break;
            }
            return "Unable to find answer!";
        }

        private static readonly IReadOnlyList<(int dx, int dy)> adjacent = [(0, 1), (0, -1), (1, 0), (-1, 0)];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IEnumerable<(int x, int y)> GetAdjacent((int x, int y) op) => adjacent.Select(pt => (op.x + pt.dx, op.y + pt.dy));

        static Queue<(int x, int y)> queue = new();
        static HashSet<(int x, int y)> visitedLocations = new();
        private static IEnumerable<List<(int x, int y)>> FindAllRegions()
        {
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var currentChar = grid[x][y];
                    if (!visitedLocations.Contains((x, y)))
                    {
                        List<(int x, int y)> region = new();
                        queue.Clear();
                        queue.Enqueue((x, y));
                        visitedLocations.Add((x, y));
                        while (queue.Any())
                        {
                            var current = queue.Dequeue();
                            region.Add(current);

                            foreach (var neighbor in GetAdjacent(current))
                            {
                                if (neighbor.x >= 0 && neighbor.y >= 0 && neighbor.x < cols && neighbor.y < rows)
                                {
                                    if (grid[neighbor.x][neighbor.y] == currentChar && !visitedLocations.Contains(neighbor))
                                    {
                                        visitedLocations.Add(neighbor);
                                        queue.Enqueue(neighbor);
                                    }
                                }
                            }
                        }

                        yield return region;
                    }
                }
            }
        }
        private static int GetPerimeter(List<(int x, int y)> region)
        {
            int perimeter = 0;
            foreach (var (x, y) in region)
            {
                foreach (var (dx, dy) in adjacent)
                {
                    int nx = x + dx, ny = y + dy;
                    if (nx < 0 || ny < 0 || nx >= cols || ny >= rows || grid[nx][ny] != grid[x][y]) 
                    { 
                        perimeter++; 
                    }
                }
            }
            return perimeter;
        }
    }
}