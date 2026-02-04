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
    }
}
