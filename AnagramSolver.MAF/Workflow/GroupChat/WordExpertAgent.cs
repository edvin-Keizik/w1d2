using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using AnagramSolver.MAF.Tools;

namespace AnagramSolver.MAF.Workflow.GroupChat;

public sealed class WordExpertAgent : IGroupChatAgent
{
    private readonly IChatClient _chatClient;
    private readonly IAnagramTools _anagramTools;
    private bool _hintGivenThisRound = false;
    private int _lastHintRound = 0;

    public string Name => "WordExpert";
    public string Role => "Word Expert - provides hints and word knowledge";

    public WordExpertAgent(IChatClient chatClient, IAnagramTools anagramTools)
    {
        _chatClient = chatClient ?? throw new ArgumentNullException(nameof(chatClient));
        _anagramTools = anagramTools ?? throw new ArgumentNullException(nameof(anagramTools));
    }

    public bool WantsToSpeak(ChatContext context)
    {
        if (!context.GameState.IsGameActive)
            return false;

        if (context.GameState.CurrentRound != _lastHintRound)
        {
            _hintGivenThisRound = false;
            _lastHintRound = context.GameState.CurrentRound;
        }

        if (context.GameState.AwaitingAnswer && 
            !_hintGivenThisRound &&
            context.GameState.CurrentChallenge != null)
        {
            var lastMessage = context.RecentMessages.LastOrDefault();
            if (lastMessage?.Type == MessageType.Challenge)
                return true;
        }

        return false;
    }

    public int GetSpeakingPriority(ChatContext context)
    {
        if (context.GameState.AwaitingAnswer && !_hintGivenThisRound)
            return 60;

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
                Content = $"[{Name}] Ready to share word wisdom!",
                Type = MessageType.Normal,
                WantsToSpeak = false
            };
        }

        _hintGivenThisRound = true;

        var hint = await GenerateHintAsync(challenge, cancellationToken);

        return new AgentResponse
        {
            Content = $"[{Name}]\n{hint}",
            Type = MessageType.Hint,
            Priority = 60
        };
    }

    private async Task<string> GenerateHintAsync(string word, CancellationToken cancellationToken)
    {
        var letters = word.ToLowerInvariant().ToCharArray();
        Array.Sort(letters);
        var sortedLetters = new string(letters);

        var vowels = word.Count(c => "aeiouy".Contains(char.ToLower(c)));
        var consonants = word.Length - vowels;

        var letterFreq = word.ToLowerInvariant()
            .GroupBy(c => c)
            .OrderByDescending(g => g.Count())
            .First();

        try
        {
            var result = await _anagramTools.SearchAnagramsAsync(word, 2, 2, cancellationToken);
            var anagramCount = result.Success ? result.Anagrams.Count : 0;

            var agent = new ChatClientAgent(
                _chatClient,
                new ChatClientAgentOptions
                {
                    Name = Name,
                    ChatOptions = new ChatOptions
                    {
                        Instructions = "You are a word expert giving a brief hint. Be helpful but don't give away the answer.",
                        MaxOutputTokens = 80
                    }
                });

            var session = await agent.CreateSessionAsync(cancellationToken);
            var response = await agent.RunAsync(
                $"Give a quick hint for finding anagrams of '{word}'. It has {word.Length} letters, " +
                $"{vowels} vowels, most common letter is '{letterFreq.Key}'. There are {anagramCount} possible anagrams.",
                session,
                cancellationToken: cancellationToken);

            return response.Text ?? GetFallbackHint(word, vowels, consonants, anagramCount);
        }
        catch
        {
            return GetFallbackHint(word, vowels, consonants, 0);
        }
    }

    private static string GetFallbackHint(string word, int vowels, int consonants, int anagramCount)
    {
        return $"Hint: '{word}' has {word.Length} letters ({vowels} vowels, {consonants} consonants). " +
               $"There are approximately {anagramCount} anagrams in the dictionary. Good luck!";
    }
}
