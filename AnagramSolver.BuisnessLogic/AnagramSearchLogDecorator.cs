using AnagramSolver.Contracts;

namespace AnagramSolver.BusinessLogic
{
    public class AnagramSearchLogDecorator : IGetAnagrams
    {
        private readonly IGetAnagrams _anagrams;

        public AnagramSearchLogDecorator(IGetAnagrams anagrams) => _anagrams = anagrams;

        public async Task<IEnumerable<Anagram>> GetAnagramsAsync(
            string input,
            int maxAnagramsToShow,
            int minWordLength,
            Func<string, bool> filter,
            CancellationToken ct = default)
        {
            Console.WriteLine($"---> Anagram search started for: {input}");

            var result = await _anagrams.GetAnagramsAsync(input, maxAnagramsToShow, minWordLength, filter, ct);

            Console.WriteLine($"---> Anagram search ended. Found {result.Count()} results");

            return result;
        }
    }
}
