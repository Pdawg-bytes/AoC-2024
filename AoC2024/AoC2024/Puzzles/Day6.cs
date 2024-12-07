using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using static AoC2024.InputSecrets;

namespace AoC2024.Puzzles
{
    internal class Day6 : IPuzzle
    {
        public int PuzzleID => 5;

        static char[][] input;
        static int rows;
        static int cols;

        enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        public string FindAnswer(byte part)
        {
            input = DAY6_INPUT.Split("\r\n")
            .Select(row => row.ToCharArray())
            .ToArray();
            rows = input.Length;
            cols = input[0].Length;

            switch (part)
            {
                case 1:
                    string[] horizontal = input.Select(row => new string(row)).ToArray();
                    string[] vertical = Enumerable.Range(0, input[0].Length).Select(col => new string(Enumerable.Range(0, input.Length).Select(row => input[row][col]).ToArray())).ToArray();

                    int[] startingPos = input
                        .Select((row, rowIndex) => new { row, rowIndex })
                        .Where(x => x.row.Contains('^'))
                        .Select(x => new int[] { x.rowIndex, Array.IndexOf(x.row, '^') })
                        .First();

                    int[] pos = startingPos;
                    int total = 0;
                    Direction dir = Direction.Up;

                    // this code is terrible i dont have any more time for tonight
                    do
                    {
                        if (dir == Direction.Up)
                        {
                            string verticalLine = vertical[pos[1]];
                            int caretIndex = pos[0];
                            int hashIndex = verticalLine.LastIndexOf('#', caretIndex - 1);
                            int distance = caretIndex - hashIndex - 1;
                            total += Math.Abs(distance);
                            pos[0] = pos[0] - distance;
                            dir = Direction.Right;
                            continue;
                        }
                        else if (dir == Direction.Down)
                        {
                            string verticalLine = vertical[pos[1]];
                            int caretIndex = pos[0];
                            int hashIndex = verticalLine.IndexOf('#', caretIndex - 1);
                            int distance = hashIndex - caretIndex - 1;
                            total += Math.Abs(distance);
                            pos[0] = pos[0] + distance;
                            dir = Direction.Left;
                            continue;
                        }
                        else if (dir == Direction.Left)
                        {
                            string horizontalLine = horizontal[pos[0]];
                            int caretIndex = pos[1];
                            int hashIndex = horizontalLine.LastIndexOf('#', caretIndex - 1);
                            int distance = caretIndex - hashIndex - 1;
                            total += Math.Abs(distance);
                            pos[1] = pos[1] - distance;
                            dir = Direction.Up;
                            continue;
                        }
                        else if (dir == Direction.Right)
                        {
                            string horizontalLine = horizontal[pos[0]];
                            int caretIndex = pos[1];
                            int hashIndex = horizontalLine.IndexOf('#', caretIndex + 1);
                            int distance = hashIndex >= 0 ? hashIndex - caretIndex - 1 : horizontalLine.Length - caretIndex - 1;
                            total += Math.Abs(distance);
                            pos[1] = pos[1] + distance;
                            dir = Direction.Down;
                            continue;
                        }
                    } while (pos[0] > 0 && pos[0] < horizontal.Length && pos[1] > 0 && pos[1] < vertical.Length);

                    Console.WriteLine((total-1).ToString());
                    break;
                case 2:
                    break;
            }

            return "Unable to find answer!";
        }
    }
}