namespace AnagramSolver.Contracts
{
    public interface IAnagramSearchEngine
    {
        bool CanSubstract(string letterBank, string signature, out string leftover);
        void FindAllCombinations(
            string remainingLetters,
            int wordsNeeded,
            List<string> currentPath,
            List<string> candidates,
            Dictionary<string, List<string>> wordGroups,
            List<List<string>> allResults,
            IEnumerable<string> originalWords);
    }
}
