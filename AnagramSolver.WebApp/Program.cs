using AnagramSolver.BusinessLogic;
using AnagramSolver.BusinessLogic.ChainOfResponsibility;
using AnagramSolver.BusinessLogic.ChainOfResponsibility.Steps;
using AnagramSolver.Contracts;
using AnagramSolver.WebApp.GraphQL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var anagramSettings = builder.Configuration.GetSection("AnagramSettings").Get<AnagramSettings>()
               ?? throw new Exception("AnagramSettings missing from appsettings.json");

builder.Services.AddDbContext<AnagramDbContext>(options =>
    options.UseSqlServer(anagramSettings.DefaultConnection));

builder.Services.AddScoped<WordProcessor>();
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Anagram}/{action=Index}/{id?}");

app.Run();