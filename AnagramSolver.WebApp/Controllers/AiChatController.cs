using AnagramSolver.WebApp.Models;
using AnagramSolver.WebApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AnagramSolver.WebApp.Controllers
{
    [ApiController]
    [Route("api/ai")]
    public class AiChatController : ControllerBase
    {
        private readonly IAiChatService _aiChatService;
        private readonly IChatHistoryService _chatHistoryService;
        private readonly ILogger<AiChatController> _logger;

        public AiChatController(
            IAiChatService aiChatService,
            IChatHistoryService chatHistoryService,
            ILogger<AiChatController> logger)
        {
            _aiChatService = aiChatService ?? throw new ArgumentNullException(nameof(aiChatService));
            _chatHistoryService = chatHistoryService ?? throw new ArgumentNullException(nameof(chatHistoryService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("chat")]
        public async Task<ActionResult<ChatResponse>> Chat(
            [FromBody] ChatRequest request,
            CancellationToken cancellationToken)
        {
            if (request == null)
            {
                _logger.LogWarning("Chat request was null");
                return BadRequest("Chat request cannot be null");
            }

            if (string.IsNullOrWhiteSpace(request.Message))
            {
                _logger.LogWarning("Chat message was empty");
                return BadRequest("Message cannot be empty");
            }

            try
            {
                _logger.LogInformation("Processing chat request with session: {SessionId}", request.SessionId ?? "new");

                var response = await _aiChatService.GetResponseAsync(request, cancellationToken);

                _logger.LogInformation("Chat request processed successfully for session: {SessionId}", response.SessionId);

                return Ok(response);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Chat request was cancelled for session: {SessionId}", request.SessionId ?? "unknown");
                return StatusCode(StatusCodes.Status408RequestTimeout, "Request was cancelled");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing chat request for session: {SessionId}", request.SessionId ?? "unknown");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request");
            }
        }

        [HttpGet("chat/{sessionId}/history")]
        public ActionResult<ChatHistoryDto> GetChatHistory(string sessionId)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                _logger.LogWarning("GetChatHistory called with empty session ID");
                return BadRequest("Session ID cannot be empty");
            }

            try
            {
                _logger.LogInformation("Retrieving chat history for session: {SessionId}", sessionId);

                var messages = _chatHistoryService.GetSessionMessages(sessionId);

                if (messages.Count == 0)
                {
                    _logger.LogInformation("No history found for session: {SessionId}", sessionId);
                    return NotFound(new { message = $"No chat history found for session: {sessionId}" });
                }

                var messagesDtos = messages.Select(m => new ChatMessageDto
                {
                    Role = m.Role,
                    Content = m.Content
                }).ToList();

                var historyDto = new ChatHistoryDto
                {
                    SessionId = sessionId,
                    Messages = messagesDtos
                };

                _logger.LogInformation("Chat history retrieved for session: {SessionId}. Message count: {Count}", 
                    sessionId, messages.Count);

                return Ok(historyDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving chat history for session: {SessionId}", sessionId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving chat history");
            }
        }

        [HttpGet("chat/sessions/active")]
        public ActionResult<ActiveSessionsDto> GetActiveSessions()
        {
            try
            {
                _logger.LogInformation("Retrieving active sessions");

                var sessions = _chatHistoryService.GetActiveSessions().ToList();

                var dto = new ActiveSessionsDto
                {
                    Sessions = sessions,
                    Count = sessions.Count
                };

                _logger.LogInformation("Retrieved {Count} active sessions", sessions.Count);

                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active sessions");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving active sessions");
            }
        }

        [HttpDelete("chat/{sessionId}/history")]
        public ActionResult<ClearHistoryResponse> ClearHistory(string sessionId)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                _logger.LogWarning("ClearHistory called with empty session ID");
                return BadRequest("Session ID cannot be empty");
            }

            try
            {
                _logger.LogInformation("Clearing chat history for session: {SessionId}", sessionId);

                var history = _chatHistoryService.GetHistory(sessionId);
                if (history == null)
                {
                    _logger.LogInformation("No history found to clear for session: {SessionId}", sessionId);
                    return NotFound(new { message = $"No chat history found for session: {sessionId}" });
                }

                _chatHistoryService.ClearHistory(sessionId);

                _logger.LogInformation("Chat history cleared for session: {SessionId}", sessionId);

                return Ok(new ClearHistoryResponse
                {
                    Success = true,
                    Message = $"Chat history cleared for session: {sessionId}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing chat history for session: {SessionId}", sessionId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while clearing chat history");
            }
        }
    }


    public class ActiveSessionsDto
    {
        public required List<string> Sessions { get; set; }
        public int Count { get; set; }
    }

    public class ClearHistoryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
