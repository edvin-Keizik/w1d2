namespace AnagramSolver.MAF.Workflow.Handoff;

public enum SpecialistType
{
    None,
    Anagram,
    WordAnalysis,
    ReturnToTriage
}
public sealed class TriageResult
{
    public required SpecialistType RecommendedSpecialist { get; init; }
    public required string Reasoning { get; init; }
    public string? ExtractedQuery { get; init; }
    public bool IsGeneralConversation { get; init; }
}

public sealed class SpecialistResult
{
    public required string Response { get; init; }
    public required SpecialistType HandledBy { get; init; }
    public bool ShouldReturnToTriage { get; init; }
    public string? SuggestedFollowUp { get; init; }
}
public interface ISpecialistAgent
{
    string Name { get; }
    string Description { get; }
    SpecialistType Type { get; }
    
    Task<SpecialistResult> HandleRequestAsync(
        string userMessage,
        string? extractedQuery,
        CancellationToken cancellationToken = default);
    
    bool CanHandle(string userMessage);
}
