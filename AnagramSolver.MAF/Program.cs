using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Chat;
using System.ClientModel;
using AnagramSolver.MAF.Services;
using AnagramSolver.MAF.Tools;
using AnagramSolver.MAF.Console;
using AnagramSolver.MAF.Workflow;
using AnagramSolver.MAF.Workflow.Agents;
using AnagramSolver.MAF.Workflow.Handoff;
using AnagramSolver.MAF.Workflow.GroupChat;

Console.WriteLine("Loading configuration...");

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .Build();


var apiKey = configuration["OpenAI:ApiKey"];
var model = configuration["OpenAI:Model"] ?? "gpt-4o";

if (string.IsNullOrEmpty(apiKey))
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("ERROR: OpenAI API key not configured!");
    Console.WriteLine("Run: dotnet user-secrets set \"OpenAI:ApiKey\" \"your-key\"");
    Console.ResetColor();
    return;
}

Console.WriteLine("Setting up services...");

var services = new ServiceCollection();

services.ConfigureAnagramServices(configuration);

services.AddScoped<IAnagramTools, AnagramTools>();

var serviceProvider = services.BuildServiceProvider();


Console.WriteLine("Initializing AI client...");

IChatClient chatClient = new ChatClient(model, new ApiKeyCredential(apiKey))
    .AsIChatClient();

using var cts = new CancellationTokenSource();

Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cts.Cancel();
    Console.WriteLine("\nShutting down...");
};

try
{
    while (!cts.Token.IsCancellationRequested)
    {
        PrintMainMenu();

        var choice = Console.ReadLine()?.Trim();

        switch (choice)
        {
            case "1":
                await RunSingleAgentModeAsync(serviceProvider, chatClient, cts.Token);
                break;

            case "2":
                await RunSequentialWorkflowAsync(serviceProvider, chatClient, cts.Token);
                break;

            case "3":
                await RunHandoffWorkflowAsync(serviceProvider, chatClient, cts.Token);
                break;

            case "4":
                await RunGroupChatWorkflowAsync(serviceProvider, chatClient, cts.Token);
                break;

            case "5":
            case "exit":
            case "quit":
                PrintGoodbye();
                return;

            default:
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Invalid choice. Please enter 1, 2, 3, 4, or 5.");
                Console.ResetColor();
                Thread.Sleep(1000);
                break;
        }
    }
}
catch (OperationCanceledException)
{
    Console.WriteLine("Application terminated by user.");
}
finally
{
    Console.WriteLine("Cleaning up resources...");
}

static void PrintMainMenu()
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("+==============================================================+");
    Console.WriteLine("|        ANAGRAM SOLVER - AI AGENT FRAMEWORK DEMO              |");
    Console.WriteLine("|                Microsoft Agent Framework                      |");
    Console.WriteLine("+==============================================================+");
    Console.WriteLine("|                                                              |");
    Console.WriteLine("|   [1] Single Agent Mode                                      |");
    Console.WriteLine("|       Interactive chat with function tools                   |");
    Console.WriteLine("|                                                              |");
    Console.WriteLine("|   [2] Sequential Workflow Mode                               |");
    Console.WriteLine("|       Pipeline: Finder -> Analyzer -> Presenter              |");
    Console.WriteLine("|                                                              |");
    Console.WriteLine("|   [3] Handoff Workflow Mode                                  |");
    Console.WriteLine("|       Triage -> Specialists (routing)                        |");
    Console.WriteLine("|                                                              |");
    Console.WriteLine("|   [4] Group Chat Mode                                        |");
    Console.WriteLine("|       Multi-agent word game (Host, Player, Expert, Judge)    |");
    Console.WriteLine("|                                                              |");
    Console.WriteLine("|   [5] Exit                                                   |");
    Console.WriteLine("|                                                              |");
    Console.WriteLine("+==============================================================+");
    Console.ResetColor();
    Console.Write("\n=> Enter your choice (1-5): ");
}

