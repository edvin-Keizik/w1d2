using AnagramSolver.Mcp;
using System.Text.Json;

var httpClient = new HttpClient();
var apiUrl = Environment.GetEnvironmentVariable("ANAGRAM_API_URL") ?? "http://localhost:5242";
var handler = new AnagramSearchToolHandler(new McpAnagramSearchTool(httpClient, apiUrl));
var processor = new McpRequestProcessor(handler);

Console.Error.WriteLine("[MCP] AnagramSolver MCP Server started - waiting for initialize");

while (true)
{
    string? line = Console.ReadLine();
    if (line is null) break;
    if (!string.IsNullOrWhiteSpace(line))
    {
        var response = await processor.ProcessAsync(line);
        if (!string.IsNullOrEmpty(response))
        {
            Console.WriteLine(response);
            Console.Out.Flush();
        }
    }
}
