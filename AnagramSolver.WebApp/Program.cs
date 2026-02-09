using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts;
using AnagramSolver.WebApp.GraphQL;

var builder = WebApplication.CreateBuilder(args);


var anagramSettings = builder.Configuration.GetSection("AnagramSettings").Get<AnagramSettings>()
               ?? throw new Exception("AnagramSettings missing from appsettings.json");

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

builder.Services.AddSingleton(anagramSettings);
builder.Services.AddSingleton<IAnagramSearchEngine, AnagramSearchEngine>();
builder.Services.AddSingleton<IWordProcessor, WordProcessor>();
builder.Services.AddSingleton<IFileSystemWrapper, FileSystemWrapper>();
builder.Services.AddSingleton<IDictionaryLoader, DictionaryLoader>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var processor = scope.ServiceProvider.GetRequiredService<IWordProcessor>();
    var loader = scope.ServiceProvider.GetRequiredService<IDictionaryLoader>();

    await loader.LoadWordsAsync(anagramSettings.FilePath, processor);
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Anagram API V1");
});

app.MapGraphQL();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Anagram}/{action=Index}/{id?}");

app.Run();
