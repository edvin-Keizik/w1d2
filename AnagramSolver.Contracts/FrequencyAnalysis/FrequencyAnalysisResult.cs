namespace AnagramSolver.Contracts.FrequencyAnalysis;

/// <summary>
/// Result of word frequency analysis on a text.
/// </summary>
public class FrequencyAnalysisResult
{
    /// <summary>Top 10 words sorted by frequency (descending) then alphabetically.</summary>
    public required List<WordFrequency> TopWords { get; set; }

    /// <summary>Total number of words in the analyzed text (excluding stop words).</summary>
    public int TotalWordCount { get; set; }

    /// <summary>Number of unique words in the analyzed text.</summary>
    public int UniqueWordCount { get; set; }

    /// <summary>The longest word by character count. Empty string if no valid words.</summary>
    public required string LongestWord { get; set; }
}
