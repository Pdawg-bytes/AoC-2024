using static AoC2024.InputSecrets;

namespace AoC2024.Puzzles
{
    internal class Day2 : IPuzzle
    {
        public int PuzzleID => 2;

        bool DiffChecks(int[] r)
        {
            int[] diffs = Enumerable.Range(0, r.Length - 1)
                .Select(j => r[j + 1] - r[j])
                .ToArray();

            bool outOfRange = diffs.Any(x => x < -3 || x > 3);
            bool conformity = diffs.All(x => x > 0) || diffs.All(x => x < 0);

            return !outOfRange && conformity;
        }

        public string FindAnswer(byte part)
        {
            string[] reports = DAY2_INPUT.Split("\r\n");
            List<int[]> vectorizedReports = new List<int[]>(reports.Count());
            for (int i = 0; i < reports.Length; i++)
            {
                vectorizedReports.Add(Array.ConvertAll(reports[i].Split(' '), int.Parse));
            }

            switch (part)
            {
                case 1:
                    {
                        int safeReports = 0;
                        for (int i = vectorizedReports.Count - 1; i >= 0; i--)
                        {
                            int[] report = vectorizedReports[i];
                            if (report.Distinct().Count() < report.Length || !DiffChecks(report)) { vectorizedReports.RemoveAt(i); continue; }

                            safeReports++;
                        }
                        return safeReports.ToString();
                    }
                case 2:
                    {
                        int safeReports = 0;
                        for (int i = vectorizedReports.Count - 1; i >= 0; i--)
                        {
                            int[] report = vectorizedReports[i];

                            // Is our report immediately valid?
                            if (!(report.Distinct().Count() < report.Length) && DiffChecks(report)) { safeReports++; continue; }

                            // If not, let's test it with each value removed.
                            for (int j = 0; j < report.Length; j++)
                            {
                                int[] modifiedReport = report.Where((_, index) => index != j).ToArray();
                                if (!(report.Distinct().Count() < report.Length) && DiffChecks(modifiedReport)) { safeReports++; continue; }
                            }

                            vectorizedReports.RemoveAt(i);
                        }
                        return safeReports.ToString();
                    }
            }
            return "Unable to find answer!";
        }
    }
}