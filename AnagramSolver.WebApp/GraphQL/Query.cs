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
        public async Task<IEnumerable<string>> GetAnagrams([Service] IWordProcessor processor, string word)
        {
            var result = await processor.GetAnagramsAsync(word, _settings.MaxAnagramsToShow, w => w.Length > 3);
            return result.Select(a => a.Word);
        }

        public List<string> GetWords([Service] IWordProcessor processor)
        {
            return processor.GetDictionary();
        }
    }
}
