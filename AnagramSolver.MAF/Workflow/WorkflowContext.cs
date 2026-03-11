namespace AnagramSolver.MAF.Workflow;
public sealed class FinderInput
{
    public required string SearchWord { get; init; }
    public int MaxAnagrams { get; init; } = 2;
    public int MinWordLength { get; init; } = 2;
}
public sealed class FinderOutput
{
    public required string OriginalWord { get; init; }
    public required List<string> Anagrams { get; init; }
    public bool Success { get; init; }
    public TimeSpan SearchDuration { get; init; }
    public string? ErrorMessage { get; init; }
}
public sealed class AnalyzerOutput
{
    public required string OriginalWord { get; init; }
    public int TotalCount { get; init; }
    public required Dictionary<int, List<string>> GroupedByLength { get; init; }
    public required List<string> TopPicks { get; init; }
    public string? ShortestAnagram { get; init; }
    public string? LongestAnagram { get; init; }
    public double AverageLength { get; init; }
    public required List<string> Insights { get; init; }
}

public sealed class PresenterOutput
{
    public required string FormattedResponse { get; init; }
    public required string Summary { get; init; }
    public required AnalyzerOutput AnalysisData { get; init; }
}
