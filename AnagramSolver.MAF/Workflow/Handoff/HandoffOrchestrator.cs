namespace AnagramSolver.MAF.Workflow.Handoff;

public sealed class HandoffOrchestrator
{
    private readonly TriageAgent _triageAgent;
    private readonly Dictionary<SpecialistType, ISpecialistAgent> _specialists;
    
    private SpecialistType _currentAgent = SpecialistType.None;
    private readonly List<ConversationTurn> _conversationHistory = [];

    public IReadOnlyList<ConversationTurn> ConversationHistory => _conversationHistory;
    public string CurrentAgentName => GetAgentName(_currentAgent);

    public HandoffOrchestrator(TriageAgent triageAgent, IEnumerable<ISpecialistAgent> specialists)
    {
        _triageAgent = triageAgent ?? throw new ArgumentNullException(nameof(triageAgent));
        _specialists = specialists?.ToDictionary(s => s.Type) 
            ?? throw new ArgumentNullException(nameof(specialists));
    }

    public async Task<HandoffResponse> ProcessMessageAsync(
        string userMessage,
        CancellationToken cancellationToken = default)
    {
        _conversationHistory.Add(new ConversationTurn
        {
            Role = "User",
            Message = userMessage,
            Timestamp = DateTime.UtcNow
        });

        if (_currentAgent != SpecialistType.None && 
            _specialists.TryGetValue(_currentAgent, out var currentSpecialist))
        {
            var specialistResult = await currentSpecialist.HandleRequestAsync(
                userMessage, null, cancellationToken);

            if (specialistResult.ShouldReturnToTriage)
            {
                _currentAgent = SpecialistType.None;
                return await RouteToTriageAsync(userMessage, cancellationToken);
            }

            AddAgentResponse(currentSpecialist.Name, specialistResult.Response);
            
            return new HandoffResponse
            {
                Response = specialistResult.Response,
                HandledBy = currentSpecialist.Name,
                WasHandoff = false,
                SuggestedFollowUp = specialistResult.SuggestedFollowUp
            };
        }

        return await RouteToTriageAsync(userMessage, cancellationToken);
    }

    private async Task<HandoffResponse> RouteToTriageAsync(
        string userMessage,
        CancellationToken cancellationToken)
    {
        var triageResult = await _triageAgent.ClassifyIntentAsync(userMessage, cancellationToken);

        if (triageResult.IsGeneralConversation || triageResult.RecommendedSpecialist == SpecialistType.None)
        {
            var response = await _triageAgent.HandleGeneralQueryAsync(userMessage, cancellationToken);
            AddAgentResponse("Triage Agent", response);
            
            return new HandoffResponse
            {
                Response = response,
                HandledBy = "Triage Agent",
                WasHandoff = false,
                RoutingReason = triageResult.Reasoning
            };
        }

        if (_specialists.TryGetValue(triageResult.RecommendedSpecialist, out var specialist))
        {
            _currentAgent = triageResult.RecommendedSpecialist;

            var handoffMessage = $"[Triage -> {specialist.Name}] {triageResult.Reasoning}";
            AddAgentResponse("Triage Agent", handoffMessage);

            var result = await specialist.HandleRequestAsync(
                userMessage,
                triageResult.ExtractedQuery,
                cancellationToken);

            if (result.ShouldReturnToTriage)
            {
                _currentAgent = SpecialistType.None;
                return await RouteToTriageAsync(userMessage, cancellationToken);
            }

            AddAgentResponse(specialist.Name, result.Response);

            return new HandoffResponse
            {
                Response = $"{handoffMessage}\n\n{result.Response}",
                HandledBy = specialist.Name,
                WasHandoff = true,
                RoutingReason = triageResult.Reasoning,
                SuggestedFollowUp = result.SuggestedFollowUp
            };
        }

        var fallbackResponse = await _triageAgent.HandleGeneralQueryAsync(userMessage, cancellationToken);
        AddAgentResponse("Triage Agent", fallbackResponse);
        
        return new HandoffResponse
        {
            Response = fallbackResponse,
            HandledBy = "Triage Agent",
            WasHandoff = false
        };
    }

    public void ResetConversation()
    {
        _currentAgent = SpecialistType.None;
        _conversationHistory.Clear();
    }

    private void AddAgentResponse(string agentName, string response)
    {
        _conversationHistory.Add(new ConversationTurn
        {
            Role = agentName,
            Message = response,
            Timestamp = DateTime.UtcNow
        });
    }

    private static string GetAgentName(SpecialistType type) => type switch
    {
        SpecialistType.Anagram => "Anagram Specialist",
        SpecialistType.WordAnalysis => "Word Analysis Specialist",
        _ => "Triage Agent"
    };
}

public sealed class HandoffResponse
{
    public required string Response { get; init; }
    public required string HandledBy { get; init; }
    public bool WasHandoff { get; init; }
    public string? RoutingReason { get; init; }
    public string? SuggestedFollowUp { get; init; }
}

public sealed class ConversationTurn
{
    public required string Role { get; init; }
    public required string Message { get; init; }
    public DateTime Timestamp { get; init; }
}
