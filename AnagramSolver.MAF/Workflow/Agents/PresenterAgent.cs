using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace AnagramSolver.MAF.Workflow.Agents;

public sealed class PresenterAgent : IWorkflowStep<AnalyzerOutput, PresenterOutput>
{
    private readonly IChatClient _chatClient;

    public string Name => "[PRESENTER]";

    public string Description => "Formats the analysis into a beautiful, user-friendly response";

    public PresenterAgent(IChatClient chatClient)
    {
        _chatClient = chatClient ?? throw new ArgumentNullException(nameof(chatClient));
    }

    public async Task<PresenterOutput> ExecuteAsync(
        AnalyzerOutput input,
        CancellationToken cancellationToken = default)
    {
        var formatted = await GenerateFormattedResponseAsync(input, cancellationToken);
        var summary = GenerateSummary(input);

        return new PresenterOutput
        {
            FormattedResponse = formatted,
            Summary = summary,
            AnalysisData = input
        };
    }

    public async IAsyncEnumerable<WorkflowStreamUpdate> ExecuteStreamingAsync(
        AnalyzerOutput input,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        yield return new WorkflowStreamUpdate
        {
            StepName = Name,
            Type = UpdateType.Started,
            Content = "Formatting results for presentation..."
        };

        if (input.TotalCount == 0)
        {
            var noResultsOutput = new PresenterOutput
            {
                FormattedResponse = $"No anagrams found for '{input.OriginalWord}'. Try a different word or adjust the search parameters.",
                Summary = "No results",
                AnalysisData = input
            };

            yield return new WorkflowStreamUpdate
            {
                StepName = Name,
                Type = UpdateType.Completed,
                Content = "[OK] Formatting completed (no results)",
                Data = noResultsOutput
            };
            yield break;
        }

        yield return new WorkflowStreamUpdate
        {
            StepName = Name,
            Type = UpdateType.Progress,
            Content = "Creating response header..."
        };

        var sb = new StringBuilder();
        sb.AppendLine(GenerateHeader(input));

        yield return new WorkflowStreamUpdate
        {
            StepName = Name,
            Type = UpdateType.Progress,
            Content = sb.ToString()
        };

        yield return new WorkflowStreamUpdate
        {
            StepName = Name,
            Type = UpdateType.Progress,
            Content = "Highlighting top picks..."
        };

        sb.AppendLine(FormatTopPicks(input));

        yield return new WorkflowStreamUpdate
        {
            StepName = Name,
            Type = UpdateType.Progress,
            Content = "Organizing results by length..."
        };

        sb.AppendLine(FormatGroupedResults(input));

        yield return new WorkflowStreamUpdate
        {
            StepName = Name,
            Type = UpdateType.Progress,
            Content = "Adding insights..."
        };

        sb.AppendLine(FormatInsights(input));

        yield return new WorkflowStreamUpdate
        {
            StepName = Name,
            Type = UpdateType.Progress,
            Content = "Generating AI summary..."
        };

        var aiSummary = await GenerateAiSummaryAsync(input, cancellationToken);

        if (!string.IsNullOrWhiteSpace(aiSummary))
        {
            sb.AppendLine();
            sb.AppendLine("===========================================");
            sb.AppendLine("AI Summary:");
            sb.AppendLine(aiSummary);
        }

        sb.AppendLine();
        sb.AppendLine(GenerateFooter(input));

        var summary = GenerateSummary(input);

        var output = new PresenterOutput
        {
            FormattedResponse = sb.ToString(),
            Summary = summary,
            AnalysisData = input
        };

        yield return new WorkflowStreamUpdate
        {
            StepName = Name,
            Type = UpdateType.Completed,
            Content = "[OK] Presentation completed!",
            Data = output
        };
    }

    private static string GenerateHeader(AnalyzerOutput input)
    {
        var sb = new StringBuilder();
        sb.AppendLine();
        sb.AppendLine("+===============================================================+");
        sb.AppendLine($"|  ANAGRAM RESULTS FOR: {input.OriginalWord.ToUpper(),-38} |");
        sb.AppendLine("+===============================================================+");
        sb.AppendLine($"|  Total anagrams found: {input.TotalCount,-39} |");
        sb.AppendLine($"|  Length range: {input.ShortestAnagram?.Length ?? 0} to {input.LongestAnagram?.Length ?? 0} letters                              |");
        sb.AppendLine($"|  Average length: {input.AverageLength:F1} letters                            |");
        sb.AppendLine("+===============================================================+");
        return sb.ToString();
    }

