using AnagramSolver.MAF.Tools;

namespace AnagramSolver.MAF.Workflow.Handoff;

public sealed class AnagramSpecialist : ISpecialistAgent
{
    private readonly IAnagramTools _anagramTools;

    public string Name => "Anagram Specialist";
    public string Description => "Finds anagrams and word combinations from letters";
    public SpecialistType Type => SpecialistType.Anagram;

    public AnagramSpecialist(IAnagramTools anagramTools)
    {
        _anagramTools = anagramTools ?? throw new ArgumentNullException(nameof(anagramTools));
    }

    public bool CanHandle(string userMessage)
    {
        var lower = userMessage.ToLowerInvariant();
        return lower.Contains("anagram") || 
               lower.Contains("rearrange") || 
               lower.Contains("shuffle") ||
               lower.Contains("find words") ||
               lower.Contains("letters");
    }

    public async Task<SpecialistResult> HandleRequestAsync(
        string userMessage,
        string? extractedQuery,
        CancellationToken cancellationToken = default)
    {
        if (ShouldReturnToTriage(userMessage))
        {
            return new SpecialistResult
            {
                Response = "This seems to be a different type of request. Let me redirect you.",
                HandledBy = Type,
                ShouldReturnToTriage = true
            };
        }

        var searchWord = extractedQuery ?? ExtractSearchWord(userMessage);

        if (string.IsNullOrWhiteSpace(searchWord))
        {
            return new SpecialistResult
            {
                Response = "I'm the Anagram Specialist. Please provide a word and I'll find its anagrams.\n" +
                          "Example: 'Find anagrams for katas'",
                HandledBy = Type,
                ShouldReturnToTriage = false,
                SuggestedFollowUp = "What word would you like me to find anagrams for?"
            };
        }

        try
        {
            var result = await _anagramTools.SearchAnagramsAsync(
                searchWord,
                maxAnagrams: 2,
                minWordLength: 2,
                cancellationToken);

            if (!result.Success || result.Anagrams.Count == 0)
            {
                return new SpecialistResult
                {
                    Response = $"No anagrams found for '{searchWord}'.\n" +
                              "Try a different word or check the spelling.",
                    HandledBy = Type,
                    ShouldReturnToTriage = false,
                    SuggestedFollowUp = "Would you like to try another word?"
                };
            }

            var response = FormatAnagramResponse(searchWord, result.Anagrams);
            
            return new SpecialistResult
            {
                Response = response,
                HandledBy = Type,
                ShouldReturnToTriage = false,
                SuggestedFollowUp = "Would you like to search for more anagrams or analyze a word?"
            };
        }
        catch (Exception ex)
        {
            return new SpecialistResult
            {
                Response = $"Error searching for anagrams: {ex.Message}",
                HandledBy = Type,
                ShouldReturnToTriage = false
            };
        }
    }

    private static bool ShouldReturnToTriage(string message)
    {
        var lower = message.ToLowerInvariant();
        return lower.Contains("length") || 
               lower.Contains("how many letters") ||
               lower.Contains("frequency") ||
               lower.Contains("statistics") ||
               lower.Contains("analyze word");
    }

    private static string ExtractSearchWord(string message)
    {
        var lower = message.ToLowerInvariant();
        
        var patterns = new[] { "for ", "of ", "from ", "with " };
        foreach (var pattern in patterns)
        {
            var idx = lower.IndexOf(pattern, StringComparison.Ordinal);
            if (idx >= 0)
            {
                var after = message[(idx + pattern.Length)..].Trim();
                var word = after.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                if (!string.IsNullOrEmpty(word) && word.All(char.IsLetter))
                {
                    return word.ToLowerInvariant();
                }
            }
        }

        var words = message.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return words
            .Where(w => w.Length >= 3 && w.All(char.IsLetter))
            .LastOrDefault()?.ToLowerInvariant() ?? string.Empty;
    }

    private static string FormatAnagramResponse(string searchWord, List<string> anagrams)
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"[ANAGRAM SPECIALIST] Results for '{searchWord}':");
        sb.AppendLine($"Found {anagrams.Count} anagram(s):\n");

        var grouped = anagrams.GroupBy(a => a.Length).OrderByDescending(g => g.Key);
        
        foreach (var group in grouped)
        {
            sb.AppendLine($"  {group.Key}-letter words:");
            foreach (var word in group.Take(10))
            {
                sb.AppendLine($"    - {word}");
            }
            if (group.Count() > 10)
            {
                sb.AppendLine($"    ... and {group.Count() - 10} more");
            }
        }

        return sb.ToString();
    }
}
