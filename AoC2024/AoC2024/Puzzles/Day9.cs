using System.Security.Cryptography;
using static AoC2024.InputSecrets;
namespace AoC2024.Puzzles
{
    internal class Day9 : IPuzzle
    {
        public int PuzzleID => 9;

        public string FindAnswer(byte part)
        {
            switch (part)
            {
                case 1:
                    {
                        List<char> diskMap = DAY9_INPUT
                            .SelectMany((c, index) =>
                                index % 2 == 0 
                                ? Enumerable.Repeat((char)('0' + index / 2), c - '0') 
                                : Enumerable.Repeat('.', c - '0')).ToList();
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
                    {
                        ulong blockPos = 0;
                        ulong fileID = 0;

                        Dictionary<ulong, (ulong pos, ulong len)> dataBlocks = new();
                        List<(ulong pos, ulong len)> freeBlocks = new();
                        for (int i = 0; i < DAY9_INPUT.Length; i++)
                        {
                            if (!ulong.TryParse(DAY9_INPUT[i].ToString(), out ulong current)) continue;
                            if (i % 2 == 0) { dataBlocks[fileID] = (blockPos, current); fileID++; }
                            else { if (current != 0) freeBlocks.Add((blockPos, current)); }
                            blockPos += current;
                        }

                        while (fileID > 0)
                        {
                            fileID--;
                            (ulong filePos, ulong fileLen) = dataBlocks[fileID];
                            for (int i = 0; i < freeBlocks.Count; i++)
                            {
                                var (blankPos, blankLen) = freeBlocks[i];
                                if (blankPos >= filePos) { freeBlocks = freeBlocks.Take(i).ToList(); break; }
                                if (fileLen <= blankLen) 
                                { 
                                    dataBlocks[fileID] = (blankPos, fileLen);
                                    if (fileLen == blankLen) freeBlocks.RemoveAt(i);
                                    else freeBlocks[i] = (blankPos + fileLen, blankLen - fileLen);
                                    break;
                                }
                            }
                        }
                        return dataBlocks.Sum(file =>
                        {
                            ulong fid = file.Key;
                            var (pos, size) = file.Value;
                            return (long)Enumerable.Range((int)pos, (int)size).Aggregate(0UL, (sum, x) => sum + fid * (ulong)x);
                        }).ToString();
                    }
            }
            return "Unable to find answer!";
        }
    }
}