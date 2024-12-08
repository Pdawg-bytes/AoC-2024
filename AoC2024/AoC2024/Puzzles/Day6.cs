using static AoC2024.InputSecrets;

namespace AoC2024.Puzzles
{
    internal class Day6 : IPuzzle
    {
        public int PuzzleID => 6;

        static char[][] input;
        static int rows;
        static int cols;

        public string FindAnswer(byte part)
        {
            input = DAY6_INPUT.Split("\r\n")
                .Select(row => row.ToCharArray())
                .ToArray();
            rows = input.Length;
            cols = input[0].Length;

            int[] startingPos = input
                .Select((row, rowIndex) => new { row, rowIndex })
                .Where(x => x.row.Contains('^'))
                .Select(x => new[] { x.rowIndex, Array.IndexOf(x.row, '^') }).First();

            int[][] directions = { [-1, 0], [0, 1], [1, 0], [0, -1] };

            switch (part)
            {
                case 1:
                    {
                        int dir = 0;
                        var pos = (x: startingPos[1], y: startingPos[0]);
                        var visited = new HashSet<(int x, int y)> { pos };

                        while (true)
                        {
                            (int x, int y) newPos = (pos.x + directions[dir][1], pos.y + directions[dir][0]);
                            if (newPos.x < 0 || newPos.x >= cols || newPos.y < 0 || newPos.y >= rows) break;
                            if (input[newPos.y][newPos.x] == '#') dir = (dir + 1) % 4;
                            else { pos = newPos; visited.Add(pos); }
                        }
                        return visited.Count.ToString();
                    }
                case 2:
                    {
                        var visited = new HashSet<(int x, int y, int dir)>();
                        return Enumerable.Range(0, rows)
                            .SelectMany(i => Enumerable.Range(0, cols), (i, j) =>
                            {
                                int dir = 0;
                                var pos = (x: startingPos[1], y: startingPos[0]);
                                visited.Clear();

                                while (true)
                                {
                                    if (!visited.Add((pos.x, pos.y, dir))) { return 1; }
                                    var newPos = (x: pos.x + directions[dir][1], y: pos.y + directions[dir][0]);
                                    if (newPos.x < 0 || newPos.x >= cols || newPos.y < 0 || newPos.y >= rows) break;
                                    if (input[newPos.y][newPos.x] == '#' || (newPos.x == j && newPos.y == i)) dir = (dir + 1) % 4;
                                    else pos = newPos;
                                }
                                return 0;
                            }).Sum().ToString();
                    }
            }
            return "Unable to find answer!";
        }
    }
}