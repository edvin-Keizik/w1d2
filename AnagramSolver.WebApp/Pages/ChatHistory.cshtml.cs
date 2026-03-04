using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnagramSolver.WebApp.Pages
{
    public class ChatHistoryModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ChatHistoryModel> _logger;

        public ChatHistoryModel(IHttpClientFactory httpClientFactory, ILogger<ChatHistoryModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public void OnGet()
        {
            _logger.LogInformation("Chat history page loaded");
        }
    }
}
