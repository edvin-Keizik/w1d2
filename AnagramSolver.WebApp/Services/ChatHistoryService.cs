using Microsoft.SemanticKernel.ChatCompletion;
using System.Collections.Concurrent;

namespace AnagramSolver.WebApp.Services
{
    public class ChatHistoryService : IChatHistoryService
    {
        private readonly ConcurrentDictionary<string, ChatHistory> _histories = new();
        private readonly ILogger<ChatHistoryService> _logger;

        public ChatHistoryService(ILogger<ChatHistoryService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public ChatHistory GetOrCreateHistory(string sessionId)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
                throw new ArgumentException("Session ID cannot be empty", nameof(sessionId));

            return _histories.GetOrAdd(sessionId, _ =>
            {
                _logger.LogInformation("Creating new chat history for session: {SessionId}", sessionId);
                return new ChatHistory();
            });
        }

        public void AddUserMessage(string sessionId, string message)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
                throw new ArgumentException("Session ID cannot be empty", nameof(sessionId));

            var history = GetOrCreateHistory(sessionId);
            history.AddUserMessage(message);

            _logger.LogDebug("Added user message to session {SessionId}. Message length: {Length}", 
                sessionId, message.Length);
        }

        public void AddAssistantMessage(string sessionId, string message)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
                throw new ArgumentException("Session ID cannot be empty", nameof(sessionId));

            var history = GetOrCreateHistory(sessionId);
            history.AddAssistantMessage(message);

            _logger.LogDebug("Added assistant message to session {SessionId}. Message length: {Length}", 
                sessionId, message.Length);
        }

        public ChatHistory? GetHistory(string sessionId)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
                return null;

            _histories.TryGetValue(sessionId, out var history);
            return history;
        }

        public List<ChatMessageItem> GetSessionMessages(string sessionId)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
                return new List<ChatMessageItem>();

            var history = GetHistory(sessionId);
            if (history == null)
                return new List<ChatMessageItem>();

            var messages = new List<ChatMessageItem>();
            foreach (var message in history)
            {
                if (message.Role != AuthorRole.System)
                {
                    messages.Add(new ChatMessageItem
                    {
                        Role = message.Role.ToString(),
                        Content = message.Content
                    });
                }
            }
            return messages;
        }

        public void ClearHistory(string sessionId)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
                return;

            if (_histories.TryRemove(sessionId, out _))
            {
                _logger.LogInformation("Cleared chat history for session: {SessionId}", sessionId);
            }
        }

        public IEnumerable<string> GetActiveSessions()
        {
            return _histories.Keys;
        }
    }

    public class ChatMessageDto
    {
        public required string Role { get; set; }
        public required string Content { get; set; }
        public long Timestamp { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    public class ChatHistoryDto
    {
        public required string SessionId { get; set; }
        public required List<ChatMessageDto> Messages { get; set; }
        public int MessageCount => Messages.Count;
        public long CreatedAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
}
