using AnagramSolver.WebApp.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using System.IO;

namespace AnagramSolver.WebApp.Services
{
    public class AiChatService : IAiChatService
    {
        private readonly Kernel _kernel;
        private readonly ILogger<AiChatService> _logger;
        private readonly IChatHistoryService _chatHistoryService;
        private readonly string _systemPromptPath;
        private const int MaxAutoFunctionCallIterations = 10;

        public AiChatService(
            Kernel kernel,
            ILogger<AiChatService> logger,
            IChatHistoryService chatHistoryService,
            IWebHostEnvironment environment)
        {
            _kernel = kernel ?? throw new ArgumentNullException(nameof(kernel));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _chatHistoryService = chatHistoryService ?? throw new ArgumentNullException(nameof(chatHistoryService));
            _systemPromptPath = System.IO.Path.Combine(environment.ContentRootPath, "Prompts", "AnagramAgentRole.md");
        }

        public async Task<ChatResponse> GetResponseAsync(ChatRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(request.Message))
                throw new ArgumentException("Message cannot be empty.", nameof(request));

            try
            {
                var systemPrompt = await LoadSystemPromptAsync(cancellationToken);
                var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();

                string sessionId = request.SessionId ?? Guid.NewGuid().ToString();

                _logger.LogInformation("Processing chat for session: {SessionId}", sessionId);

                var chatHistory = _chatHistoryService.GetOrCreateHistory(sessionId);

                if (chatHistory.Count == 0)
                {
                    chatHistory.AddSystemMessage(systemPrompt);
                    _logger.LogDebug("Added system prompt to new session: {SessionId}", sessionId);
                }

                chatHistory.AddUserMessage(request.Message);
                _chatHistoryService.AddUserMessage(sessionId, request.Message);

                var settings = new PromptExecutionSettings
                {
                    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
                };

                string finalResponse = string.Empty;
                int iterations = 0;

                while (iterations < MaxAutoFunctionCallIterations)
                {
                    iterations++;
                    _logger.LogDebug("Chat completion iteration {Iteration} for session: {SessionId}", iterations, sessionId);

                    var response = await chatCompletionService.GetChatMessageContentAsync(
                        chatHistory,
                        settings,
                        _kernel,
                        cancellationToken: cancellationToken);

                    if (!string.IsNullOrWhiteSpace(response.Content))
                    {
                        finalResponse = response.Content;
                        chatHistory.AddAssistantMessage(response.Content);
                        _chatHistoryService.AddAssistantMessage(sessionId, response.Content);
                        break;
                    }

                    if (response.Content == null && iterations >= MaxAutoFunctionCallIterations)
                    {
                        _logger.LogWarning("Maximum auto function call iterations ({MaxIterations}) reached for session: {SessionId}", 
                            MaxAutoFunctionCallIterations, sessionId);
                        finalResponse = "Unable to complete the request after multiple iterations.";
                        chatHistory.AddAssistantMessage(finalResponse);
                        _chatHistoryService.AddAssistantMessage(sessionId, finalResponse);
                        break;
                    }

                    chatHistory.AddAssistantMessage(response.Content ?? "");
                }

                return new ChatResponse 
                { 
                    Response = finalResponse ?? string.Empty,
                    SessionId = sessionId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting AI response for session: {SessionId}", request.SessionId);
                throw;
            }
        }

        private async Task<string> LoadSystemPromptAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (File.Exists(_systemPromptPath))
                {
                    return await File.ReadAllTextAsync(_systemPromptPath, cancellationToken);
                }

                _logger.LogWarning("System prompt file not found at {Path}", _systemPromptPath);
                return "You are a helpful AI assistant for the AnagramSolver application.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading system prompt from file");
                return "You are a helpful AI assistant for the AnagramSolver application.";
            }
        }
    }
}
