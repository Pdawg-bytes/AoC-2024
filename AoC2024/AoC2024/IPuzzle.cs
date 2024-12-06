namespace AoC2024
{
    internal interface IPuzzle
    {
        /// <summary>
        /// The day of the puzzle
        /// </summary>
        public int PuzzleID { get; }

        /// <summary>
        /// The method used to find the answer of the puzzle.
        /// </summary>
        /// <returns>The answer in <see cref="string"/> form.</returns>
        public string FindAnswer(byte part);
    }
}