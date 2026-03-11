using AnagramSolver.BusinessLogic;
using AnagramSolver.BusinessLogic.ChainOfResponsibility;
using AnagramSolver.BusinessLogic.ChainOfResponsibility.Steps;
using AnagramSolver.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AnagramSolver.MAF.Services;

public static class ServiceConfiguration
{
    public static IServiceCollection ConfigureAnagramServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        
        var connectionString = configuration["AnagramSettings:DefaultConnection"]
            ?? throw new InvalidOperationException(
                "Database connection string not found. Check appsettings.json for 'AnagramSettings:DefaultConnection'");

        services.AddDbContext<AnagramDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddSingleton<IAnagramSearchEngine, AnagramSearchEngine>();

        services.AddSingleton<WordLengthStep>();
        services.AddSingleton<AllowedCharactersStep>();

        services.AddSingleton<FilterPipeline>(sp =>
        {
            var pipeline = new FilterPipeline();
            pipeline.AddStep(sp.GetRequiredService<WordLengthStep>());
            pipeline.AddStep(sp.GetRequiredService<AllowedCharactersStep>());
            return pipeline;
        });

        services.AddScoped<WordProcessor>(sp =>
        {
            var searchEngine = sp.GetRequiredService<IAnagramSearchEngine>();
            var pipeline = sp.GetRequiredService<FilterPipeline>();
            var context = sp.GetRequiredService<AnagramDbContext>();
            
            var signatures = context.WordGroupsEntity
                .Select(x => x.Signature)
                .ToHashSet();

            return new WordProcessor(searchEngine, pipeline, context, signatures);
        });

        services.AddScoped<IWordProcessor>(sp => sp.GetRequiredService<WordProcessor>());
        
        services.AddScoped<IGetAnagrams>(sp =>
        {
            var coreSearch = sp.GetRequiredService<WordProcessor>();
            var timingDecorator = new AnagramSearchTimingDecorator(coreSearch);
            var loggingDecorator = new AnagramSearchLogDecorator(timingDecorator);
            return loggingDecorator;
        });

        return services;
    }
}
