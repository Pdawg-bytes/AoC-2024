using System.Runtime.CompilerServices;
using static AoC2024.InputSecrets;
namespace AoC2024.Puzzles
{
    internal class Day11 : IPuzzle
    {
        public int PuzzleID => 11;

        public string FindAnswer(byte part)
        {
            List<long> stones = DAY11_INPUT.Split(' ').Select(long.Parse).ToList();
            switch (part) 
            {
                case 1:
                    return stones.Sum(num => EvolvedStones(25, num)).ToString();
                case 2:
                    return stones.Sum(num => EvolvedStones(75, num)).ToString();
            }
            return "Unable to find answer!";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long SimulateGeneration(long stone, Span<long> results)
        {
            if (stone == 0) { results[0] = 1; return 1; }

            Span<char> digits = stackalloc char[20];
            int len = stone.TryFormat(digits, out var written) ? written : 0;

            if (len % 2 == 0)
            {
                int split = len / 2;
                results[0] = long.Parse(digits.Slice(0, split));
                results[1] = long.Parse(digits.Slice(split, split));
                return 2;
            }
            else { results[0] = stone * 2024; return 1; }
        }


        private readonly Dictionary<(long, long), long> _calculationCache = new();
        private long EvolvedStones(long generations, long stone)
        {
            if (generations == 0) return 1;

            var key = (generations, stone);
            if (_calculationCache.TryGetValue(key, out var cachedResult)) return cachedResult;

            Span<long> descendants = stackalloc long[2];
            long descendantCount = SimulateGeneration(stone, descendants);

            long result = 0;
            for (int i = 0; i < descendantCount; i++) { result += EvolvedStones(generations - 1, descendants[i]); }

            _calculationCache[key] = result;
            return result;
        }
    }
}