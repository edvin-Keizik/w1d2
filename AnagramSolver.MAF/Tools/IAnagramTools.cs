namespace AnagramSolver.MAF.Tools;

public interface IAnagramTools
{
    Task<AnagramToolResult> SearchAnagramsAsync(
        string input,
        int maxAnagrams = 2,
        int minWordLength = 2,
        CancellationToken cancellationToken = default);
    Task<WordCountResult> GetWordCountAsync(CancellationToken cancellationToken = default);
    Task<FilterByLengthResult> FilterByLengthAsync(
        int length,
        int maxResults = 100,
        CancellationToken cancellationToken = default);
}

public abstract class ToolResultBase
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}
public sealed class AnagramToolResult : ToolResultBase
{
    public string InputWord { get; set; } = string.Empty;
    public List<string> Anagrams { get; set; } = [];
    public int Count => Anagrams.Count;
}
public sealed class WordCountResult : ToolResultBase
{
    public int WordCount { get; set; }
}
public sealed class FilterByLengthResult : ToolResultBase
{
    public int FilteredLength { get; set; }
    public List<string> Words { get; set; } = [];
    public int Count => Words.Count;
}
