namespace AoC2024
{
    internal interface IPuzzle
    {
        /// <summary>
        /// The day of the puzzle
        /// </summary>
        public int PuzzleID { get; }

        /// <summary>
        /// If the puzzle has multiple parts. Used in <see cref="FindAnswer(byte)"/>.
        /// </summary>
        public bool RequiresMultiPart { get; }

        /// <summary>
        /// The method used to find the answer of the puzzle.
        /// </summary>
        /// <returns>The answer in <see cref="string"/> form.</returns>
        public string FindAnswer(byte part);
    }
}