static void PrintGoodbye()
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("\n+==============================================================+");
    Console.WriteLine("|  Thank you for using the Anagram Solver AI Agent Demo!       |");
    Console.WriteLine("|  Goodbye!                                                    |");
    Console.WriteLine("+==============================================================+");
    Console.ResetColor();
}

static async Task RunSingleAgentModeAsync(
    ServiceProvider serviceProvider,
    IChatClient chatClient,
    CancellationToken cancellationToken)
{
    using var scope = serviceProvider.CreateScope();
    var scopedProvider = scope.ServiceProvider;

    var anagramTools = scopedProvider.GetRequiredService<IAnagramTools>();
    var toolFunctions = new AnagramToolFunctions(anagramTools);
    var tools = toolFunctions.CreateAllFunctions();

    Console.WriteLine($"[OK] Registered {tools.Count} tools: {string.Join(", ", tools.Select(t => t.Name))}");
    Thread.Sleep(500);

    var console = new AnagramAgentConsole(chatClient, tools, cancellationToken);
    await console.RunAsync();
}

static async Task RunSequentialWorkflowAsync(
    ServiceProvider serviceProvider,
    IChatClient chatClient,
    CancellationToken cancellationToken)
{
    using var scope = serviceProvider.CreateScope();
    var scopedProvider = scope.ServiceProvider;

    var anagramTools = scopedProvider.GetRequiredService<IAnagramTools>();

    var finder = new FinderAgent(anagramTools, chatClient);
    var analyzer = new AnalyzerAgent(chatClient);
    var presenter = new PresenterAgent(chatClient);

    Console.WriteLine("[OK] Created workflow agents: Finder, Analyzer, Presenter");
    Thread.Sleep(500);

    var orchestrator = new SequentialWorkflowOrchestrator(finder, analyzer, presenter);

    var workflowConsole = new WorkflowConsole(orchestrator, cancellationToken);
    await workflowConsole.RunAsync();
}

static async Task RunHandoffWorkflowAsync(
    ServiceProvider serviceProvider,
    IChatClient chatClient,
    CancellationToken cancellationToken)
{
    using var scope = serviceProvider.CreateScope();
    var scopedProvider = scope.ServiceProvider;

    var anagramTools = scopedProvider.GetRequiredService<IAnagramTools>();

    var specialists = new List<ISpecialistAgent>
    {
        new AnagramSpecialist(anagramTools),
        new WordAnalysisSpecialist(anagramTools)
    };

    var triageAgent = new TriageAgent(chatClient, specialists);
    var orchestrator = new HandoffOrchestrator(triageAgent, specialists);

    Console.WriteLine("[OK] Created handoff agents: Triage, Anagram Specialist, Word Analysis Specialist");
    Thread.Sleep(500);

    var handoffConsole = new HandoffConsole(orchestrator, cancellationToken);
    await handoffConsole.RunAsync();
}

static async Task RunGroupChatWorkflowAsync(
    ServiceProvider serviceProvider,
    IChatClient chatClient,
    CancellationToken cancellationToken)
{
    using var scope = serviceProvider.CreateScope();
    var scopedProvider = scope.ServiceProvider;

    var anagramTools = scopedProvider.GetRequiredService<IAnagramTools>();

    var agents = new List<IGroupChatAgent>
    {
        new GameHostAgent(chatClient),
        new AnagramPlayerAgent(anagramTools),
        new WordExpertAgent(chatClient, anagramTools),
        new JudgeAgent(chatClient)
    };

    var orchestrator = new GroupChatOrchestrator(agents);

    Console.WriteLine("[OK] Created group chat agents: GameHost, AnagramPlayer, WordExpert, Judge");
    Thread.Sleep(500);

    var groupChatConsole = new GroupChatConsole(orchestrator, cancellationToken);
    await groupChatConsole.RunAsync();
}