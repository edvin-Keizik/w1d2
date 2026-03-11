using AnagramSolver.MAF.Tools;

namespace AnagramSolver.MAF.Workflow.GroupChat;

public sealed class AnagramPlayerAgent : IGroupChatAgent
{
    private readonly IAnagramTools _anagramTools;
    private readonly string _playerName;
    private readonly Random _random = new();

    public string Name => _playerName;
    public string Role => "Anagram Player - finds anagrams using the dictionary";

    public AnagramPlayerAgent(IAnagramTools anagramTools, string playerName = "AnagramPlayer")
    {
        _anagramTools = anagramTools ?? throw new ArgumentNullException(nameof(anagramTools));
        _playerName = playerName;
    }

    public bool WantsToSpeak(ChatContext context)
    {
        if (!context.GameState.IsGameActive)
            return false;

        if (context.GameState.AwaitingAnswer && 
            context.GameState.CurrentChallenge != null)
            return true;

        return false;
    }

    public int GetSpeakingPriority(ChatContext context)
    {
        if (context.GameState.AwaitingAnswer)
            return 70;

        return 5;
    }

    public async Task<AgentResponse> GenerateResponseAsync(
        ChatContext context,
        CancellationToken cancellationToken = default)
    {
        var challenge = context.GameState.CurrentChallenge;
        
        if (string.IsNullOrEmpty(challenge))
        {
            return new AgentResponse
            {
                Content = $"[{Name}] I'm ready for a challenge!",
                Type = MessageType.Normal,
                WantsToSpeak = false
            };
        }

        try
        {
            var result = await _anagramTools.SearchAnagramsAsync(
                challenge, 
                maxAnagrams: 2, 
                minWordLength: 2, 
                cancellationToken);

            context.GameState.AwaitingAnswer = false;
            context.GameState.AwaitingEvaluation = true;

            if (result.Success && result.Anagrams.Count > 0)
            {
                var selectedAnagrams = result.Anagrams
                    .Where(a => !a.Equals(challenge, StringComparison.OrdinalIgnoreCase))
                    .Take(3)
                    .ToList();

                if (selectedAnagrams.Count > 0)
                {
                    var answer = selectedAnagrams[_random.Next(selectedAnagrams.Count)];
                    context.GameState.LastAnswer = answer;

                    var allFound = string.Join(", ", selectedAnagrams);
                    
                    return new AgentResponse
                    {
                        Content = $"[{Name}]\n" +
                                 $"I found some anagrams for '{challenge}'!\n" +
                                 $"My answer: {answer.ToUpper()}\n" +
                                 $"(Also found: {allFound})",
                        Type = MessageType.Answer,
                        Priority = 70
                    };
                }
            }

            context.GameState.LastAnswer = null;
            return new AgentResponse
            {
                Content = $"[{Name}]\n" +
                         $"Hmm, I couldn't find any anagrams for '{challenge}' in the dictionary.\n" +
                         "I'll pass on this one.",
                Type = MessageType.Answer,
                Priority = 70
            };
        }
        catch (Exception ex)
        {
            context.GameState.AwaitingAnswer = false;
            context.GameState.LastAnswer = null;
            
            return new AgentResponse
            {
                Content = $"[{Name}]\nI had trouble searching: {ex.Message}",
                Type = MessageType.Answer,
                Priority = 70
            };
        }
    }
}
