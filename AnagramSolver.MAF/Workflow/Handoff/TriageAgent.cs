using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace AnagramSolver.MAF.Workflow.Handoff;

public sealed class TriageAgent
{
    private readonly IChatClient _chatClient;
    private readonly List<ISpecialistAgent> _specialists;

    private const string TriageInstructions = """
        You are a triage agent that classifies user requests and routes them to specialists.
        
        Analyze the user's message and determine what they want:
        
        1. ANAGRAM requests - user wants to:
           - Find anagrams for a word
           - Search for word rearrangements
           - Find words from letters
           - Keywords: anagram, rearrange, shuffle, letters, find words
        
        2. WORD_ANALYSIS requests - user wants to:
           - Know word length or character count
           - Get letter frequency analysis
           - Word statistics (vowels, consonants)
           - Compare words
           - Keywords: length, how many letters, frequency, statistics, analyze
        
        3. GENERAL - greetings, help requests, unclear queries
        
        Respond with EXACTLY one line in format:
        SPECIALIST:ANAGRAM|WORD_ANALYSIS|GENERAL
        QUERY:<extracted word or query if applicable>
        REASON:<brief explanation>
        """;

    public TriageAgent(IChatClient chatClient, IEnumerable<ISpecialistAgent> specialists)
    {
        _chatClient = chatClient ?? throw new ArgumentNullException(nameof(chatClient));
        _specialists = specialists?.ToList() ?? throw new ArgumentNullException(nameof(specialists));
    }

    public async Task<TriageResult> ClassifyIntentAsync(
        string userMessage,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userMessage))
        {
            return new TriageResult
            {
                RecommendedSpecialist = SpecialistType.None,
                Reasoning = "Empty message received",
                IsGeneralConversation = true
            };
        }

        try
        {
            var agent = new ChatClientAgent(
                _chatClient,
                new ChatClientAgentOptions
                {
                    Name = "TriageClassifier",
                    ChatOptions = new ChatOptions
                    {
                        Instructions = TriageInstructions,
                        MaxOutputTokens = 150
                    }
                });

            var session = await agent.CreateSessionAsync(cancellationToken);
            var response = await agent.RunAsync(userMessage, session, cancellationToken: cancellationToken);
            
            return ParseTriageResponse(response.Text ?? string.Empty, userMessage);
        }
        catch
        {
            return FallbackClassification(userMessage);
        }
    }

    public async Task<string> HandleGeneralQueryAsync(
        string userMessage,
        CancellationToken cancellationToken = default)
    {
        var agent = new ChatClientAgent(
            _chatClient,
            new ChatClientAgentOptions
            {
                Name = "TriageResponder",
                ChatOptions = new ChatOptions
                {
                    Instructions = """
                        You are a helpful assistant for the AnagramSolver application.
                        You can help users with:
                        - Finding anagrams (rearranging letters to form words)
                        - Analyzing words (length, letter frequency, statistics)
                        
                        If the user's request is unclear, ask for clarification.
                        Be concise and helpful.
                        """,
                    MaxOutputTokens = 200
                }
            });

        var session = await agent.CreateSessionAsync(cancellationToken);
        var response = await agent.RunAsync(userMessage, session, cancellationToken: cancellationToken);
        return response.Text ?? "I'm not sure how to help with that. Try asking about anagrams or word analysis.";
    }

    public ISpecialistAgent? GetSpecialist(SpecialistType type)
    {
        return _specialists.FirstOrDefault(s => s.Type == type);
    }

    private static TriageResult ParseTriageResponse(string response, string originalMessage)
    {
        var specialist = SpecialistType.None;
        string? query = null;
        var reasoning = "Classification completed";
        var isGeneral = false;

        var lines = response.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        foreach (var line in lines)
        {
            var trimmed = line.Trim();
            
            if (trimmed.StartsWith("SPECIALIST:", StringComparison.OrdinalIgnoreCase))
            {
                var value = trimmed[11..].Trim().ToUpperInvariant();
                specialist = value switch
                {
                    "ANAGRAM" => SpecialistType.Anagram,
                    "WORD_ANALYSIS" => SpecialistType.WordAnalysis,
                    _ => SpecialistType.None
                };
                isGeneral = value == "GENERAL";
            }
            else if (trimmed.StartsWith("QUERY:", StringComparison.OrdinalIgnoreCase))
            {
                query = trimmed[6..].Trim();
                if (query.Equals("none", StringComparison.OrdinalIgnoreCase) || 
                    query.Equals("n/a", StringComparison.OrdinalIgnoreCase))
                {
                    query = null;
                }
            }
            else if (trimmed.StartsWith("REASON:", StringComparison.OrdinalIgnoreCase))
            {
                reasoning = trimmed[7..].Trim();
            }
        }

        if (query == null)
        {
            query = ExtractQueryFromMessage(originalMessage);
        }

        return new TriageResult
        {
            RecommendedSpecialist = specialist,
            Reasoning = reasoning,
            ExtractedQuery = query,
            IsGeneralConversation = isGeneral
        };
    }

    private static TriageResult FallbackClassification(string message)
    {
        var lower = message.ToLowerInvariant();
        
        if (lower.Contains("anagram") || lower.Contains("rearrange") || 
            lower.Contains("shuffle") || lower.Contains("find words"))
        {
            return new TriageResult
            {
                RecommendedSpecialist = SpecialistType.Anagram,
                Reasoning = "Detected anagram-related keywords",
                ExtractedQuery = ExtractQueryFromMessage(message),
                IsGeneralConversation = false
            };
        }
        
        if (lower.Contains("length") || lower.Contains("how many") || 
            lower.Contains("frequency") || lower.Contains("analyze") ||
            lower.Contains("statistics") || lower.Contains("letters in"))
        {
            return new TriageResult
            {
                RecommendedSpecialist = SpecialistType.WordAnalysis,
                Reasoning = "Detected word analysis keywords",
                ExtractedQuery = ExtractQueryFromMessage(message),
                IsGeneralConversation = false
            };
        }
        
        return new TriageResult
        {
            RecommendedSpecialist = SpecialistType.None,
            Reasoning = "General conversation",
            IsGeneralConversation = true
        };
    }

    private static string? ExtractQueryFromMessage(string message)
    {
        var words = message.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var potentialWords = words
            .Where(w => w.Length >= 3 && w.All(char.IsLetter))
            .ToList();
        
        return potentialWords.LastOrDefault();
    }
}
