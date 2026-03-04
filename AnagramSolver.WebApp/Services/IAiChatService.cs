using AnagramSolver.WebApp.Models;

namespace AnagramSolver.WebApp.Services
{
    public interface IAiChatService
    {
        Task<ChatResponse> GetResponseAsync(ChatRequest request, CancellationToken cancellationToken = default);
    }
}
