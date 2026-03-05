using AnagramSolver.Contracts.FrequencyAnalysis;
using Microsoft.Extensions.Logging;

namespace AnagramSolver.BusinessLogic.FrequencyAnalysis;

/// <summary>Analyzes word frequency in text with support for stop words and Lithuanian language.</summary>
public class FrequencyAnalysisService : IFrequencyAnalysisService
{
    private readonly IStopWordProvider _stopWordProvider;
    private readonly ILogger<FrequencyAnalysisService> _logger;
    private const int MaxTopWordsCount = 10;
    private const int MaxTextLength = 100_000;

    public FrequencyAnalysisService(
        IStopWordProvider stopWordProvider,
        ILogger<FrequencyAnalysisService> logger)
    {
        _stopWordProvider = stopWordProvider;
        _logger = logger;
    }

    /// <summary>Analyzes the given text and returns frequency analysis results.</summary>
    /// <param name="text">The text to analyze (max 100,000 characters)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Frequency analysis with top 10 words, word counts, and longest word</returns>
    /// <exception cref="ArgumentException">Thrown if text is empty, exceeds max length, or contains no valid words</exception>
    public Task<FrequencyAnalysisResult> AnalyzeAsync(
        string text,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Text cannot be empty or whitespace only", nameof(text));

        if (text.Length > MaxTextLength)
            throw new ArgumentException(
                $"Text exceeds maximum allowed length of {MaxTextLength:N0} characters",
                nameof(text));

        // Tokenize and filter stop words
        var words = WordTokenizer.Tokenize(text, _stopWordProvider);

        if (words.Count == 0)
            throw new ArgumentException(
                "Text must contain at least one valid word after filtering stop words and special characters",
                nameof(text));

        // Count word frequencies using case-sensitive dictionary (words are pre-normalized to lowercase)
        var frequencies = new Dictionary<string, int>(StringComparer.Ordinal);
        foreach (var word in words)
        {
            if (frequencies.ContainsKey(word))
            {
                frequencies[word]++;
            }
            else
            {
                frequencies[word] = 1;
            }
        }

        // Get top 10 words sorted by frequency (descending) then alphabetically
        var topWords = frequencies
            .OrderByDescending(x => x.Value)
            .ThenBy(x => x.Key)
            .Take(MaxTopWordsCount)
            .Select(x => new WordFrequency
            {
                Word = x.Key,
                Frequency = x.Value
            })
            .ToList();

        var longestWord = WordTokenizer.FindLongestWord(words);

        var result = new FrequencyAnalysisResult
        {
            TopWords = topWords,
            TotalWordCount = words.Count,
            UniqueWordCount = frequencies.Count,
            LongestWord = longestWord
        };

        return Task.FromResult(result);
    }
}
