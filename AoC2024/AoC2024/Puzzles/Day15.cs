using System.Runtime.CompilerServices;
using static AoC2024.InputSecrets;
using Vector = (int x, int y);
namespace AoC2024.Puzzles
{
    internal class Day15 : IPuzzle
    {
        public int PuzzleID => 15;

        static char[][] map;

        enum CollisionType
        {
            Box,
            Wall,
            None,
        }

        public string FindAnswer(byte part)
        {
            string[] split = DAY15_INPUT.Split("\r\n\r\n");
            map = split[0].Split("\r\n").Select(row => row.ToCharArray()).ToArray();

            List<Vector> movements = split[1]
                .Split("\r\n")
                .SelectMany(line => line.Select(c => c switch
                {
                    '<' => new Vector(-1, 0),
                    '>' => new Vector(1, 0),
                    '^' => new Vector(0, -1),
                    'v' => new Vector(0, 1),
                })).ToList();

            Vector robotPosition = map
                .Select((row, rowIndex) => new { row, rowIndex })
                .Where(x => x.row.Contains('@'))
                .Select(x => (x.rowIndex, Array.IndexOf(x.row, '@'))).First();

            switch (part)
            {
                case 1:
                    Vector firstBox;
                    Vector current;
                    foreach (var move in movements)
                    {
                        map[robotPosition.y][robotPosition.x] = '.';
                        var (collision, pos) = AttemptMovement(robotPosition, move);
                        switch (collision)
                        {
                            case CollisionType.Wall: continue;
                            case CollisionType.None: { robotPosition = pos; break; }
                            case CollisionType.Box:
                                map[robotPosition.y][robotPosition.x] = '.';
                                current = firstBox = pos;
                                while (true)
                                {
                                    current = (current.x + move.x, current.y + move.y);
                                    if (map[current.y][current.x] != 'O') break;
                                }
                                if (map[current.y][current.x] == '#') continue;
                                if (map[current.y][current.x] == '.')
                                {
                                    robotPosition = firstBox;
                                    map[firstBox.y][firstBox.x] = '.';
                                    map[current.y][current.x] = 'O';
                                }
                                break;
                        }
                        map[pos.y][pos.x] = '@';
                    }
                    return map
                        .SelectMany((row, y) => row.Select((cell, x) => (cell, x, y)))
                        .Where(item => item.cell == 'O')
                        .Sum(item => 100 * item.y + item.x).ToString();
                case 2:
                    break;
            }
            return "Unable to find answer!";
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (CollisionType, Vector) AttemptMovement(Vector pos, Vector dir) => map[pos.y + dir.y][pos.x + dir.x] switch
        {
            '#' => (CollisionType.Wall, pos),
            'O' => (CollisionType.Box, (pos.x + dir.x, pos.y + dir.y)),
            '.' => (CollisionType.None, (pos.x + dir.x, pos.y + dir.y))
        };
    }
}