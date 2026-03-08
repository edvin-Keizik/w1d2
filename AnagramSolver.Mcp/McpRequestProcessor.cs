namespace AnagramSolver.Mcp;

using System.Text.Json;

public class McpRequestProcessor
{
    private readonly AnagramSearchToolHandler _toolHandler;

    public McpRequestProcessor(AnagramSearchToolHandler toolHandler)
    {
        _toolHandler = toolHandler;
    }

    public async Task<string> ProcessAsync(string jsonRequest)
    {
        try
        {
            using var doc = JsonDocument.Parse(jsonRequest);
            var root = doc.RootElement;

            var jsonrpc = root.TryGetProperty("jsonrpc", out var jsonRpcProp) ? jsonRpcProp.GetString() : null;
            var method = root.TryGetProperty("method", out var methodProp) ? methodProp.GetString() : null;
            object? id = null;
            if (root.TryGetProperty("id", out var idProp))
            {
                id = idProp.ValueKind switch
                {
                    JsonValueKind.String => idProp.GetString(),
                    JsonValueKind.Number => idProp.GetInt64(),
                    _ => idProp.GetRawText()
                };
            }

            // Handle initialize request
            if (method == "initialize")
            {
                var toolDef = AnagramSearchToolHandler.GetToolDefinition();
                return JsonSerializer.Serialize(new
                {
                    jsonrpc = "2.0",
                    id,
                    result = new
                    {
                        protocolVersion = "2024-11-05",
                        serverInfo = new
                        {
                            name = "AnagramSolver",
                            version = "1.0.0"
                        },
                        capabilities = new
                        {
                            tools = new { listChanged = false }
                        }
                    }
                });
            }

            // Handle list_tools request
            if (method == "tools/list")
            {
                var toolDef = AnagramSearchToolHandler.GetToolDefinition();
                var response = new
                {
                    jsonrpc = "2.0",
                    id,
                    result = new
                    {
                        tools = new object[] { toolDef }
                    }
                };
                var json = JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = false });
                Console.Error.WriteLine($"[MCP] tools/list response: {json}");
                return json;
            }

            // Handle call_tool request
            if (method == "tools/call")
            {
                var paramsObj = root.GetProperty("params");
                var toolName = paramsObj.TryGetProperty("name", out var nameProp) ? nameProp.GetString() : null;
                var hasArguments = paramsObj.TryGetProperty("arguments", out var arguments);

                if (toolName == "search_anagrams")
                {
                    var word = hasArguments && arguments.TryGetProperty("word", out var wordProp) ? wordProp.GetString() : null;

                    if (string.IsNullOrWhiteSpace(word))
                    {
                        return JsonSerializer.Serialize(new
                        {
                            jsonrpc = "2.0",
                            id,
                            error = new
                            {
                                code = -32602,
                                message = "Invalid params: missing word"
                            }
                        });
                    }

                    var result = await _toolHandler.HandleSearchAnagramsAsync(word);
                    return JsonSerializer.Serialize(new
                    {
                        jsonrpc = "2.0",
                        id,
                        result = new
                        {
                            content = new[]
                            {
                                new
                                {
                                    type = "text",
                                    text = $"Found {result.AnagramCount} anagrams for '{result.Word}' in {result.SearchDurationMs}ms: {string.Join(", ", result.Anagrams.Take(10))}{(result.AnagramCount > 10 ? "..." : "")}"
                                }
                            }
                        }
                    });
                }

                return JsonSerializer.Serialize(new
                {
                    jsonrpc = "2.0",
                    id,
                    error = new
                    {
                        code = -32601,
                        message = $"Method not found: {toolName}"
                    }
                });
            }

            // Handle notifications (no response expected)
            if (method == "notifications/initialized")
            {
                Console.Error.WriteLine("[MCP] Client initialized");
                return null!;
            }

            return JsonSerializer.Serialize(new
            {
                jsonrpc = "2.0",
                id,
                error = new
                {
                    code = -32601,
                    message = $"Method not found: {method}"
                }
            });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[MCP ERROR] {ex}");
            return JsonSerializer.Serialize(new
            {
                jsonrpc = "2.0",
                error = new
                {
                    code = -32603,
                    message = $"Internal error: {ex.Message}"
                }
            });
        }
    }
}
