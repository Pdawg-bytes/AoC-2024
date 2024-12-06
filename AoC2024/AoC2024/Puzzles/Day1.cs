using static AoC2024.InputSecrets;

namespace AoC2024.Puzzles
{
    internal class Day1 : IPuzzle
    {
        public int PuzzleID => 1;

        public string FindAnswer(byte part)
        {
            // Input parsing (part-agnostic)
            string[] splitPairs = DAY1_INPUT.Split("\r\n");
            var pairs = new List<(int left, int right)>();

            foreach (string pair in splitPairs)
            {
                string[] s = pair.Split("   ");
                pairs.Add((int.Parse(s[0]), int.Parse(s[1])));
            }

            switch (part)
            {
                case 1:
                    {
                        int[] leftHalf = pairs.Select(p => p.left).OrderBy(x => x).ToArray();
                        int[] rightHalf = pairs.Select(p => p.right).OrderBy(x => x).ToArray();
                        int sum = 0;
                        for (int i = 0; i < leftHalf.Length; i++)
                        {
                            sum += Math.Abs(leftHalf[i] - rightHalf[i]);
                        }

                        return sum.ToString();
                    }
                case 2:
                    {
                        int[] leftHalf = pairs.Select(p => p.left).ToArray();
                        int[] rightHalf = pairs.Select(p => p.right).ToArray();
                        int totalScore = 0;
                        for (int i = 0; i < leftHalf.Length; i++)
                        {
                            totalScore += leftHalf[i] * rightHalf.AsSpan().Count(leftHalf[i]);
                        }
                        return totalScore.ToString();
                    }
            }
            return "Unable to find answer!";
        }

    }
}