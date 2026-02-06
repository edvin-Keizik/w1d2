using AnagramSolver.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.BusinessLogic
{
    public class DictionaryLoader : IDictionaryLoader
    {
        private readonly IFileSystemWrapper _fileSystem;

        public DictionaryLoader(IFileSystemWrapper fileSystem)
        {
            _fileSystem = fileSystem;
        }
        public async Task LoadWordsAsync(string path, IWordProcessor processor)
        {
            try
            {
                await foreach (var line in _fileSystem.ReadLinesAsync(path))
                {
                    string word = line.Trim();
                    if(!string.IsNullOrEmpty(word))
                    {
                        processor.AddWord(word);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public async Task<IEnumerable<string>> GetWordsAsync(IWordProcessor processor)
        {   
            return processor.GetDictionary();
        }

        public async Task AddWordAsync(string path, string word, IWordProcessor processor)
        {
            processor.AddWord(word);
            await File.AppendAllLinesAsync(path, new[] { word });
        }

        public async Task<bool> DeleteWordAsync(string path, int lineIndex, IWordProcessor processor)
        {
            var currentWords = processor.GetDictionary();

            if (lineIndex < 0 || lineIndex >= currentWords.Count) return false;

            currentWords.RemoveAt(lineIndex);
            await File.WriteAllLinesAsync(path, currentWords);
            processor.LoadDictionary(currentWords);

            return true;
        }
    }
}
