using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts;

var builder = WebApplication.CreateBuilder(args);


var anagramSettings = builder.Configuration.GetSection("AnagramSettings").Get<AnagramSettings>()
               ?? throw new Exception("AnagramSettings missing from appsettings.json");

builder.Services.AddControllersWithViews();

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

    loader.LoadWords(anagramSettings.FilePath, processor);
}


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Anagram}/{action=Index}/{id?}");

app.Run();
