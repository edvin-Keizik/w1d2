namespace AnagramSolver.Contracts.FrequencyAnalysis;

public class FrequencyAnalysisRequest
{
    public required string Text { get; set; }
    public int MaxLength { get; set; } = 100000;
}
