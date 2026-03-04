using AnagramSolver.BusinessLogic;
using AnagramSolver.BusinessLogic.ChainOfResponsibility;
using AnagramSolver.BusinessLogic.ChainOfResponsibility.Steps;
using AnagramSolver.Contracts;
using AnagramSolver.WebApp.GraphQL;
using AnagramSolver.WebApp.Plugins;
using AnagramSolver.WebApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

var kernelBuilder = builder.Services.AddKernel();
kernelBuilder.AddOpenAIChatCompletion(
    modelId: builder.Configuration["OpenAI:Model"]!,
    apiKey: builder.Configuration["OpenAI:ApiKey"]!);

kernelBuilder.Plugins.AddFromType<AnagramPlugin>();
kernelBuilder.Plugins.AddFromType<TimePlugin>();

builder.Services.AddSingleton<AnagramPlugin>();
builder.Services.AddSingleton<TimePlugin>();

// AI Chat Service and Chat History
builder.Services.AddSingleton<IChatHistoryService, ChatHistoryService>();
builder.Services.AddScoped<IAiChatService, AiChatService>();

var anagramSettings = builder.Configuration.GetSection("AnagramSettings").Get<AnagramSettings>()
               ?? throw new Exception("AnagramSettings missing from appsettings.json");

builder.Services.AddDbContext<AnagramDbContext>(options =>
    options.UseSqlServer(anagramSettings.DefaultConnection));

builder.Services.AddScoped<WordProcessor>(sp =>
{
    var searchEngine = sp.GetRequiredService<IAnagramSearchEngine>();
    var pipeline = sp.GetRequiredService<FilterPipeline>();
    var context = sp.GetRequiredService<AnagramDbContext>();
    var signatures = context.WordGroupsEntity.Select(x => x.Signature).ToHashSet();

    return new WordProcessor(searchEngine, pipeline, context, signatures);
});
builder.Services.AddScoped<IWordProcessor>(sp => sp.GetRequiredService<WordProcessor>());

builder.Services.AddScoped<IGetAnagrams>(sp =>
{
    var coreSearch = sp.GetRequiredService<WordProcessor>();
    var timingDecorator = new AnagramSearchTimingDecorator(coreSearch);
    var loggingDecorator = new AnagramSearchLogDecorator(timingDecorator);
    return loggingDecorator;
});

builder.Services.AddSingleton(anagramSettings);
builder.Services.AddSingleton<IAnagramSearchEngine, AnagramSearchEngine>();
builder.Services.AddSingleton<WordLengthStep>();
builder.Services.AddSingleton<AllowedCharactersStep>();

builder.Services.AddSingleton<FilterPipeline>(sp =>
{
    var pipeline = new FilterPipeline();
    pipeline.AddStep(sp.GetRequiredService<WordLengthStep>());
    pipeline.AddStep(sp.GetRequiredService<AllowedCharactersStep>());
    return pipeline;
});

builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGraphQLServer().AddQueryType<Query>();
builder.Services.AddHttpClient();
builder.Services.AddRazorPages();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AnagramDbContext>();
        await context.Database.MigrateAsync();

        if (File.Exists(anagramSettings.FilePath))
        {
            var seeder = new DataSeeder();
            await seeder.SeedDatabaseAsync(anagramSettings.FilePath, context);
            Console.WriteLine("Database seeding completed successfully.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred during seeding: {ex.Message}");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Anagram API V1"));
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.MapGraphQL();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Anagram}/{action=Index}/{id?}");

app.Run();