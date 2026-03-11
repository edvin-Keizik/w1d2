namespace AnagramSolver.MAF.Workflow;

public interface IWorkflowStep<TInput, TOutput>
{
    string Name { get; }
    string Description { get; }
    Task<TOutput> ExecuteAsync(TInput input, CancellationToken cancellationToken = default);
    IAsyncEnumerable<WorkflowStreamUpdate> ExecuteStreamingAsync(
        TInput input,
        CancellationToken cancellationToken = default);
}
public sealed class WorkflowStreamUpdate
{
    public required string StepName { get; init; }
    public required UpdateType Type { get; init; }
    public required string Content { get; init; }
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
    public object? Data { get; init; }
}
public enum UpdateType
{
    Started,
    Progress,
    Completed,
    Error
}