    private static string FormatTopPicks(AnalyzerOutput input)
    {
        if (input.TopPicks.Count == 0)
            return string.Empty;

        var sb = new StringBuilder();
        sb.AppendLine();
        sb.AppendLine("TOP PICKS (Most Interesting Anagrams):");
        sb.AppendLine("-------------------------------------------");

        for (int i = 0; i < input.TopPicks.Count; i++)
        {
            var pick = input.TopPicks[i];
            var medal = i switch
            {
                0 => "[1st]",
                1 => "[2nd]",
                2 => "[3rd]",
                _ => "     "
            };
            sb.AppendLine($"  {medal} {pick} ({pick.Length} letters)");
        }

        return sb.ToString();
    }


    private static string FormatGroupedResults(AnalyzerOutput input)
    {
        if (input.GroupedByLength.Count == 0)
            return string.Empty;

        var sb = new StringBuilder();
        sb.AppendLine();
        sb.AppendLine("GROUPED BY LENGTH:");
        sb.AppendLine("-------------------------------------------");

        foreach (var group in input.GroupedByLength.OrderByDescending(g => g.Key))
        {
            var words = group.Value.Take(10).ToList();
            var moreCount = group.Value.Count - 10;
            var wordList = string.Join(", ", words);

            if (moreCount > 0)
            {
                wordList += $" (+{moreCount} more)";
            }

            sb.AppendLine($"  {group.Key}-letter words ({group.Value.Count}): {wordList}");
        }

        return sb.ToString();
    }

    private static string FormatInsights(AnalyzerOutput input)
    {
        if (input.Insights.Count == 0)
            return string.Empty;

        var sb = new StringBuilder();
        sb.AppendLine();
        sb.AppendLine("INSIGHTS:");
        sb.AppendLine("-------------------------------------------");

        foreach (var insight in input.Insights)
        {
            sb.AppendLine($"  * {insight}");
        }

        return sb.ToString();
    }

    private static string GenerateFooter(AnalyzerOutput input)
    {
        return $"===========================================\n" +
               $"Analysis complete for '{input.OriginalWord}'\n" +
               $"===========================================";
    }

    private static string GenerateSummary(AnalyzerOutput input)
    {
        if (input.TotalCount == 0)
            return $"No anagrams found for '{input.OriginalWord}'";

        return $"Found {input.TotalCount} anagrams for '{input.OriginalWord}' " +
               $"(lengths {input.ShortestAnagram?.Length}-{input.LongestAnagram?.Length})";
    }

    private async Task<string> GenerateFormattedResponseAsync(
        AnalyzerOutput input,
        CancellationToken cancellationToken)
    {
        var sb = new StringBuilder();
        
        sb.Append(GenerateHeader(input));
        sb.Append(FormatTopPicks(input));
        sb.Append(FormatGroupedResults(input));
        sb.Append(FormatInsights(input));

        var aiSummary = await GenerateAiSummaryAsync(input, cancellationToken);
        if (!string.IsNullOrWhiteSpace(aiSummary))
        {
            sb.AppendLine();
            sb.AppendLine("═══════════════════════════════════════════");
            sb.AppendLine("AI Summary:");
            sb.AppendLine(aiSummary);
        }

        sb.AppendLine();
        sb.Append(GenerateFooter(input));

        return sb.ToString();
    }

    private async Task<string> GenerateAiSummaryAsync(
        AnalyzerOutput input,
        CancellationToken cancellationToken)
    {
        if (input.TotalCount == 0)
            return string.Empty;

        try
        {
            var agent = new ChatClientAgent(
                _chatClient,
                new ChatClientAgentOptions
                {
                    Name = "SummaryWriter",
                    ChatOptions = new ChatOptions
                    {
                        Instructions = """
                            You are a friendly word game expert. Write a brief, engaging summary of anagram search results.
                            Be enthusiastic but concise. Include interesting observations about the words.
                            Keep your response under 100 words.
                            """,
                        MaxOutputTokens = 200
                    }
                });

            var session = await agent.CreateSessionAsync(cancellationToken);

            var prompt = $"""
                Original word: {input.OriginalWord}
                Total anagrams: {input.TotalCount}
                Top picks: {string.Join(", ", input.TopPicks)}
                Longest: {input.LongestAnagram}
                Shortest: {input.ShortestAnagram}
                
                Write a brief, engaging summary of these anagram results.
                """;

            var response = await agent.RunAsync(prompt, session, cancellationToken: cancellationToken);
            return response.Text?.Trim() ?? string.Empty;
        }
        catch
        {
            return "Unable to generate AI summary.";
        }
    }
}
