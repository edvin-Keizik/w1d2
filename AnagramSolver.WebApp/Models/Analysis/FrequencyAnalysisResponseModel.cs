namespace AnagramSolver.WebApp.Models.Analysis;

public class FrequencyAnalysisResponseModel
{
    public required List<WordFrequencyModel> TopWords { get; set; }
    public int TotalWordCount { get; set; }
    public int UniqueWordCount { get; set; }
    public required string LongestWord { get; set; }
    public DateTime AnalyzedAt { get; set; }
}
