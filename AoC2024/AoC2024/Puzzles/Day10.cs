using System.Runtime.CompilerServices;
using static AoC2024.InputSecrets;
namespace AoC2024.Puzzles
{
    internal class Day10 : IPuzzle
    {
        public int PuzzleID => 10;

        static char[][] input;
        static int rows = 0;
        static int cols = 0;

        public string FindAnswer(byte part)
        {
            input = DAY10_INPUT.Split("\r\n")
                .Select(row => row.ToCharArray())
                .ToArray();
            rows = input.Length;
            cols = input[0].Length;

            var zeros = input
                .SelectMany((row, rowIndex) => row
                    .Select((col, colIndex) => new { rowIndex, colIndex, value = col }))
                .Where(x => x.value == '0')
                .Select(x => (x.rowIndex, x.colIndex)).ToList();

            switch (part)
            {
                case 1:
                    return (from z in zeros
                           from path in Search(z, true)
                           select 1).Count().ToString();
                case 2:
                    return (from z in zeros
                            from path in Search(z, false)
                            select 1).Count().ToString();
            }
            return "Unable to find answer!";
        }

        private static readonly IReadOnlyList<(int dx, int dy)> adjacent = [(0, 1), (0, -1), (1, 0), (-1, 0)];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IEnumerable<(int x, int y)> GetAdjacent((int x, int y) op) => adjacent.Select(pt => (op.x + pt.dx, op.y + pt.dy));

        static Queue<List<(int x, int y)>> pathQueue = new();
        static HashSet<(int x, int y)> visitedLocations = new();
        private static IEnumerable<List<(int x, int y)>> Search((int x, int y) start, bool dedupe)
        {
            pathQueue.Clear();
            if (dedupe) visitedLocations.Clear();
            pathQueue.Enqueue(new List<(int x, int y)> { start });
            if (dedupe) visitedLocations.Add(start);

            while (pathQueue.Any())
            {
                List<(int x, int y)> path = pathQueue.Dequeue();
                var currentPos = path.Last();
                char currentValue = input[currentPos.x][currentPos.y];
                if (currentValue == '9') yield return path;
                foreach (var neighbor in GetAdjacent(currentPos))
                {
                    bool visited = dedupe && visitedLocations.Contains(neighbor);
                    if (!visited && neighbor.x >= 0 && neighbor.y >= 0 && neighbor.x < cols && neighbor.y < rows)
                    {
                        char neighborValue = input[neighbor.x][neighbor.y];
                        if (neighborValue == currentValue + 1)
                        {
                            if (dedupe) visitedLocations.Add(neighbor);
                            var newPath = new List<(int x, int y)>(path) { neighbor };
                            pathQueue.Enqueue(newPath);
                        }
                    }
                }
            }
        }
    }
}