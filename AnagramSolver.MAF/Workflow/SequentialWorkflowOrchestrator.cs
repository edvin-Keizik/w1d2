using System.Runtime.CompilerServices;
using AnagramSolver.MAF.Workflow.Agents;

namespace AnagramSolver.MAF.Workflow;

public sealed class SequentialWorkflowOrchestrator
{
    private readonly FinderAgent _finder;
    private readonly AnalyzerAgent _analyzer;
    private readonly PresenterAgent _presenter;

    public SequentialWorkflowOrchestrator(
        FinderAgent finder,
        AnalyzerAgent analyzer,
        PresenterAgent presenter)
    {
        _finder = finder ?? throw new ArgumentNullException(nameof(finder));
        _analyzer = analyzer ?? throw new ArgumentNullException(nameof(analyzer));
        _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
    }

    public async Task<PresenterOutput> ExecuteAsync(
        string searchWord,
        int maxAnagrams = 2,
        int minWordLength = 2,
        CancellationToken cancellationToken = default)
    {
        var input = new FinderInput
        {
            SearchWord = searchWord,
            MaxAnagrams = maxAnagrams,
            MinWordLength = minWordLength
        };

        var finderResult = await _finder.ExecuteAsync(input, cancellationToken);

        var analyzerResult = await _analyzer.ExecuteAsync(finderResult, cancellationToken);

        var presenterResult = await _presenter.ExecuteAsync(analyzerResult, cancellationToken);

        return presenterResult;
    }

    public async IAsyncEnumerable<WorkflowStreamUpdate> ExecuteStreamingAsync(
        string searchWord,
        int maxAnagrams = 2,
        int minWordLength = 2,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var input = new FinderInput
        {
            SearchWord = searchWord,
            MaxAnagrams = maxAnagrams,
            MinWordLength = minWordLength
        };

        yield return new WorkflowStreamUpdate
        {
            StepName = "[WORKFLOW]",
            Type = UpdateType.Started,
            Content = $"Starting sequential workflow for '{searchWord}'..."
        };

        yield return new WorkflowStreamUpdate
        {
            StepName = "[WORKFLOW]",
            Type = UpdateType.Progress,
            Content = "Pipeline: [FINDER] -> [ANALYZER] -> [PRESENTER]"
        };

        FinderOutput? finderResult = null;

        await foreach (var update in _finder.ExecuteStreamingAsync(input, cancellationToken))
        {
            yield return update;

            if (update.Type == UpdateType.Completed && update.Data is FinderOutput fo)
            {
                finderResult = fo;
            }

            if (update.Type == UpdateType.Error)
            {
                yield return new WorkflowStreamUpdate
                {
                    StepName = "[WORKFLOW]",
                    Type = UpdateType.Error,
                    Content = "Workflow aborted due to Finder error"
                };
                yield break;
            }
        }

        if (finderResult == null)
        {
            finderResult = await _finder.ExecuteAsync(input, cancellationToken);
        }

        yield return new WorkflowStreamUpdate
        {
            StepName = "[WORKFLOW]",
            Type = UpdateType.Progress,
            Content = "--------------------------------------------"
        };

        AnalyzerOutput? analyzerResult = null;

        await foreach (var update in _analyzer.ExecuteStreamingAsync(finderResult, cancellationToken))
        {
            yield return update;

            if (update.Type == UpdateType.Completed && update.Data is AnalyzerOutput ao)
            {
                analyzerResult = ao;
            }

            if (update.Type == UpdateType.Error)
            {
                yield return new WorkflowStreamUpdate
                {
                    StepName = "[WORKFLOW]",
                    Type = UpdateType.Error,
                    Content = "Workflow aborted due to Analyzer error"
                };
                yield break;
            }
        }

        if (analyzerResult == null)
        {
            analyzerResult = await _analyzer.ExecuteAsync(finderResult, cancellationToken);
        }

        yield return new WorkflowStreamUpdate
        {
            StepName = "[WORKFLOW]",
            Type = UpdateType.Progress,
            Content = "--------------------------------------------"
        };

        PresenterOutput? presenterResult = null;

        await foreach (var update in _presenter.ExecuteStreamingAsync(analyzerResult, cancellationToken))
        {
            yield return update;

            if (update.Type == UpdateType.Completed && update.Data is PresenterOutput po)
            {
                presenterResult = po;
            }

            if (update.Type == UpdateType.Error)
            {
                yield return new WorkflowStreamUpdate
                {
                    StepName = "[WORKFLOW]",
                    Type = UpdateType.Error,
                    Content = "Workflow aborted due to Presenter error"
                };
                yield break;
            }
        }

        if (presenterResult == null)
        {
            presenterResult = await _presenter.ExecuteAsync(analyzerResult, cancellationToken);
        }

        yield return new WorkflowStreamUpdate
        {
            StepName = "Workflow",
            Type = UpdateType.Progress,
            Content = "════════════════════════════════════════════"
        };

        yield return new WorkflowStreamUpdate
        {
            StepName = "Workflow",
            Type = UpdateType.Completed,
            Content = "Sequential workflow completed successfully!",
            Data = presenterResult
        };
    }
}
