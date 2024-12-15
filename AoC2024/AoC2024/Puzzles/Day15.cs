using System.Runtime.CompilerServices;
using static AoC2024.InputSecrets;
using Vector = (int x, int y);
namespace AoC2024.Puzzles
{
    internal class Day15 : IPuzzle
    {
        public int PuzzleID => 15;

        static char[][] map;

        public string FindAnswer(byte part)
        {
            string[] split = DAY15_TEST.Split("\r\n\r\n");
            map = split[0].Split("\r\n").Select(row => row.ToCharArray()).ToArray();

            List<Vector> movements = split[1].Select(c => c switch
            {
                '<' => (-1, 0),
                '>' => (1, 0),
                '^' => (0, -1),
                'v' => (0, 1),
            }).ToList();

            (int posX, int posY) startingPos = map
                .Select((row, rowIndex) => new { row, rowIndex })
                .Where(x => x.row.Contains('@'))
                .Select(x => (x.rowIndex, Array.IndexOf(x.row, '@'))).First();

            switch (part)
            {
                case 1:
                    for (int i = 0; i < movements.Count; i++)
                    {
                    }
                    break;
                case 2:
                    break;
            }
            return "Unable to find answer!";
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (bool, Vector) AttemptMovement(Vector pos, Vector dir)
        {
            if (map[pos.y + dir.y][pos.x + dir.x] == '#') return (false, pos);
            return (true, (pos.x + dir.x, pos.y + dir.y));
        }
    }
}