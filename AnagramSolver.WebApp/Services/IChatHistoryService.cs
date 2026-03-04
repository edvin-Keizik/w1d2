using Microsoft.SemanticKernel.ChatCompletion;

namespace AnagramSolver.WebApp.Services
{
    public class ChatMessageItem
    {
        public required string Role { get; set; }
        public required string Content { get; set; }
    }

    public interface IChatHistoryService
    {
        ChatHistory GetOrCreateHistory(string sessionId);
        void AddUserMessage(string sessionId, string message);
        void AddAssistantMessage(string sessionId, string message);
        ChatHistory? GetHistory(string sessionId);
        List<ChatMessageItem> GetSessionMessages(string sessionId);
        void ClearHistory(string sessionId);
        IEnumerable<string> GetActiveSessions();
    }
}
