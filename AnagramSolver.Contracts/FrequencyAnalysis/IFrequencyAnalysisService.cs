namespace AnagramSolver.Contracts.FrequencyAnalysis;

/// <summary>
/// Service for analyzing word frequency in text.
/// 
/// Supports stop word filtering and Lithuanian language characters.
/// Text input is limited to 100,000 characters for performance.
/// </summary>
public interface IFrequencyAnalysisService
{
    /// <summary>
    /// Analyzes the given text and returns word frequency analysis results.
    /// </summary>
    /// <param name="text">
    /// The text to analyze. Must be non-empty and contain at least one valid word.
    /// Maximum length: 100,000 characters.
    /// </param>
    /// <param name="cancellationToken">Cancellation token for async operation</param>
    /// <returns>
    /// Frequency analysis result containing:
    /// - TopWords: Up to 10 most frequent words sorted by frequency (descending) then alphabetically
    /// - TotalWordCount: Total number of words (excluding stop words)
    /// - UniqueWordCount: Number of unique words
    /// - LongestWord: The longest word by character count
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when:
    /// - Text is null, empty, or whitespace only
    /// - Text exceeds 100,000 characters
    /// - Text contains no valid words after filtering stop words
    /// </exception>
    Task<FrequencyAnalysisResult> AnalyzeAsync(
        string text,
        CancellationToken cancellationToken);
}
