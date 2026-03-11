namespace AnagramSolver.MAF.Workflow.GroupChat;

public sealed class GroupChatOrchestrator
{
    private readonly List<IGroupChatAgent> _agents;
    private readonly List<GroupMessage> _messageHistory = [];
    private readonly GameState _gameState = new();
    
    private const int MaxMessagesInContext = 10;

    public IReadOnlyList<GroupMessage> MessageHistory => _messageHistory;
    public GameState CurrentGameState => _gameState;
    public bool IsGameComplete => !_gameState.IsGameActive && _gameState.CurrentRound > 0;

    public GroupChatOrchestrator(IEnumerable<IGroupChatAgent> agents)
    {
        _agents = agents?.ToList() ?? throw new ArgumentNullException(nameof(agents));
        
        if (_agents.Count < 2)
            throw new ArgumentException("At least 2 agents required for group chat", nameof(agents));
    }

    public async Task<GroupMessage?> RunNextTurnAsync(CancellationToken cancellationToken = default)
    {
        var context = CreateContext();
        var nextAgent = SelectNextAgent(context);

        if (nextAgent == null)
            return null;

        var response = await nextAgent.GenerateResponseAsync(context, cancellationToken);

        var message = new GroupMessage
        {
            AgentName = nextAgent.Name,
            Content = response.Content,
            Type = response.Type
        };

        _messageHistory.Add(message);
        return message;
    }

    public async IAsyncEnumerable<GroupMessage> RunGameAsync(
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        _gameState.CurrentRound = 0;
        _gameState.IsGameActive = false;
        _gameState.Scores.Clear();
        _gameState.UsedWords.Clear();
        _messageHistory.Clear();

        while (!cancellationToken.IsCancellationRequested)
        {
            var message = await RunNextTurnAsync(cancellationToken);
            
            if (message == null)
                break;

            yield return message;

            if (!_gameState.IsGameActive && _gameState.CurrentRound > _gameState.TotalRounds)
                break;

            await Task.Delay(500, cancellationToken);
        }
    }

    public void SetTotalRounds(int rounds)
    {
        _gameState.TotalRounds = Math.Clamp(rounds, 1, 10);
    }

    private ChatContext CreateContext()
    {
        var recentMessages = _messageHistory
            .TakeLast(MaxMessagesInContext)
            .ToList();

        var lastSpeaker = _messageHistory.LastOrDefault()?.AgentName ?? string.Empty;

        return new ChatContext
        {
            RecentMessages = recentMessages,
            GameState = _gameState,
            LastSpeaker = lastSpeaker
        };
    }

    private IGroupChatAgent? SelectNextAgent(ChatContext context)
    {
        var candidates = _agents
            .Where(a => a.WantsToSpeak(context))
            .Select(a => new { Agent = a, Priority = a.GetSpeakingPriority(context) })
            .OrderByDescending(x => x.Priority)
            .ToList();

        if (candidates.Count == 0)
        {
            if (!_gameState.IsGameActive && _gameState.CurrentRound == 0)
            {
                return _agents.FirstOrDefault(a => a.Name == "GameHost");
            }
            return null;
        }

        return candidates.First().Agent;
    }
}
