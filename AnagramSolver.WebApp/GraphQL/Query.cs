using AnagramSolver.Contracts;

namespace AnagramSolver.WebApp.GraphQL
{
    public class Query
    {
        private readonly AnagramSettings _settings;
        public Query(AnagramSettings settings)
        {
            _settings = settings;
        }
        public async Task<IEnumerable<string>> GetAnagrams([Service] IGetAnagrams anagrams, string word)
        {
            var result = await anagrams.GetAnagramsAsync(word, _settings.MaxAnagramsToShow, _settings.MinWordLength, w => w.Length > 3);
            return result.Select(a => a.Word);
        }

        public async Task<List<string>> GetWords([Service] IWordProcessor processor)
        {
            return await processor.GetDictionary();
        }
    }
}
