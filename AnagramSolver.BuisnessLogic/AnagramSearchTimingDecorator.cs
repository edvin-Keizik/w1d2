using AnagramSolver.Contracts;
using System.Diagnostics;

namespace AnagramSolver.BusinessLogic
{
    public class AnagramSearchTimingDecorator : IGetAnagrams
    {
        private readonly IGetAnagrams _anagrams;

        public AnagramSearchTimingDecorator(IGetAnagrams anagrams) => _anagrams = anagrams;

        public async Task<IEnumerable<Anagram>> GetAnagramsAsync(
            string input,
            int maxAnagramsToShow,
            int minWordLength,
            Func<string, bool> filter,
            CancellationToken ct = default)
        {
            var watch = Stopwatch.StartNew();

            var result = await _anagrams.GetAnagramsAsync(input, maxAnagramsToShow, minWordLength, filter, ct);

            watch.Stop();
            Console.WriteLine($"[TIMER] Anagram search took {watch.ElapsedMilliseconds}ms");

            return result;
        }
    }
}
