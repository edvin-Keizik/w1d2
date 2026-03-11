namespace AnagramSolver.MAF.Workflow.GroupChat;

public sealed class GroupMessage
{
    public required string AgentName { get; init; }
    public required string Content { get; init; }
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
    public MessageType Type { get; init; } = MessageType.Normal;
}

public enum MessageType
{
    Normal,
    Challenge,
    Answer,
    Evaluation,
    Hint,
    SystemAnnouncement
}

public sealed class GameState
{
    public int CurrentRound { get; set; } = 0;
    public int TotalRounds { get; set; } = 3;
    public string? CurrentChallenge { get; set; }
    public Dictionary<string, int> Scores { get; } = new();
    public bool IsGameActive { get; set; } = false;
    public List<string> UsedWords { get; } = [];
    public string? LastAnswer { get; set; }
    public bool AwaitingAnswer { get; set; } = false;
    public bool AwaitingEvaluation { get; set; } = false;
}

public sealed class ChatContext
{
    public required IReadOnlyList<GroupMessage> RecentMessages { get; init; }
    public required GameState GameState { get; init; }
    public required string LastSpeaker { get; init; }
}

public sealed class AgentResponse
{
    public required string Content { get; init; }
    public MessageType Type { get; init; } = MessageType.Normal;
    public bool WantsToSpeak { get; init; } = true;
    public int Priority { get; init; } = 0;
}

public interface IGroupChatAgent
{
    string Name { get; }
    string Role { get; }
    
    bool WantsToSpeak(ChatContext context);
    
    int GetSpeakingPriority(ChatContext context);
    
    Task<AgentResponse> GenerateResponseAsync(
        ChatContext context,
        CancellationToken cancellationToken = default);
}
