using AnagramSolver.BusinessLogic.ChainOfResponsibility;
using AnagramSolver.Contracts;

namespace AnagramSolver.BusinessLogic
{
    public class WordProcessor : IWordProcessor, IGetAnagrams
    {
        private readonly Dictionary<string, List<string>> _wordGroups = new();
        private readonly IAnagramSearchEngine _searchEngine;
        private readonly FilterPipeline _pipeline;
        private readonly MemoryCache<IEnumerable<Anagram>> _cache = new();

        public WordProcessor(IAnagramSearchEngine searchEngine, FilterPipeline pipeline)
        {
            _searchEngine = searchEngine;
            _pipeline = pipeline;
        }
        public bool AddWord(string word)
        {
            string signature = GetSignature(word.ToLower());
            if (!_wordGroups.ContainsKey(signature))
            {
                _wordGroups[signature] = new List<string>();
            }
            if (_wordGroups[signature].Contains(word))
            {
                return false;
            }

            _wordGroups[signature].Add(word);      
            return true;
        }

        public List<string> GetDictionary()
        {
            List<string> words = _wordGroups.Values.SelectMany(list => list).ToList();
            return words; 
        }

        public void LoadDictionary(List<string> dictionary)
        {
            _wordGroups.Clear();
            foreach(string word in dictionary)
            {
                AddWord(word);
            }
        }

        private string GetSignature(string word)
        {
            char[] wordArray = word.ToCharArray();

            Array.Sort(wordArray);

            return new string(wordArray);
        }
        private async Task<List<string>> GetCandidatesKeysAsync(string letterBank, int minWordLength)
        {
            var context = new FilterContext
            {
                LetterBank = letterBank,
                AllowedChars = new HashSet<char>(letterBank),
                BankLength = letterBank.Length,
                MinWordLength = minWordLength

            };

            return await Task.Run(() =>
            {
                var candidates = new List<string>();
                foreach (var signature in _wordGroups.Keys)
                {
                    if (!_pipeline.Execute(context, signature)) continue;

                    if (_searchEngine.CanSubstract(letterBank, signature, out _))
                    {
                        candidates.Add(signature);
                    }
                }
                return candidates.OrderByDescending(s => s.Length).ToList();
            });
        }


        public async Task<IEnumerable<Anagram>> GetAnagramsAsync(
            string input,
            int maxAnagramsToShow,
            int minWordLength,
            Func<string, bool> filter,
            CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            if(_cache.TryGetCache(input, out var cachedAnagrams))
            {
                return cachedAnagrams;
            }

            string letterBank = GetSignature(input.Replace(" ", "").ToLower());
            var originalWords = input.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var candidates = await GetCandidatesKeysAsync(letterBank, minWordLength);

            var searchTasks = Enumerable.Range(1, maxAnagramsToShow).Select(i =>
            {
                return Task.Run(() =>
                {
                    var resultsForThisCount = new List<List<string>>();
                    _searchEngine.FindAllCombinations(
                        letterBank,
                        i,
                        new List<string>(),
                        candidates,
                        _wordGroups,
                        resultsForThisCount,
                        originalWords);
                    return resultsForThisCount;
                }, ct);
            });

            var allTaskResults = await Task.WhenAll(searchTasks);

            var flatResults = allTaskResults.SelectMany(list => list);

            var processingQuery = flatResults.Count() > 5000
                ? flatResults.AsParallel().AsOrdered()
                : flatResults.AsEnumerable();

            var finalResults = processingQuery
                .Select(combination => string.Join(" ", combination))
                .Where(filter)
                .Select(str => new Anagram { Word = str })
                .ToList();


            _cache.AddCache(input, finalResults);

            return finalResults;
        }
    }
}
