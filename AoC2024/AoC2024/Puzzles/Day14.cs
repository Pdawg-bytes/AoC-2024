using System.Numerics;
using static AoC2024.InputSecrets;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
namespace AoC2024.Puzzles
{
    internal class Day14 : IPuzzle
    {
        public int PuzzleID => 14;

        const int W_MAP = 101;
        const int H_MAP = 103;
        const int MID_X = W_MAP / 2;
        const int MID_Y = H_MAP / 2;
        const int MAP_AREA_BIT = W_MAP * H_MAP * sizeof(int);
        const int MAP_AREA = W_MAP * H_MAP;

        const int ITERATIONS_P1 = 100;
        const int ITERATIONS_P2 = 10402; // thanks sblom!

        const float COMPRESSION_THRESHHOLD = 0.95f;

        Regex robotRegex = new Regex(@"p=(-?\d+),(-?\d+)\s+v=(-?\d+),(-?\d+)", RegexOptions.Compiled);
        class Robot
        {
            public Robot((int x, int y) pos, (int dx, int dy) vel) { p = pos; v = vel; }
            public (int x, int y) p { get; set; }
            public (int dx, int dy) v { get; set; }
        }

        public string FindAnswer(byte part)
        {
            List<Robot> robots = DAY14_INPUT.Split("\r\n")
                .Select(line =>
                {
                    var match = robotRegex.Match(line);
                    return new Robot((int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)), (int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value)));
                }).ToList();

            switch (part)
            {
                case 1:
                    {
                        int[][] grid = new int[H_MAP][];
                        for (int i = 0; i < H_MAP; i++) grid[i] = new int[W_MAP];

                        for (int i = 0; i < ITERATIONS_P1; i++)
                        {
                            foreach (var robot in robots) robot.p = Wrap(Add(robot.p, robot.v));
                        }
                        robots = robots.Where(robot => robot.p.x != W_MAP / 2 && robot.p.y != H_MAP / 2).ToList();
                        foreach (var robot in robots) grid[robot.p.y][robot.p.x]++;

                        var topHalf = grid.Take(MID_Y);
                        var bottomHalf = grid.Skip(MID_Y);

                        var topLeft = topHalf.Sum(row => row.Take(MID_X).Sum());
                        var topRight = topHalf.Sum(row => row.Skip(MID_X).Sum());
                        var bottomLeft = bottomHalf.Sum(row => row.Take(MID_X).Sum());
                        var bottomRight = bottomHalf.Sum(row => row.Skip(MID_X).Sum());

                        return (topLeft * topRight * bottomLeft * bottomRight).ToString();
                    }
                case 2:
                    {
                        (float bestCompressionRatio, int bestIteration) = (float.MaxValue, -1);
                        for (int i = 0; i < ITERATIONS_P2; i++)
                        {
                            foreach (var robot in robots) robot.p = Wrap(Add(robot.p, robot.v));
                            float compressionRatio = (float)GetCompressedSize(ref robots) / MAP_AREA_BIT;
                            if ((1 - compressionRatio) >= COMPRESSION_THRESHHOLD && compressionRatio < bestCompressionRatio)
                            {
                                bestCompressionRatio = compressionRatio;
                                bestIteration = i + 1;
                            }
                        }
                        return bestIteration.ToString();
                    }
            }
            return "Unable to find answer!";
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (int x, int y) Wrap((int x, int y) p) => ((p.x % W_MAP + W_MAP) % W_MAP, (p.y % H_MAP + H_MAP) % H_MAP);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (int x, int y) Add((int x, int y) a, (int dx, int dy) b) => (a.x + b.dx, a.y + b.dy);

        static int[] flattened = new int[MAP_AREA];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int GetCompressedSize(ref List<Robot> robots)
        {
            Array.Clear(flattened, 0, MAP_AREA);
            foreach (var robot in robots) flattened[robot.p.y * W_MAP + robot.p.x]++;

            int compressedSize = 0;
            int current = flattened[0];
            int count = 1;

            Span<int> data = flattened.AsSpan(1, flattened.Length - 1);
            foreach (var value in data)
            {
                if (value == current) count++;
                else
                {
                    compressedSize += 1 + BitOperations.Log2((uint)(count + 1));
                    current = value;
                    count = 1;
                }
            }
            compressedSize += 1 + BitOperations.Log2((uint)(count + 1));
            return compressedSize;
        }
    }
}