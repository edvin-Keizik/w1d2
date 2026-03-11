using System.Runtime.CompilerServices;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace AnagramSolver.MAF.Workflow.Agents;

public sealed class AnalyzerAgent : IWorkflowStep<FinderOutput, AnalyzerOutput>
{
    private readonly IChatClient _chatClient;

    public string Name => "[ANALYZER]";

    /// <inheritdoc />
    public string Description => "Analyzes anagrams: groups by length, sorts, identifies interesting patterns";

    public AnalyzerAgent(IChatClient chatClient)
    {
        _chatClient = chatClient ?? throw new ArgumentNullException(nameof(chatClient));
    }

    public async Task<AnalyzerOutput> ExecuteAsync(
        FinderOutput input,
        CancellationToken cancellationToken = default)
    {
        if (!input.Success || input.Anagrams.Count == 0)
        {
            return new AnalyzerOutput
            {
                OriginalWord = input.OriginalWord,
                TotalCount = 0,
                GroupedByLength = new Dictionary<int, List<string>>(),
                TopPicks = [],
                Insights = [input.ErrorMessage ?? "No anagrams found to analyze"]
            };
        }

        var grouped = GroupByLength(input.Anagrams);
        var sorted = SortAnagrams(input.Anagrams);
        var topPicks = SelectTopPicks(input.Anagrams, input.OriginalWord);
        var insights = await GenerateInsightsAsync(input, grouped, cancellationToken);

        return new AnalyzerOutput
        {
            OriginalWord = input.OriginalWord,
            TotalCount = input.Anagrams.Count,
            GroupedByLength = grouped,
            TopPicks = topPicks,
            ShortestAnagram = sorted.FirstOrDefault(),
            LongestAnagram = sorted.LastOrDefault(),
            AverageLength = input.Anagrams.Count > 0 
                ? input.Anagrams.Average(a => a.Length) 
                : 0,
            Insights = insights
        };
    }

    public async IAsyncEnumerable<WorkflowStreamUpdate> ExecuteStreamingAsync(
        FinderOutput input,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        yield return new WorkflowStreamUpdate
        {
            StepName = Name,
            Type = UpdateType.Started,
            Content = $"Starting analysis of {input.Anagrams.Count} anagrams..."
        };

        if (!input.Success || input.Anagrams.Count == 0)
        {
            yield return new WorkflowStreamUpdate
            {
                StepName = Name,
                Type = UpdateType.Error,
                Content = $"ERROR: No anagrams to analyze: {input.ErrorMessage ?? "Empty result set"}"
            };
            yield break;
        }

        yield return new WorkflowStreamUpdate
        {
            StepName = Name,
            Type = UpdateType.Progress,
            Content = "Grouping anagrams by length..."
        };

        var grouped = GroupByLength(input.Anagrams);

        yield return new WorkflowStreamUpdate
        {
            StepName = Name,
            Type = UpdateType.Progress,
            Content = $"   Found {grouped.Count} different length groups: {string.Join(", ", grouped.Keys.OrderBy(k => k).Select(k => $"{k}-letter ({grouped[k].Count})"))}"
        };

        yield return new WorkflowStreamUpdate
        {
            StepName = Name,
            Type = UpdateType.Progress,
            Content = "Sorting anagrams..."
        };

        var sorted = SortAnagrams(input.Anagrams);
        var shortest = sorted.FirstOrDefault();
        var longest = sorted.LastOrDefault();

        yield return new WorkflowStreamUpdate
        {
            StepName = Name,
            Type = UpdateType.Progress,
            Content = $"   Shortest: '{shortest}' ({shortest?.Length} letters), Longest: '{longest}' ({longest?.Length} letters)"
        };

        yield return new WorkflowStreamUpdate
        {
            StepName = Name,
            Type = UpdateType.Progress,
            Content = "Selecting most interesting anagrams..."
        };

        var topPicks = SelectTopPicks(input.Anagrams, input.OriginalWord);

        yield return new WorkflowStreamUpdate
        {
            StepName = Name,
            Type = UpdateType.Progress,
            Content = $"   Top picks: {string.Join(", ", topPicks)}"
        };

        yield return new WorkflowStreamUpdate
        {
            StepName = Name,
            Type = UpdateType.Progress,
            Content = "Generating insights with AI..."
        };

        var insights = await GenerateInsightsAsync(input, grouped, cancellationToken);

        foreach (var insight in insights)
        {
            yield return new WorkflowStreamUpdate
            {
                StepName = Name,
                Type = UpdateType.Progress,
                Content = $"   -> {insight}"
            };
        }

        var avgLength = input.Anagrams.Count > 0 
            ? input.Anagrams.Average(a => a.Length) 
            : 0;

        var output = new AnalyzerOutput
        {
            OriginalWord = input.OriginalWord,
            TotalCount = input.Anagrams.Count,
            GroupedByLength = grouped,
            TopPicks = topPicks,
            ShortestAnagram = shortest,
            LongestAnagram = longest,
            AverageLength = avgLength,
            Insights = insights
        };

        yield return new WorkflowStreamUpdate
        {
            StepName = Name,
            Type = UpdateType.Completed,
            Content = $"[OK] Analysis completed: {insights.Count} insights generated",
            Data = output
        };
    }

