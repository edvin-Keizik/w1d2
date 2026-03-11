using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using AnagramSolver.MAF.Tools;

namespace AnagramSolver.MAF.Console;
public sealed class AnagramAgentConsole
{
    private readonly IChatClient _chatClient;
    private readonly IList<AIFunction> _tools;
    private readonly CancellationToken _cancellationToken;

    private const string AgentName = "AnagramAssistant";
    private const string AgentInstructions = """
        You are an intelligent Anagram Assistant that helps users find anagrams and explore word dictionaries.

        Your capabilities include:
        1. **SearchAnagrams**: Find all anagrams (word rearrangements) for given letters
        2. **GetWordCount**: Report how many words are in the dictionary
        3. **FilterByLength**: Find words of a specific length

        Guidelines:
        - Always use the appropriate tool when the user asks about anagrams, word counts, or word lengths
        - Explain your findings in a clear, friendly manner
        - If no anagrams are found, suggest trying different letters or shorter minimum word lengths
        - The dictionary contains Lithuanian words, so results will be in Lithuanian
        - Be helpful and provide context about the results

        When presenting anagram results:
        - List them clearly
        - Mention the total count
        - If there are many results, highlight interesting ones

        You can handle follow-up questions and maintain conversation context.
        """;

    public AnagramAgentConsole(
        IChatClient chatClient,
        IList<AIFunction> tools,
        CancellationToken cancellationToken = default)
    {
        _chatClient = chatClient ?? throw new ArgumentNullException(nameof(chatClient));
        _tools = tools ?? throw new ArgumentNullException(nameof(tools));
        _cancellationToken = cancellationToken;
    }
    public async Task RunAsync()
    {
        PrintWelcome();

        var agent = CreateAgent();

        var session = await agent.CreateSessionAsync(_cancellationToken);

        while (!_cancellationToken.IsCancellationRequested)
        {
            try
            {
                System.Console.ForegroundColor = ConsoleColor.Cyan;
                System.Console.Write("\nYou: ");
                System.Console.ResetColor();

                var userInput = System.Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userInput))
                {
                    continue;
                }

                if (IsExitCommand(userInput))
                {
                    PrintGoodbye();
                    break;
                }

                System.Console.ForegroundColor = ConsoleColor.DarkGray;
                System.Console.Write("Thinking");

                var response = await agent.RunAsync(
                    userInput,
                    session,
                    cancellationToken: _cancellationToken);

                System.Console.Write("\r" + new string(' ', 20) + "\r");

                System.Console.ForegroundColor = ConsoleColor.Green;
                System.Console.Write("Agent: ");
                System.Console.ResetColor();
                System.Console.WriteLine(response.Text ?? "I couldn't generate a response.");
            }
            catch (OperationCanceledException)
            {
                System.Console.WriteLine("\nOperation cancelled.");
                break;
            }
            catch (Exception ex)
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine($"\nError: {ex.Message}");
                System.Console.ResetColor();
                System.Console.WriteLine("Please try again or type 'exit' to quit.");
            }
        }
    }

    private ChatClientAgent CreateAgent()
    {
        var chatClientWithTools = new FunctionInvokingChatClient(_chatClient);

        var toolsList = _tools.Cast<AITool>().ToList();

        return new ChatClientAgent(
            chatClientWithTools,
            new ChatClientAgentOptions
            {
                Name = AgentName,
                ChatOptions = new ChatOptions
                {
                    Instructions = AgentInstructions,
                    Tools = toolsList
                }
            });
    }

    private static bool IsExitCommand(string input)
    {
        var exitCommands = new[] { "exit", "quit", "bye", "q", "išeiti", "baigti" };
        return exitCommands.Contains(input.Trim().ToLowerInvariant());
    }

    private static void PrintWelcome()
    {
        System.Console.Clear();
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine("+==============================================================+");
        System.Console.WriteLine("|            ANAGRAM SOLVER AI ASSISTANT                       |");
        System.Console.WriteLine("|          Microsoft Agent Framework Demo                      |");
        System.Console.WriteLine("+==============================================================+");
        System.Console.WriteLine("|  Available commands:                                         |");
        System.Console.WriteLine("|  * Ask for anagrams: 'Find anagrams for katas'               |");
        System.Console.WriteLine("|  * Check word count: 'How many words in the dictionary?'     |");
        System.Console.WriteLine("|  * Filter by length: 'Show me 5-letter words'                |");
        System.Console.WriteLine("|  * Type 'exit' or 'quit' to end the conversation             |");
        System.Console.WriteLine("+==============================================================+");
        System.Console.ResetColor();
        System.Console.WriteLine();
        System.Console.WriteLine("The agent will automatically use tools when needed.");
        System.Console.WriteLine("Start typing your questions below:\n");
    }

    private static void PrintGoodbye()
    {
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine("\n+==============================================================+");
        System.Console.WriteLine("|  Thank you for using the Anagram Solver AI Assistant!        |");
        System.Console.WriteLine("|  Goodbye!                                                    |");
        System.Console.WriteLine("+==============================================================+");
        System.Console.ResetColor();
    }
}
