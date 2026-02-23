using AnagramSolver.BusinessLogic.ChainOfResponsibility;
using AnagramSolver.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AnagramSolver.BusinessLogic
{
    public class WordProcessor : IWordProcessor, IGetAnagrams
    {
        private readonly AnagramDbContext _context;
        private readonly HashSet<string> _signatures = new();
        private readonly IAnagramSearchEngine _searchEngine;
        private readonly FilterPipeline _pipeline;
        private readonly MemoryCache<IEnumerable<Anagram>> _cache = new();

        public WordProcessor(IAnagramSearchEngine searchEngine, FilterPipeline pipeline, AnagramDbContext context)
        {
            _searchEngine = searchEngine;
            _pipeline = pipeline;
            _context = context;

            _signatures = _context.WordGroupsEntity.Select(x => x.Signature).ToHashSet();
        }

        public async Task<List<string>> GetDictionary()
        {
            return await _context.Words.Select(w => w.Value).ToListAsync();
        }

        public async Task<bool> AddWordAsync(string word)
        {
            string cleanedWord = word.Trim().ToLower();
            string signature = GetSignature(cleanedWord);

            if (await _context.Words.AnyAsync(w => w.Value == cleanedWord))
                return false;

            _context.Words.Add(new WordEntity { Value = cleanedWord });

            var existingGroup = await _context.WordGroupsEntity.FindAsync(signature);
            if (existingGroup == null)
            {
                _context.WordGroupsEntity.Add(new WordGroupsEntity
                {
                    Signature = signature,
                    Words = cleanedWord
                });
            }
            else
            {
                existingGroup.Words += $",{cleanedWord}";
            }

            await _context.SaveChangesAsync();

            _signatures.Add(signature);

            return true;
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
                foreach (var signature in _signatures)
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

            var wordMap = await _context.WordGroupsEntity
                .Where(x => candidates.Contains(x.Signature))
                .ToDictionaryAsync(x => x.Signature, x => x.Words.Split(',').ToList(), ct);

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
                        wordMap,
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
