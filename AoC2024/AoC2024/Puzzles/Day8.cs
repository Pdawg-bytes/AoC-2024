using System.Runtime.CompilerServices;
using static AoC2024.InputSecrets;
namespace AoC2024.Puzzles
{
    internal class Day8 : IPuzzle
    {
        public int PuzzleID => 8;

        static char[][] grid;
        static int rows = 0;
        static int cols = 0;

        public string FindAnswer(byte part)
        {
            grid = DAY8_INPUT.Split("\r\n")
                .Select(row => row.ToCharArray())
                .ToArray();
            rows = grid.Length;
            cols = grid[0].Length;

            var antennaMap = grid
                .SelectMany((row, y) => row.Select((ch, x) => (ch, x, y)))
                .Where(cell => cell.ch != '.')
                .GroupBy(cell => cell.ch)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(cell => (cell.x, cell.y)).ToList()
                );

            switch (part)
            {
                case 1: return GetAntinodes(ref antennaMap, includeAllCollinear: false).Count.ToString();
                case 2: return GetAntinodes(ref antennaMap, includeAllCollinear: true).Count.ToString();
            }

            return "Unable to find answer!";
        }

        private static HashSet<(int x, int y)> GetAntinodes(ref Dictionary<char, List<(int x, int y)>> antennaMap, bool includeAllCollinear)
        {
            HashSet<(int x, int y)> antinodes = new();
            foreach (var (character, points) in antennaMap)
            {
                if (points.Count >= 2)
                {
                    for (int i = 0; i < points.Count; i++)
                    {
                        for (int j = i + 1; j < points.Count; j++)
                        {
                            var point1 = points[i];
                            var point2 = points[j];
                            if (includeAllCollinear) { antinodes.Add(point1); antinodes.Add(point2); }

                            int rise = point2.y - point1.y;
                            int run = point2.x - point1.x;

                            var extendedPoint1 = (point1.x - run, point1.y - rise);
                            var extendedPoint2 = (point2.x + run, point2.y + rise);

                            if (includeAllCollinear)
                            {
                                while (true)
                                {
                                    bool addedNewAntinode = false;
                                    if (IsWithinBounds(extendedPoint1, rows, cols))
                                    {
                                        antinodes.Add(extendedPoint1);
                                        extendedPoint1 = (extendedPoint1.Item1 - run, extendedPoint1.Item2 - rise);
                                        addedNewAntinode = true;
                                    }
                                    if (IsWithinBounds(extendedPoint2, rows, cols))
                                    {
                                        antinodes.Add(extendedPoint2);
                                        extendedPoint2 = (extendedPoint2.Item1 + run, extendedPoint2.Item2 + rise);
                                        addedNewAntinode = true;
                                    }
                                    if (!addedNewAntinode) break;
                                }
                            }
                            else
                            {
                                if (IsWithinBounds(extendedPoint1, rows, cols)) antinodes.Add(extendedPoint1);
                                if (IsWithinBounds(extendedPoint2, rows, cols)) antinodes.Add(extendedPoint2);
                            }
                        }
                    }
                }
            }
            return antinodes;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IsWithinBounds((int x, int y) point, int rows, int cols)
        {
            return point.x >= 0 && point.x < cols && point.y >= 0 && point.y < rows;
        }
    }
}