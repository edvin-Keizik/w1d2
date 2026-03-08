namespace AnagramSolver.Mcp
{
    /// <summary>
    /// MCP Tool for searching anagrams using the AnagramSolver API endpoint.
    /// This tool calls the /api/anagrams/{word} endpoint to find anagrams for a given word.
    /// </summary>
    public class McpAnagramSearchTool
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public McpAnagramSearchTool(HttpClient httpClient, string apiBaseUrl = "http://localhost:5000")
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiBaseUrl = apiBaseUrl ?? throw new ArgumentNullException(nameof(apiBaseUrl));
        }

        /// <summary>
        /// Searches for anagrams of a given word by calling the AnagramSolver API.
        /// </summary>
        /// <param name="word">The word to find anagrams for.</param>
        /// <param name="cancellationToken">Cancellation token for async operations.</param>
        /// <returns>A list of anagrams found for the given word.</returns>
        public async Task<IEnumerable<string>> SearchAnagramsAsync(
            string word,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                throw new ArgumentException("Word cannot be null or empty.", nameof(word));
            }

            try
            {
                // Build the API endpoint URL
                var url = $"{_apiBaseUrl.TrimEnd('/')}/api/anagrams/{Uri.EscapeDataString(word)}";

                // Call the API endpoint
                var response = await _httpClient.GetAsync(url, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(
                        $"AnagramSolver API returned status {response.StatusCode}: {response.ReasonPhrase}");
                }

                // Read the response content
                var content = await response.Content.ReadAsStringAsync(cancellationToken);

                // Parse the JSON response
                var anagrams = System.Text.Json.JsonSerializer.Deserialize<List<string>>(content)
                    ?? new List<string>();

                // Extract custom headers if available (for diagnostics)
                if (response.Headers.TryGetValues("X-Anagram-Count", out var countValues))
                {
                    var count = countValues.FirstOrDefault();
                    System.Diagnostics.Debug.WriteLine($"Found {count} anagrams for '{word}'");
                }

                if (response.Headers.TryGetValues("X-Search-Duration-Ms", out var durationValues))
                {
                    var duration = durationValues.FirstOrDefault();
                    System.Diagnostics.Debug.WriteLine($"Search took {duration}ms");
                }

                return anagrams;
            }
            catch (HttpRequestException ex)
            {
                // Log network errors
                System.Diagnostics.Debug.WriteLine($"Network error calling AnagramSolver API: {ex.Message}");
                throw;
            }
            catch (OperationCanceledException)
            {
                System.Diagnostics.Debug.WriteLine("Anagram search operation was cancelled");
                throw;
            }
        }

        /// <summary>
        /// Gets the API base URL currently configured.
        /// </summary>
        public string ApiBaseUrl => _apiBaseUrl;
    }
}
