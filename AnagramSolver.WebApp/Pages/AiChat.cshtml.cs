using AnagramSolver.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace AnagramSolver.WebApp.Pages
{
    public class AiChatModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AiChatModel> _logger;

        public AiChatModel(IHttpClientFactory httpClientFactory, ILogger<AiChatModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [BindProperty]
        public string Message { get; set; } = string.Empty;

        [BindProperty]
        public string SessionId { get; set; } = string.Empty;

        public void OnGet()
        {
            SessionId = HttpContext.Session.GetString("AiChatSessionId") ?? Guid.NewGuid().ToString();
            HttpContext.Session.SetString("AiChatSessionId", SessionId);

            _logger.LogInformation("Chat page loaded with session: {SessionId}", SessionId);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Message))
                return BadRequest("Message cannot be empty");

            SessionId = HttpContext.Session.GetString("AiChatSessionId") ?? Guid.NewGuid().ToString();
            HttpContext.Session.SetString("AiChatSessionId", SessionId);

            try
            {
                var client = _httpClientFactory.CreateClient();
                var request = new ChatRequest { Message = Message, SessionId = SessionId };

                var response = await client.PostAsJsonAsync("http://localhost:5242/api/ai/chat", request);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var chatResponse = JsonSerializer.Deserialize<ChatResponse>(content);
                    return new JsonResult(chatResponse);
                }

                return BadRequest("Error occurred while processing the message");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message");
                return BadRequest("An error occurred");
            }
        }
    }
}
