using System.Diagnostics;
using SystemConsole = System.Console;

namespace AnagramSolver.MAF.Workflow;

public sealed class WorkflowConsole
{
    private readonly SequentialWorkflowOrchestrator _orchestrator;
    private readonly CancellationToken _cancellationToken;

    private static readonly Dictionary<string, ConsoleColor> _stepColors = new()
    {
        { "[WORKFLOW]", ConsoleColor.Magenta },
        { "[FINDER]", ConsoleColor.Cyan },
        { "[ANALYZER]", ConsoleColor.Yellow },
        { "[PRESENTER]", ConsoleColor.Green }
    };

    public WorkflowConsole(
        SequentialWorkflowOrchestrator orchestrator,
        CancellationToken cancellationToken = default)
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
                SystemConsole.ForegroundColor = ConsoleColor.White;
                SystemConsole.Write("\nEnter a word to find anagrams (or 'exit' to quit): ");
                SystemConsole.ResetColor();

                var input = SystemConsole.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(input))
                {
                    continue;
                }

                if (IsExitCommand(input))
                {
                    PrintGoodbye();
                    break;
                }

                var (searchWord, maxAnagrams, minLength) = ParseInput(input);

                SystemConsole.WriteLine();
                SystemConsole.ForegroundColor = ConsoleColor.DarkGray;
                SystemConsole.WriteLine("Starting sequential workflow with streaming...");
                SystemConsole.ResetColor();
                SystemConsole.WriteLine();

                var stopwatch = Stopwatch.StartNew();
                PresenterOutput? finalResult = null;

                await foreach (var update in _orchestrator.ExecuteStreamingAsync(
                    searchWord,
                    maxAnagrams,
                    minLength,
                    _cancellationToken))
                {
                    DisplayUpdate(update);

                    if (update.Type == UpdateType.Completed && update.Data is PresenterOutput po)
                    {
                        finalResult = po;
                    }
                }

                stopwatch.Stop();

                if (finalResult != null)
                {
                    SystemConsole.WriteLine();
                    SystemConsole.ForegroundColor = ConsoleColor.White;
                    SystemConsole.WriteLine("═══════════════════════════════════════════════════════════════");
                    SystemConsole.WriteLine("                    📋 FINAL RESULT                            ");
                    SystemConsole.WriteLine("═══════════════════════════════════════════════════════════════");
                    SystemConsole.ResetColor();
                    SystemConsole.WriteLine(finalResult.FormattedResponse);
                }

                SystemConsole.ForegroundColor = ConsoleColor.DarkGray;
                SystemConsole.WriteLine($"\nTotal workflow time: {stopwatch.ElapsedMilliseconds}ms");
                SystemConsole.ResetColor();
            }
            catch (OperationCanceledException)
            {
                SystemConsole.WriteLine("\nOperation cancelled.");
                break;
            }
            catch (Exception ex)
            {
                SystemConsole.ForegroundColor = ConsoleColor.Red;
                SystemConsole.WriteLine($"\nError: {ex.Message}");
                SystemConsole.ResetColor();
            }
        }
    }

    private static void DisplayUpdate(WorkflowStreamUpdate update)
    {
        var color = _stepColors.GetValueOrDefault(update.StepName, ConsoleColor.Gray);

        switch (update.Type)
        {
            case UpdateType.Started:
                SystemConsole.ForegroundColor = color;
                SystemConsole.WriteLine($"┌─ {update.StepName}");
                SystemConsole.ForegroundColor = ConsoleColor.DarkGray;
                SystemConsole.WriteLine($"│  {update.Content}");
                break;

            case UpdateType.Progress:
                SystemConsole.ForegroundColor = ConsoleColor.DarkGray;
                SystemConsole.Write("│  ");
                SystemConsole.ForegroundColor = color;
                SystemConsole.WriteLine(update.Content);
                break;

            case UpdateType.Completed:
                SystemConsole.ForegroundColor = ConsoleColor.DarkGray;
                SystemConsole.Write("│  ");
                SystemConsole.ForegroundColor = ConsoleColor.Green;
                SystemConsole.WriteLine(update.Content);
                SystemConsole.ForegroundColor = color;
                SystemConsole.WriteLine($"└─ {update.StepName} finished");
                SystemConsole.ResetColor();
                SystemConsole.WriteLine();
                break;

            case UpdateType.Error:
                SystemConsole.ForegroundColor = ConsoleColor.DarkGray;
                SystemConsole.Write("│  ");
                SystemConsole.ForegroundColor = ConsoleColor.Red;
                SystemConsole.WriteLine(update.Content);
                SystemConsole.ForegroundColor = color;
                SystemConsole.WriteLine($"└─ {update.StepName} failed");
                SystemConsole.ResetColor();
                break;
        }

        SystemConsole.ResetColor();
    }

    private static (string word, int maxAnagrams, int minLength) ParseInput(string input)
    {
        var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var word = parts[0];
        var maxAnagrams = 2;
        var minLength = 2;

        foreach (var part in parts.Skip(1))
        {
            if (part.StartsWith("--max=") && int.TryParse(part[6..], out var max))
            {
                maxAnagrams = Math.Clamp(max, 1, 5);
            }
            else if (part.StartsWith("--min=") && int.TryParse(part[6..], out var min))
            {
                minLength = Math.Max(min, 1);
            }
        }

        return (word, maxAnagrams, minLength);
    }

    private static bool IsExitCommand(string input)
    {
        var exitCommands = new[] { "exit", "quit", "bye", "q", "back", "menu" };
        return exitCommands.Contains(input.ToLowerInvariant());
    }

    private static void PrintWelcome()
    {
        SystemConsole.Clear();
        SystemConsole.ForegroundColor = ConsoleColor.Cyan;
        SystemConsole.WriteLine("╔══════════════════════════════════════════════════════════════╗");
        SystemConsole.WriteLine("║      SEQUENTIAL WORKFLOW: ANAGRAM ANALYSIS PIPELINE          ║");
        SystemConsole.WriteLine("║                                                              ║");
        SystemConsole.WriteLine("║     Pipeline: Finder -> Analyzer -> Presenter                ║");
        SystemConsole.WriteLine("╠══════════════════════════════════════════════════════════════╣");
        SystemConsole.WriteLine("║  Watch each agent work in real-time with streaming!          ║");
        SystemConsole.WriteLine("║                                                              ║");
        SystemConsole.WriteLine("║  Options:                                                    ║");
        SystemConsole.WriteLine("║  • Simple: just type a word                                  ║");
        SystemConsole.WriteLine("║  • Advanced: word --max=3 --min=2                            ║");
        SystemConsole.WriteLine("║  • Type 'exit' to return to main menu                        ║");
        SystemConsole.WriteLine("╚══════════════════════════════════════════════════════════════╝");
        SystemConsole.ResetColor();
    }

    private static void PrintGoodbye()
    {
        SystemConsole.ForegroundColor = ConsoleColor.Cyan;
        SystemConsole.WriteLine("\nExiting Sequential Workflow mode...");
        SystemConsole.ResetColor();
    }
}
