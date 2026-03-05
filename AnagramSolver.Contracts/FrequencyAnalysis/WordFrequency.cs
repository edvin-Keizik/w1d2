namespace AnagramSolver.Contracts.FrequencyAnalysis;

/// <summary>
/// Represents a word and its frequency (occurrence count) in analyzed text.
/// </summary>
public class WordFrequency
{
    /// <summary>The word (normalized to lowercase).</summary>
    public required string Word { get; set; }

    /// <summary>Number of times the word appears in the text.</summary>
    public int Frequency { get; set; }
}
