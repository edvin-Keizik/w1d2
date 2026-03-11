using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace AnagramSolver.MAF.Workflow.GroupChat;

public sealed class GameHostAgent : IGroupChatAgent
{
    private readonly IChatClient _chatClient;
    private readonly Random _random = new();
    
    private static readonly string[] ChallengeWords = 
    [
        "katas", "vilnius", "lietuva", "zodis", "raide", "knyga", 
        "langas", "stalas", "medis", "vanduo", "saule", "menulis"
    ];

    public string Name => "GameHost";
    public string Role => "Game Moderator - proposes challenges and manages rounds";

    public GameHostAgent(IChatClient chatClient)
    {
        _chatClient = chatClient ?? throw new ArgumentNullException(nameof(chatClient));
    }

    public bool WantsToSpeak(ChatContext context)
    {
        if (!context.GameState.IsGameActive && context.GameState.CurrentRound == 0)
            return true;

        if (context.GameState.IsGameActive && 
            !context.GameState.AwaitingAnswer && 
            !context.GameState.AwaitingEvaluation &&
            context.GameState.CurrentChallenge == null)
            return true;

        if (context.GameState.CurrentRound > context.GameState.TotalRounds)
            return true;

        return false;
    }

    public int GetSpeakingPriority(ChatContext context)
    {
        if (!context.GameState.IsGameActive && context.GameState.CurrentRound == 0)
            return 100;

        if (context.GameState.CurrentRound > context.GameState.TotalRounds)
            return 100;

        if (context.GameState.CurrentChallenge == null && context.GameState.IsGameActive)
            return 80;

        return 10;
    }

    public async Task<AgentResponse> GenerateResponseAsync(
        ChatContext context,
        CancellationToken cancellationToken = default)
    {
        if (!context.GameState.IsGameActive && context.GameState.CurrentRound == 0)
        {
            return await GenerateWelcomeAsync(context, cancellationToken);
        }

        if (context.GameState.CurrentRound > context.GameState.TotalRounds)
        {
            return await GenerateGameEndAsync(context, cancellationToken);
        }

        return await GenerateChallengeAsync(context, cancellationToken);
    }

    private async Task<AgentResponse> GenerateWelcomeAsync(
        ChatContext context,
        CancellationToken cancellationToken)
    {
        context.GameState.IsGameActive = true;
        context.GameState.CurrentRound = 1;

        var agent = CreateAgent("Welcome players to the Anagram Word Game. Be enthusiastic but brief.");
        var session = await agent.CreateSessionAsync(cancellationToken);
        var response = await agent.RunAsync(
            $"Welcome everyone to a {context.GameState.TotalRounds}-round anagram game. Explain briefly that you'll give a word, players find anagrams, and the judge scores.",
            session,
            cancellationToken: cancellationToken);

        return new AgentResponse
        {
            Content = $"[GAME HOST]\n{response.Text ?? GetFallbackWelcome(context)}",
            Type = MessageType.SystemAnnouncement,
            Priority = 100
        };
    }

    private async Task<AgentResponse> GenerateChallengeAsync(
        ChatContext context,
        CancellationToken cancellationToken)
    {
        var word = SelectChallengeWord(context);
        context.GameState.CurrentChallenge = word;
        context.GameState.AwaitingAnswer = true;
        context.GameState.UsedWords.Add(word);

        var agent = CreateAgent("Present a word challenge. Be encouraging.");
        var session = await agent.CreateSessionAsync(cancellationToken);
        var response = await agent.RunAsync(
            $"Round {context.GameState.CurrentRound}: Present the challenge word '{word}' for anagram finding.",
            session,
            cancellationToken: cancellationToken);

        return new AgentResponse
        {
            Content = $"[GAME HOST - Round {context.GameState.CurrentRound}]\n" +
                     $"{response.Text ?? $"Challenge word: {word.ToUpper()}"}\n" +
                     $"Challenge: {word.ToUpper()}",
            Type = MessageType.Challenge,
            Priority = 80
        };
    }

    private async Task<AgentResponse> GenerateGameEndAsync(
        ChatContext context,
        CancellationToken cancellationToken)
    {
        context.GameState.IsGameActive = false;

        var winner = context.GameState.Scores
            .OrderByDescending(s => s.Value)
            .FirstOrDefault();

        var scoreBoard = string.Join("\n", context.GameState.Scores
            .OrderByDescending(s => s.Value)
            .Select(s => $"  {s.Key}: {s.Value} points"));

        var agent = CreateAgent("Announce game end and winner. Be celebratory.");
        var session = await agent.CreateSessionAsync(cancellationToken);
        var response = await agent.RunAsync(
            $"Game over! Winner: {winner.Key} with {winner.Value} points. Thank everyone.",
            session,
            cancellationToken: cancellationToken);

        return new AgentResponse
        {
            Content = $"[GAME HOST - FINAL RESULTS]\n" +
                     $"{response.Text ?? "Game Over!"}\n\n" +
                     $"Final Scores:\n{scoreBoard}",
            Type = MessageType.SystemAnnouncement,
            Priority = 100
        };
    }

    private string SelectChallengeWord(ChatContext context)
    {
        var available = ChallengeWords.Except(context.GameState.UsedWords).ToList();
        if (available.Count == 0)
            available = ChallengeWords.ToList();
        
        return available[_random.Next(available.Count)];
    }

    private ChatClientAgent CreateAgent(string instruction)
    {
        return new ChatClientAgent(
            _chatClient,
            new ChatClientAgentOptions
            {
                Name = Name,
                ChatOptions = new ChatOptions
                {
                    Instructions = instruction,
                    MaxOutputTokens = 100
                }
            });
    }

    private static string GetFallbackWelcome(ChatContext context) =>
        $"Welcome to the Anagram Word Game! We'll play {context.GameState.TotalRounds} rounds. " +
        "I'll give you a word, and our player will try to find anagrams. Let's begin!";
}