    private static Dictionary<int, List<string>> GroupByLength(List<string> anagrams)
    {
        return anagrams
            .GroupBy(a => a.Length)
            .OrderBy(g => g.Key)
            .ToDictionary(g => g.Key, g => g.OrderBy(w => w).ToList());
    }

    private static List<string> SortAnagrams(List<string> anagrams)
    {
        return anagrams
            .OrderBy(a => a.Length)
            .ThenBy(a => a)
            .ToList();
    }

    private static List<string> SelectTopPicks(List<string> anagrams, string originalWord)
    {
        var picks = new List<string>();
        var originalLength = originalWord.Length;

        var exactMatches = anagrams
            .Where(a => a.Length == originalLength && !a.Equals(originalWord, StringComparison.OrdinalIgnoreCase))
            .Take(3)
            .ToList();
        picks.AddRange(exactMatches);

        var longest = anagrams
            .OrderByDescending(a => a.Length)
            .Where(a => !picks.Contains(a))
            .Take(2)
            .ToList();
        picks.AddRange(longest);

        var unique = anagrams
            .Where(a => !picks.Contains(a))
            .OrderByDescending(a => CalculateUniqueness(a, originalWord))
            .Take(2)
            .ToList();
        picks.AddRange(unique);

        return picks.Distinct().Take(7).ToList();
    }

    private static int CalculateUniqueness(string anagram, string original)
    {
        int score = 0;
        int minLen = Math.Min(anagram.Length, original.Length);

        for (int i = 0; i < minLen; i++)
        {
            if (char.ToLower(anagram[i]) != char.ToLower(original[i]))
            {
                score++;
            }
        }

        score += Math.Abs(anagram.Length - original.Length);

        return score;
    }

    private async Task<List<string>> GenerateInsightsAsync(
        FinderOutput input,
        Dictionary<int, List<string>> grouped,
        CancellationToken cancellationToken)
    {
        var insights = new List<string>();

        if (input.Anagrams.Count > 0)
        {
            insights.Add($"Found {input.Anagrams.Count} anagrams for '{input.OriginalWord}'");
            
            var mostCommonLength = grouped
                .OrderByDescending(g => g.Value.Count)
                .First();
            insights.Add($"Most common length: {mostCommonLength.Key} letters ({mostCommonLength.Value.Count} words)");

            if (input.SearchDuration.TotalMilliseconds > 0)
            {
                insights.Add($"Search completed in {input.SearchDuration.TotalMilliseconds:F0}ms");
            }
        }

        try
        {
            var chatAgent = new ChatClientAgent(
                _chatClient,
                new ChatClientAgentOptions
                {
                    Name = "InsightGenerator",
                    ChatOptions = new ChatOptions
                    {
                        Instructions = "You are a word analysis expert. Generate ONE short, interesting observation about anagram patterns. Keep it under 100 characters. Be creative and fun.",
                        MaxOutputTokens = 100
                    }
                });

            var session = await chatAgent.CreateSessionAsync(cancellationToken);
            var sampleWords = string.Join(", ", input.Anagrams.Take(10));
            
            var response = await chatAgent.RunAsync(
                $"Original word: '{input.OriginalWord}'. Sample anagrams: {sampleWords}. Give one interesting observation.",
                session,
                cancellationToken: cancellationToken);

            if (!string.IsNullOrWhiteSpace(response.Text))
            {
                insights.Add(response.Text.Trim());
            }
        }
        catch
        {
            insights.Add("AI analysis unavailable - showing statistical insights only");
        }

        return insights;
    }
}
