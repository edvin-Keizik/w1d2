using AnagramSolver.Contracts;
using Microsoft.EntityFrameworkCore;
using AnagramSolver.BusinessLogic;

namespace AnagramSolver.MAF.Tools;

public sealed class AnagramTools : IAnagramTools
{
    private readonly IGetAnagrams _anagramService;
    private readonly IWordProcessor _wordProcessor;
    private readonly AnagramDbContext _dbContext;

    public AnagramTools(
        IGetAnagrams anagramService,
        IWordProcessor wordProcessor,
        AnagramDbContext dbContext)
    {
        _anagramService = anagramService ?? throw new ArgumentNullException(nameof(anagramService));
        _wordProcessor = wordProcessor ?? throw new ArgumentNullException(nameof(wordProcessor));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    public async Task<AnagramToolResult> SearchAnagramsAsync(
        string input,
        int maxAnagrams = 2,
        int minWordLength = 2,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return new AnagramToolResult
            {
                Success = false,
                Message = "Input cannot be empty. Please provide letters to search for anagrams.",
                InputWord = input ?? string.Empty
            };
        }

        try
        {
            var normalizedInput = input.Trim().ToLowerInvariant();
            Func<string, bool> noFilter = _ => true;
            
            var results = await _anagramService.GetAnagramsAsync(
                normalizedInput,
                maxAnagrams,
                minWordLength,
                noFilter,
                cancellationToken);

            var anagrams = results.Select(a => a.Word).ToList();

            return new AnagramToolResult
            {
                Success = true,
                Message = anagrams.Count > 0
                    ? $"Found {anagrams.Count} anagram(s) for '{normalizedInput}'"
                    : $"No anagrams found for '{normalizedInput}'",
                InputWord = normalizedInput,
                Anagrams = anagrams
            };
        }
        catch (OperationCanceledException)
        {
            return new AnagramToolResult
            {
                Success = false,
                Message = "The anagram search operation was cancelled.",
                InputWord = input
            };
        }
        catch (Exception ex)
        {
            return new AnagramToolResult
            {
                Success = false,
                Message = $"An error occurred while searching for anagrams: {ex.Message}",
                InputWord = input
            };
        }
    }
    
    public async Task<WordCountResult> GetWordCountAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var dictionary = await _wordProcessor.GetDictionary();
            var count = dictionary.Count;

            return new WordCountResult
            {
                Success = true,
                Message = $"The dictionary contains {count:N0} words.",
                WordCount = count
            };
        }
        catch (OperationCanceledException)
        {
            return new WordCountResult
            {
                Success = false,
                Message = "The word count operation was cancelled.",
                WordCount = 0
            };
        }
        catch (Exception ex)
        {
            return new WordCountResult
            {
                Success = false,
                Message = $"An error occurred while counting words: {ex.Message}",
                WordCount = 0
            };
        }
    }

    public async Task<FilterByLengthResult> FilterByLengthAsync(
        int length,
        int maxResults = 100,
        CancellationToken cancellationToken = default)
    {
        if (length <= 0)
        {
            return new FilterByLengthResult
            {
                Success = false,
                Message = "Word length must be a positive number.",
                FilteredLength = length
            };
        }

        if (maxResults <= 0)
        {
            maxResults = 100;
        }

        try
        {
            var words = await _dbContext.Words
                .AsNoTracking()
                .Where(w => w.Value.Length == length)
                .Take(maxResults)
                .Select(w => w.Value)
                .ToListAsync(cancellationToken);

            var totalCount = await _dbContext.Words
                .AsNoTracking()
                .CountAsync(w => w.Value.Length == length, cancellationToken);

            var message = words.Count > 0
                ? $"Found {totalCount:N0} words with length {length}. Showing first {words.Count}."
                : $"No words found with length {length}.";

            return new FilterByLengthResult
            {
                Success = true,
                Message = message,
                FilteredLength = length,
                Words = words
            };
        }
        catch (OperationCanceledException)
        {
            return new FilterByLengthResult
            {
                Success = false,
                Message = "The filter operation was cancelled.",
                FilteredLength = length
            };
        }
        catch (Exception ex)
        {
            return new FilterByLengthResult
            {
                Success = false,
                Message = $"An error occurred while filtering words: {ex.Message}",
                FilteredLength = length
            };
        }
    }
}
