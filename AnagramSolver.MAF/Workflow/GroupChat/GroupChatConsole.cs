using SystemConsole = System.Console;

namespace AnagramSolver.MAF.Workflow.GroupChat;

public sealed class GroupChatConsole
{
    private readonly GroupChatOrchestrator _orchestrator;
    private readonly CancellationToken _cancellationToken;

    private static readonly Dictionary<string, ConsoleColor> AgentColors = new()
    {
        { "GameHost", ConsoleColor.Yellow },
        { "AnagramPlayer", ConsoleColor.Cyan },
        { "WordExpert", ConsoleColor.Green },
        { "Judge", ConsoleColor.Magenta }
    };

    public GroupChatConsole(GroupChatOrchestrator orchestrator, CancellationToken cancellationToken = default)
    {
        _orchestrator = orchestrator ?? throw new ArgumentNullException(nameof(orchestrator));
        _cancellationToken = cancellationToken;
    }

    public async Task RunAsync()
    {
        PrintWelcome();

        while (!_cancellationToken.IsCancellationRequested)
        {
            SystemConsole.ForegroundColor = ConsoleColor.White;
            SystemConsole.Write("\nPress ENTER to start a game, or type 'exit' to quit: ");
            SystemConsole.ResetColor();

            var input = SystemConsole.ReadLine()?.Trim().ToLowerInvariant();

            if (input == "exit" || input == "quit" || input == "q" || input == "back")
            {
                PrintGoodbye();
                break;
            }

            SystemConsole.Write("How many rounds? (1-5, default 3): ");
            var roundsInput = SystemConsole.ReadLine()?.Trim();
            var rounds = 3;
            if (int.TryParse(roundsInput, out var parsedRounds))
                rounds = Math.Clamp(parsedRounds, 1, 5);

            _orchestrator.SetTotalRounds(rounds);

            SystemConsole.WriteLine();
            SystemConsole.ForegroundColor = ConsoleColor.DarkGray;
            SystemConsole.WriteLine("=".PadRight(60, '='));
            SystemConsole.WriteLine("       ANAGRAM WORD GAME - GROUP CHAT SESSION");
            SystemConsole.WriteLine("=".PadRight(60, '='));
            SystemConsole.ResetColor();
            SystemConsole.WriteLine();

            try
            {
                await foreach (var message in _orchestrator.RunGameAsync(_cancellationToken))
                {
                    DisplayMessage(message);
                    
                    if (_orchestrator.IsGameComplete)
                        break;
                }

                SystemConsole.WriteLine();
                SystemConsole.ForegroundColor = ConsoleColor.DarkGray;
                SystemConsole.WriteLine("=".PadRight(60, '='));
                SystemConsole.WriteLine("              GAME SESSION ENDED");
                SystemConsole.WriteLine("=".PadRight(60, '='));
                SystemConsole.ResetColor();
            }
            catch (OperationCanceledException)
            {
                SystemConsole.WriteLine("\nGame interrupted.");
            }
            catch (Exception ex)
            {
                SystemConsole.ForegroundColor = ConsoleColor.Red;
                SystemConsole.WriteLine($"\nError during game: {ex.Message}");
                SystemConsole.ResetColor();
            }
        }
    }

    private static void DisplayMessage(GroupMessage message)
    {
        var color = AgentColors.GetValueOrDefault(message.AgentName, ConsoleColor.Gray);
        var typeIndicator = message.Type switch
        {
            MessageType.Challenge => "[CHALLENGE]",
            MessageType.Answer => "[ANSWER]",
            MessageType.Evaluation => "[EVALUATION]",
            MessageType.Hint => "[HINT]",
            MessageType.SystemAnnouncement => "[ANNOUNCEMENT]",
            _ => ""
        };

        SystemConsole.ForegroundColor = ConsoleColor.DarkGray;
        SystemConsole.WriteLine("-".PadRight(60, '-'));
        
        if (!string.IsNullOrEmpty(typeIndicator))
        {
            SystemConsole.ForegroundColor = ConsoleColor.DarkYellow;
            SystemConsole.WriteLine(typeIndicator);
        }

        SystemConsole.ForegroundColor = color;
        
        var lines = message.Content.Split('\n');
        foreach (var line in lines)
        {
            SystemConsole.WriteLine(line);
        }

        SystemConsole.ResetColor();
    }

    private static void PrintWelcome()
    {
        SystemConsole.Clear();
        SystemConsole.ForegroundColor = ConsoleColor.Cyan;
        SystemConsole.WriteLine("+==============================================================+");
        SystemConsole.WriteLine("|           GROUP CHAT - ANAGRAM WORD GAME                    |");
        SystemConsole.WriteLine("+==============================================================+");
        SystemConsole.WriteLine("|                                                              |");
        SystemConsole.WriteLine("|  Watch AI agents play an anagram game together!             |");
        SystemConsole.WriteLine("|                                                              |");
        SystemConsole.WriteLine("|  Agents:                                                     |");
        SystemConsole.WriteLine("|    * GameHost     - Moderates and proposes challenges       |");
        SystemConsole.WriteLine("|    * AnagramPlayer - Finds anagrams from the dictionary     |");
        SystemConsole.WriteLine("|    * WordExpert    - Provides hints and word facts          |");
        SystemConsole.WriteLine("|    * Judge         - Evaluates answers and scores           |");
        SystemConsole.WriteLine("|                                                              |");
        SystemConsole.WriteLine("|  The orchestrator decides who speaks next!                  |");
        SystemConsole.WriteLine("|                                                              |");
        SystemConsole.WriteLine("+==============================================================+");
        SystemConsole.ResetColor();
    }

    private static void PrintGoodbye()
    {
        SystemConsole.ForegroundColor = ConsoleColor.Cyan;
        SystemConsole.WriteLine("\nExiting Group Chat mode...");
        SystemConsole.ResetColor();
    }
}
