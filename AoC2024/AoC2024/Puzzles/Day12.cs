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
                    return FindAllRegions().Sum(region => region.Count * GetSides(region)).ToString();
            }
            return "Unable to find answer!";
        }

        private static readonly IReadOnlyList<(int dx, int dy)> adjacent = [(0, -1), (1, 0), (0, 1), (-1, 0)];
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
                    if (nx < 0 || ny < 0 || nx >= cols || ny >= rows || grid[nx][ny] != grid[x][y]) perimeter++;
                }
            }
            return perimeter;
        }

        private static int GetSides(List<(int x, int y)> region)
        {
            var expanded = new List<((int x, int y) point, int direction)>();
            foreach (var (x, y) in region)
            {
                if (!region.Contains((x, y - 1))) expanded.Add(((x, y - 1), 1));
                if (!region.Contains((x + 1, y))) expanded.Add(((x + 1, y), 2));
                if (!region.Contains((x, y + 1))) expanded.Add(((x, y + 1), 3));
                if (!region.Contains((x - 1, y))) expanded.Add(((x - 1, y), 0));
            }
            expanded.Sort((a, b) => { return SortKey(a.point, a.direction).CompareTo(SortKey(b.point, b.direction)); });
            int sides = 0;
            if (expanded.Count > 0)
            {
                sides = 1;
                var prev = expanded[0];
                for (int i = 1; i < expanded.Count; i++)
                {
                    var cur = expanded[i];
                    if (!Continued(prev, cur)) sides++;
                    prev = cur;
                }
            }
            return sides;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static (int dir, int coord1, int coord2) SortKey((int x, int y) point, int direction)
        {
            var (x, y) = point;
            return direction switch
            {
                1 or 3 => (direction, y, direction == 1 ? x : -x),
                0 or 2 => (direction, x, direction == 2 ? y : -y),
                _ => throw new InvalidOperationException("Invalid direction. How did we get here?")
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool Continued(((int x, int y) point, int dir) prev, ((int x, int y) point, int dir) cur) => 
            !(cur.dir != prev.dir) && (cur.point == (prev.point.x + adjacent[prev.dir].dx, prev.point.y + adjacent[prev.dir].dy));
    }
}