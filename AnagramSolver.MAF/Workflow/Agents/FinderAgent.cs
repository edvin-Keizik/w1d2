using System.Diagnostics;
using System.Runtime.CompilerServices;
using AnagramSolver.MAF.Tools;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace AnagramSolver.MAF.Workflow.Agents;

public sealed class FinderAgent : IWorkflowStep<FinderInput, FinderOutput>
{
    private readonly IAnagramTools _anagramTools;
    private readonly IChatClient _chatClient;

    public string Name => "[FINDER]";

    public string Description => "Searches for anagrams using the AnagramSolver database";

    public FinderAgent(IAnagramTools anagramTools, IChatClient chatClient)
    {
        _anagramTools = anagramTools ?? throw new ArgumentNullException(nameof(anagramTools));
        _chatClient = chatClient ?? throw new ArgumentNullException(nameof(chatClient));
    }

    public async Task<FinderOutput> ExecuteAsync(
        FinderInput input,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(input.SearchWord))
        {
            return new FinderOutput
            {
                OriginalWord = input.SearchWord ?? string.Empty,
                Anagrams = [],
                Success = false,
                ErrorMessage = "Search word cannot be empty"
            };
        }

        var stopwatch = Stopwatch.StartNew();

        try
        {
            var result = await _anagramTools.SearchAnagramsAsync(
                input.SearchWord,
                input.MaxAnagrams,
                input.MinWordLength,
                cancellationToken);

            stopwatch.Stop();

            return new FinderOutput
            {
                OriginalWord = input.SearchWord,
                Anagrams = result.Anagrams,
                Success = result.Success,
                SearchDuration = stopwatch.Elapsed,
                ErrorMessage = result.Success ? null : result.Message
            };
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return new FinderOutput
            {
                OriginalWord = input.SearchWord,
                Anagrams = [],
                Success = false,
                SearchDuration = stopwatch.Elapsed,
                ErrorMessage = ex.Message
            };
        }
    }

    public async IAsyncEnumerable<WorkflowStreamUpdate> ExecuteStreamingAsync(
        FinderInput input,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        yield return new WorkflowStreamUpdate
        {
            StepName = Name,
            Type = UpdateType.Started,
            Content = $"Starting anagram search for '{input.SearchWord}'..."
        };

        if (string.IsNullOrWhiteSpace(input.SearchWord))
        {
            yield return new WorkflowStreamUpdate
            {
                StepName = Name,
                Type = UpdateType.Error,
                Content = "ERROR: Search word cannot be empty"
            };
            yield break;
        }

        yield return new WorkflowStreamUpdate
        {
            StepName = Name,
            Type = UpdateType.Progress,
            Content = $"Searching database for anagrams of '{input.SearchWord}'..."
        };

        var stopwatch = Stopwatch.StartNew();

        var result = await _anagramTools.SearchAnagramsAsync(
            input.SearchWord,
            input.MaxAnagrams,
            input.MinWordLength,
            cancellationToken);

        stopwatch.Stop();

        if (!result.Success)
        {
            yield return new WorkflowStreamUpdate
            {
                StepName = Name,
                Type = UpdateType.Error,
                Content = $"ERROR: Search failed: {result.Message}"
            };
            yield break;
        }

        yield return new WorkflowStreamUpdate
        {
            StepName = Name,
            Type = UpdateType.Progress,
            Content = $"Found {result.Anagrams.Count} anagrams in {stopwatch.ElapsedMilliseconds}ms"
        };

        if (result.Anagrams.Count > 0)
        {
            var preview = result.Anagrams.Take(5);
            yield return new WorkflowStreamUpdate
            {
                StepName = Name,
                Type = UpdateType.Progress,
                Content = $"Preview: {string.Join(", ", preview)}{(result.Anagrams.Count > 5 ? "..." : "")}"
            };
        }

        yield return new WorkflowStreamUpdate
        {
            StepName = Name,
            Type = UpdateType.Completed,
            Content = $"[OK] Finder completed: {result.Anagrams.Count} anagrams found",
            Data = new FinderOutput
            {
                OriginalWord = input.SearchWord,
                Anagrams = result.Anagrams,
                Success = true,
                SearchDuration = stopwatch.Elapsed
            }
        };
    }
}
