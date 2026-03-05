namespace AnagramSolver.Contracts.FrequencyAnalysis;

/// <summary>
/// Provides stop word filtering for text analysis across multiple languages (Lithuanian and English).
/// </summary>
public interface IStopWordProvider
{
    /// <summary>
    /// Checks if a word is a stop word (case-insensitive).
    /// Must be called after InitializeAsync completes.
    /// </summary>
    /// <param name="word">The word to check (case-insensitive)</param>
    /// <returns>True if the word is a stop word; otherwise false</returns>
    bool IsStopWord(ReadOnlySpan<char> word);

    /// <summary>
    /// Initializes and loads stop words from embedded resources.
    /// Must be called during application startup.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for async operation</param>
    /// <returns>Task that completes when stop words are loaded</returns>
    /// <exception cref="Exception">Thrown if stop word resources cannot be loaded</exception>
    Task InitializeAsync(CancellationToken cancellationToken);
}
