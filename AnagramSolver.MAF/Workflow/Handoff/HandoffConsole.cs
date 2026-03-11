using SystemConsole = System.Console;

namespace AnagramSolver.MAF.Workflow.Handoff;

public sealed class HandoffConsole
{
    private readonly HandoffOrchestrator _orchestrator;
    private readonly CancellationToken _cancellationToken;

    private static readonly Dictionary<string, ConsoleColor> AgentColors = new()
    {
        { "Triage Agent", ConsoleColor.Magenta },
        { "Anagram Specialist", ConsoleColor.Cyan },
        { "Word Analysis Specialist", ConsoleColor.Yellow }
    };

    public HandoffConsole(HandoffOrchestrator orchestrator, CancellationToken cancellationToken = default)
    {
        _orchestrator = orchestrator ?? throw new ArgumentNullException(nameof(orchestrator));
        _cancellationToken = cancellationToken;
    }

    public async Task RunAsync()
    {
        PrintWelcome();

        while (!_cancellationToken.IsCancellationRequested)
        {
            try
            {
                PrintCurrentAgent();
                SystemConsole.ForegroundColor = ConsoleColor.White;
                SystemConsole.Write("You: ");
                SystemConsole.ResetColor();

                var input = SystemConsole.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                if (IsExitCommand(input))
                {
                    PrintGoodbye();
                    break;
                }

                if (IsResetCommand(input))
                {
                    _orchestrator.ResetConversation();
                    SystemConsole.ForegroundColor = ConsoleColor.DarkGray;
                    SystemConsole.WriteLine("[Conversation reset. Starting fresh.]\n");
                    SystemConsole.ResetColor();
                    continue;
                }

                SystemConsole.ForegroundColor = ConsoleColor.DarkGray;
                SystemConsole.WriteLine("Processing...\n");
                SystemConsole.ResetColor();

                var response = await _orchestrator.ProcessMessageAsync(input, _cancellationToken);

                DisplayResponse(response);
            }
            catch (OperationCanceledException)
            {
                SystemConsole.WriteLine("\nOperation cancelled.");
                break;
            }
            catch (Exception ex)
            {
                SystemConsole.ForegroundColor = ConsoleColor.Red;
                SystemConsole.WriteLine($"Error: {ex.Message}");
                SystemConsole.ResetColor();
            }
        }
    }

    private void PrintCurrentAgent()
    {
        var agentName = _orchestrator.CurrentAgentName;
        var color = AgentColors.GetValueOrDefault(agentName, ConsoleColor.Gray);
        
        SystemConsole.ForegroundColor = ConsoleColor.DarkGray;
        SystemConsole.Write($"[Current: ");
        SystemConsole.ForegroundColor = color;
        SystemConsole.Write(agentName);
        SystemConsole.ForegroundColor = ConsoleColor.DarkGray;
        SystemConsole.WriteLine("]");
        SystemConsole.ResetColor();
    }

    private static void DisplayResponse(HandoffResponse response)
    {
        var color = AgentColors.GetValueOrDefault(response.HandledBy, ConsoleColor.Gray);

        if (response.WasHandoff)
        {
            SystemConsole.ForegroundColor = ConsoleColor.DarkYellow;
            SystemConsole.WriteLine($">> Handoff occurred: {response.RoutingReason}");
            SystemConsole.ResetColor();
            SystemConsole.WriteLine();
        }

        SystemConsole.ForegroundColor = color;
        SystemConsole.Write($"[{response.HandledBy}]: ");
        SystemConsole.ResetColor();
        
        var responseLines = response.Response.Split('\n');
        var firstLine = true;
        foreach (var line in responseLines)
        {
            if (firstLine && line.StartsWith("[Triage"))
            {
                firstLine = false;
                continue;
            }
            SystemConsole.WriteLine(line);
            firstLine = false;
        }

        if (!string.IsNullOrEmpty(response.SuggestedFollowUp))
        {
            SystemConsole.ForegroundColor = ConsoleColor.DarkGray;
            SystemConsole.WriteLine($"\nTip: {response.SuggestedFollowUp}");
            SystemConsole.ResetColor();
        }

        SystemConsole.WriteLine();
    }

    private static bool IsExitCommand(string input)
    {
        var commands = new[] { "exit", "quit", "bye", "q", "back", "menu" };
        return commands.Contains(input.ToLowerInvariant());
    }

    private static bool IsResetCommand(string input)
    {
        var commands = new[] { "reset", "clear", "new", "restart" };
        return commands.Contains(input.ToLowerInvariant());
    }

    private static void PrintWelcome()
    {
        SystemConsole.Clear();
        SystemConsole.ForegroundColor = ConsoleColor.Green;
        SystemConsole.WriteLine("+==============================================================+");
        SystemConsole.WriteLine("|              HANDOFF WORKFLOW - MULTI-AGENT CHAT            |");
        SystemConsole.WriteLine("+==============================================================+");
        SystemConsole.WriteLine("|                                                              |");
        SystemConsole.WriteLine("|  Agents:                                                     |");
        SystemConsole.WriteLine("|    * Triage Agent      - Routes your requests               |");
        SystemConsole.WriteLine("|    * Anagram Specialist    - Finds anagrams                 |");
        SystemConsole.WriteLine("|    * Word Analysis Specialist - Analyzes words              |");
        SystemConsole.WriteLine("|                                                              |");
        SystemConsole.WriteLine("|  Commands:                                                   |");
        SystemConsole.WriteLine("|    * 'reset' - Start new conversation                       |");
        SystemConsole.WriteLine("|    * 'exit'  - Return to main menu                          |");
        SystemConsole.WriteLine("|                                                              |");
        SystemConsole.WriteLine("|  Try asking:                                                 |");
        SystemConsole.WriteLine("|    'Find anagrams for katas'                                |");
        SystemConsole.WriteLine("|    'Analyze the word programming'                           |");
        SystemConsole.WriteLine("|    'How many letters in elephant?'                          |");
        SystemConsole.WriteLine("|                                                              |");
        SystemConsole.WriteLine("+==============================================================+");
        SystemConsole.ResetColor();
        SystemConsole.WriteLine();
    }

    private static void PrintGoodbye()
    {
        SystemConsole.ForegroundColor = ConsoleColor.Green;
        SystemConsole.WriteLine("\nExiting Handoff Workflow mode...");
        SystemConsole.ResetColor();
    }
}
