using System.Numerics;
using System.Text.RegularExpressions;
using static AoC2024.InputSecrets;
namespace AoC2024.Puzzles
{
    internal class Day13 : IPuzzle
    {
        public int PuzzleID => 13;

        public string FindAnswer(byte part)
        {
            var parsed = new Dictionary<Vector2, double[,]>();
            string[] blocks = DAY13_INPUT.Trim().Split("\r\n\r\n");
            foreach (string block in blocks)
            {
                string[] lines = block.Split('\n');
                var buttonA = ParseButtonLine(lines[0]);
                var buttonB = ParseButtonLine(lines[1]);
                var prize = ParsePrizeLine(lines[2]);
                double[,] matrix = new double[2, 2]
                {
                    { buttonA.X, buttonB.X },
                    { buttonA.Y, buttonB.Y }
                };
                parsed[prize] = matrix;
            }

            switch (part)
            {
                case 1:
                    return Solve(false, ref parsed).ToString();
                case 2:
                    return Solve(true, ref parsed).ToString();
            }
            return "Unable to find answer!";
        }

        const double TOLERANCE = 0.0001;
        static ulong Solve(bool add, ref Dictionary<Vector2, double[,]> parsed)
        {
            double total = 0;
            foreach (var res in parsed)
            {
                ulong[] target = [(ulong)res.Key[0], (ulong)res.Key[1]];
                if (add) target[0] += 10000000000000L; target[1] += 10000000000000L;
                Matrix m = new Matrix(res.Value);
                var coefficients = m.Solve(target);
                ulong coeffX = (ulong)Math.Round(coefficients[0]);
                ulong coeffY = (ulong)Math.Round(coefficients[1]);
                double resultX = (coeffX * res.Value[0, 0]) + (coeffY * res.Value[0, 1]);
                double resultY = (coeffX * res.Value[1, 0]) + (coeffY * res.Value[1, 1]);
                if (Math.Abs(resultX - target[0]) <= TOLERANCE && Math.Abs(resultY - target[1]) <= TOLERANCE)
                    total += Math.Round(coefficients[0] * 3) + Math.Round(coefficients[1]);
            }
            return (ulong)Math.Round(total);
        }

        static Vector2 ParseButtonLine(string line)
        {
            var match = Regex.Match(line, @"X\+(-?\d+), Y\+(-?\d+)");
            return new Vector2(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
        }
        static Vector2 ParsePrizeLine(string line)
        {
            var match = Regex.Match(line, @"X=(-?\d+), Y=(-?\d+)");
            return new Vector2(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
        }
    }

    internal class Matrix
    {
        private readonly double[,] _data;
        public Matrix(int rows, int columns) { _data = new double[rows, columns]; }
        public Matrix(double[,] data) { _data = (double[,])data.Clone(); }

        public int Rows => _data.GetLength(0);
        public int Columns => _data.GetLength(1);

        public double this[int row, int col]
        {
            get => _data[row, col];
            set => _data[row, col] = value;
        }

        public double[] Solve(ulong[] target)
        {
            var augmented = new double[Rows, Columns + 1];
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                    augmented[i, j] = _data[i, j];
                augmented[i, Columns] = target[i];
            }
            for (int i = 0; i < Rows; i++)
            {
                int maxRow = i;
                for (int k = i + 1; k < Rows; k++) 
                    if (Math.Abs(augmented[k, i]) > Math.Abs(augmented[maxRow, i])) maxRow = k;
                for (int k = i; k <= Columns; k++)
                {
                    var tmp = augmented[maxRow, k];
                    augmented[maxRow, k] = augmented[i, k];
                    augmented[i, k] = tmp;
                }
                for (int k = i + 1; k < Rows; k++)
                {
                    double factor = augmented[k, i] / augmented[i, i];
                    for (int j = i; j <= Columns; j++)
                        augmented[k, j] -= factor * augmented[i, j];
                }
            }
            var solution = new double[Rows];
            for (int i = Rows - 1; i >= 0; i--)
            {
                solution[i] = augmented[i, Columns] / augmented[i, i];
                for (int k = i - 1; k >= 0; k--)
                    augmented[k, Columns] -= augmented[k, i] * solution[i];
            }
            return solution;
        }
    }
}