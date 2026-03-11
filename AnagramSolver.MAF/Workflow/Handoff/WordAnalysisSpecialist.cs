
using AnagramSolver.MAF.Tools;

namespace AnagramSolver.MAF.Workflow.Handoff;

public sealed class WordAnalysisSpecialist : ISpecialistAgent
{
    private readonly IAnagramTools _anagramTools;

    public string Name => "Word Analysis Specialist";
    public string Description => "Analyzes words: length, letter frequency, statistics";
    public SpecialistType Type => SpecialistType.WordAnalysis;

    private static readonly HashSet<char> Vowels = ['a', 'e', 'i', 'o', 'u', 'y'];

    public WordAnalysisSpecialist(IAnagramTools anagramTools)
    {
        _anagramTools = anagramTools ?? throw new ArgumentNullException(nameof(anagramTools));
    }

    public bool CanHandle(string userMessage)
    {
        var lower = userMessage.ToLowerInvariant();
        return lower.Contains("length") ||
               lower.Contains("how many") ||
               lower.Contains("frequency") ||
               lower.Contains("statistics") ||
               lower.Contains("analyze") ||
               lower.Contains("letters in");
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
                Response = "This seems to be an anagram request. Let me redirect you.",
                HandledBy = Type,
                ShouldReturnToTriage = true
            };
        }

        var word = extractedQuery ?? ExtractWord(userMessage);

        if (string.IsNullOrWhiteSpace(word))
        {
            var dictResult = await HandleDictionaryStatsAsync(cancellationToken);
            if (dictResult != null) return dictResult;

            return new SpecialistResult
            {
                Response = "I'm the Word Analysis Specialist. I can analyze words for you.\n" +
                          "Tell me a word and I'll provide:\n" +
                          "  - Letter count\n" +
                          "  - Vowel/consonant breakdown\n" +
                          "  - Letter frequency\n\n" +
                          "Example: 'Analyze the word programming'",
                HandledBy = Type,
                ShouldReturnToTriage = false,
                SuggestedFollowUp = "What word would you like me to analyze?"
            };
        }

        var analysis = AnalyzeWord(word);
        
        return new SpecialistResult
        {
            Response = analysis,
            HandledBy = Type,
            ShouldReturnToTriage = false,
            SuggestedFollowUp = "Would you like to analyze another word or find anagrams?"
        };
    }

    private async Task<SpecialistResult?> HandleDictionaryStatsAsync(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _anagramTools.GetWordCountAsync(cancellationToken);
            if (result.Success)
            {
                return new SpecialistResult
                {
                    Response = $"[WORD ANALYSIS SPECIALIST] Dictionary Statistics:\n" +
                              $"Total words in dictionary: {result.WordCount:N0}\n\n" +
                              "I can also analyze specific words for you.",
                    HandledBy = Type,
                    ShouldReturnToTriage = false
                };
            }
        }
        catch { }
        
        return null;
    }

    private static bool ShouldReturnToTriage(string message)
    {
        var lower = message.ToLowerInvariant();
        return lower.Contains("anagram") ||
               lower.Contains("rearrange") ||
               lower.Contains("shuffle");
    }

    private static string ExtractWord(string message)
    {
        var lower = message.ToLowerInvariant();
        
        var patterns = new[] { "word ", "analyze ", "about ", "for ", "of " };
        foreach (var pattern in patterns)
        {
            var idx = lower.IndexOf(pattern, StringComparison.Ordinal);
            if (idx >= 0)
            {
                var after = message[(idx + pattern.Length)..].Trim();
                var word = after.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                if (!string.IsNullOrEmpty(word))
                {
                    return new string(word.Where(char.IsLetter).ToArray()).ToLowerInvariant();
                }
            }
        }

        var words = message.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return words
            .Select(w => new string(w.Where(char.IsLetter).ToArray()))
            .Where(w => w.Length >= 3)
            .LastOrDefault()?.ToLowerInvariant() ?? string.Empty;
    }

    private static string AnalyzeWord(string word)
    {
        var cleanWord = word.ToLowerInvariant();
        var length = cleanWord.Length;
        
        var vowelCount = cleanWord.Count(c => Vowels.Contains(c));
        var consonantCount = cleanWord.Count(c => char.IsLetter(c) && !Vowels.Contains(c));
        
        var frequency = cleanWord
            .GroupBy(c => c)
            .OrderByDescending(g => g.Count())
            .ThenBy(g => g.Key)
            .ToDictionary(g => g.Key, g => g.Count());

        var uniqueLetters = frequency.Count;
        var mostCommon = frequency.First();
        
        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"[WORD ANALYSIS SPECIALIST] Analysis for '{word}':\n");
        sb.AppendLine($"  Length: {length} characters");
        sb.AppendLine($"  Unique letters: {uniqueLetters}");
        sb.AppendLine($"  Vowels: {vowelCount} ({100.0 * vowelCount / length:F1}%)");
        sb.AppendLine($"  Consonants: {consonantCount} ({100.0 * consonantCount / length:F1}%)");
        sb.AppendLine($"  Most common letter: '{mostCommon.Key}' (appears {mostCommon.Value}x)");
        sb.AppendLine();
        sb.AppendLine("  Letter frequency:");
        
        foreach (var kvp in frequency)
        {
            var bar = new string('#', kvp.Value);
            sb.AppendLine($"    {kvp.Key}: {bar} ({kvp.Value})");
        }

        return sb.ToString();
    }
}
