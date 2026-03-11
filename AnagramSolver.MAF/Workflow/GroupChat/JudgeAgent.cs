using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace AnagramSolver.MAF.Workflow.GroupChat;

public sealed class JudgeAgent : IGroupChatAgent
{
    private readonly IChatClient _chatClient;

    public string Name => "Judge";
    public string Role => "Judge - evaluates answers and awards points";

    public JudgeAgent(IChatClient chatClient)
    {
        _chatClient = chatClient ?? throw new ArgumentNullException(nameof(chatClient));
    }

    public bool WantsToSpeak(ChatContext context)
    {
        if (!context.GameState.IsGameActive)
            return false;

        if (context.GameState.AwaitingEvaluation)
            return true;

        return false;
    }

    public int GetSpeakingPriority(ChatContext context)
    {
        if (context.GameState.AwaitingEvaluation)
            return 90;

        return 5;
    }

    public async Task<AgentResponse> GenerateResponseAsync(
        ChatContext context,
        CancellationToken cancellationToken = default)
    {
        var challenge = context.GameState.CurrentChallenge;
        var answer = context.GameState.LastAnswer;

        context.GameState.AwaitingEvaluation = false;
        context.GameState.CurrentChallenge = null;
        
        var currentRound = context.GameState.CurrentRound;
        context.GameState.CurrentRound++;

        if (string.IsNullOrEmpty(answer))
        {
            return new AgentResponse
            {
                Content = $"[{Name}]\n" +
                         $"No valid answer was provided for '{challenge}'.\n" +
                         "No points awarded this round.\n" +
                         GetScoreBoard(context),
                Type = MessageType.Evaluation,
                Priority = 90
            };
        }

        var isValid = ValidateAnagram(challenge!, answer);
        var points = CalculatePoints(challenge!, answer, isValid);
        var playerName = "AnagramPlayer";

        if (isValid && points > 0)
        {
            if (!context.GameState.Scores.ContainsKey(playerName))
                context.GameState.Scores[playerName] = 0;
            
            context.GameState.Scores[playerName] += points;
        }

        var evaluation = await GenerateEvaluationAsync(
            challenge!, answer, isValid, points, currentRound, cancellationToken);

        return new AgentResponse
        {
            Content = $"[{Name}]\n{evaluation}\n{GetScoreBoard(context)}",
            Type = MessageType.Evaluation,
            Priority = 90
        };
    }

    private static bool ValidateAnagram(string original, string answer)
    {
        if (string.IsNullOrEmpty(original) || string.IsNullOrEmpty(answer))
            return false;

        var origSorted = string.Concat(original.ToLowerInvariant().OrderBy(c => c));
        var ansSorted = string.Concat(answer.ToLowerInvariant().OrderBy(c => c));

        if (origSorted == ansSorted)
            return true;

        var origChars = original.ToLowerInvariant().ToHashSet();
        var ansChars = answer.ToLowerInvariant().ToHashSet();
        
        return ansChars.IsSubsetOf(origChars);
    }

    private static int CalculatePoints(string original, string answer, bool isValid)
    {
        if (!isValid) return 0;

        var basePoints = 10;
        var lengthBonus = answer.Length >= original.Length ? 5 : 0;
        var exactBonus = answer.Length == original.Length ? 10 : 0;

        return basePoints + lengthBonus + exactBonus;
    }

    private async Task<string> GenerateEvaluationAsync(
        string challenge,
        string answer,
        bool isValid,
        int points,
        int round,
        CancellationToken cancellationToken)
    {
        try
        {
            var agent = new ChatClientAgent(
                _chatClient,
                new ChatClientAgentOptions
                {
                    Name = Name,
                    ChatOptions = new ChatOptions
                    {
                        Instructions = "You are a fair judge. Give brief, encouraging feedback. Be concise.",
                        MaxOutputTokens = 80
                    }
                });

            var session = await agent.CreateSessionAsync(cancellationToken);
            var prompt = isValid
                ? $"Round {round}: Player answered '{answer}' for challenge '{challenge}'. Valid anagram! Award {points} points."
                : $"Round {round}: Player answered '{answer}' for challenge '{challenge}'. Not a valid anagram. No points.";

            var response = await agent.RunAsync(prompt, session, cancellationToken: cancellationToken);
            return response.Text ?? GetFallbackEvaluation(challenge, answer, isValid, points, round);
        }
        catch
        {
            return GetFallbackEvaluation(challenge, answer, isValid, points, round);
        }
    }

    private static string GetFallbackEvaluation(
        string challenge, string answer, bool isValid, int points, int round)
    {
        if (isValid)
        {
            return $"Round {round} Result:\n" +
                   $"Challenge: {challenge} -> Answer: {answer}\n" +
                   $"CORRECT! +{points} points!";
        }
        
        return $"Round {round} Result:\n" +
               $"Challenge: {challenge} -> Answer: {answer}\n" +
               $"Not a valid anagram. No points this round.";
    }

    private static string GetScoreBoard(ChatContext context)
    {
        if (context.GameState.Scores.Count == 0)
            return "\nScoreboard: No scores yet";

        var scores = string.Join(", ", context.GameState.Scores
            .OrderByDescending(s => s.Value)
            .Select(s => $"{s.Key}: {s.Value}"));

        return $"\nScoreboard: {scores}";
    }
}
