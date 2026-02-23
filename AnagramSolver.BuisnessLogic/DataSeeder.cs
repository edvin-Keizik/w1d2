using Microsoft.EntityFrameworkCore;
using AnagramSolver.Contracts;
using System.Text;

namespace AnagramSolver.BusinessLogic
{
    public class DataSeeder
    {
        public async Task SeedDatabaseAsync(string filePath, AnagramDbContext context)
        {
            if (await context.WordGroupsEntity.AnyAsync()) return;

            var rawLines = await File.ReadAllLinesAsync(filePath, Encoding.UTF8);
            var dictionary = new Dictionary<string, HashSet<string>>();

            var cleanedWordsList = new HashSet<string>();

            foreach (var line in rawLines)
            {
                string word = line.Trim().ToLower();
                if (string.IsNullOrWhiteSpace(word)) continue;

                cleanedWordsList.Add(word);

                string signature = string.Concat(word.OrderBy(c => c));

                if (!dictionary.ContainsKey(signature))
                {
                    dictionary[signature] = new HashSet<string>();
                }
                dictionary[signature].Add(word);
            }

            var addWords = cleanedWordsList.Select(word => new WordEntity
            {
                Value = word
            }).ToList();

            var entities = dictionary.Select(kvp => new WordGroupsEntity
            {
                Signature = kvp.Key,
                Words = string.Join(",", kvp.Value)
            }).ToList();

            await context.Words.AddRangeAsync(addWords);
            await context.WordGroupsEntity.AddRangeAsync(entities);
            await context.SaveChangesAsync();
        }
    }
}