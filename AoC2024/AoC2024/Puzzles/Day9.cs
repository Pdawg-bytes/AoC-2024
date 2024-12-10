using static AoC2024.InputSecrets;
namespace AoC2024.Puzzles
{
    internal class Day9 : IPuzzle
    {
        public int PuzzleID => 9;

        public string FindAnswer(byte part)
        {
            List<char> diskMap = DAY9_TEST
                .SelectMany((c, index) =>
                    index % 2 == 0
                        ? Enumerable.Repeat((char)('0' + index / 2), c - '0')
                        : Enumerable.Repeat('.', c - '0')).ToList();

            switch (part)
            {
                case 1:
                    {
                        ulong checksum = 0;
                        diskMap.Reverse();
                        for (int i = 0; i < diskMap.Count; i++)
                        {
                            char current = diskMap[0];
                            int rightmost = diskMap.LastIndexOf('.');
                            if (rightmost < 0) break;
                            diskMap[rightmost] = current;
                            diskMap.RemoveAt(0);
                        }
                        diskMap.Reverse();
                        for (int i = 0; i < diskMap.Count; i++)
                        {
                            checksum += (ulong)i * (ulong)(diskMap[i] - '0');
                        }
                        return checksum.ToString();
                    }
                case 2:
                    break;
            }
            return "Unable to find answer!";
        }
    }
}