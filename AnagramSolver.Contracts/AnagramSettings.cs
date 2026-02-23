namespace AnagramSolver.Contracts
{
    public class AnagramSettings
    {
        public int MinWordLength {  get; set; }
        public int MaxAnagramsToShow { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public string DefaultConnection { get; set; } = null!;
    }
}
