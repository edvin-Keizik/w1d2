using AnagramSolver.BuisnessLogic;
using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnagramSolver.Cli
{
    public class App
    {
        private readonly string _filePath;
        private readonly IWordProcessor _processor;
        private readonly IDictionaryLoader _loader;
        private readonly AnagramSettings _settings;


        public App(string filePath, AnagramSettings settings)
        {
            _filePath = filePath;
            _settings = settings;
            IAnagramSearchEngine searchEngine = new AnagramSearchEngine();
            _processor = new WordProcessor(searchEngine);
            _loader = new DictionaryLoader();
        }

        public void Run()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("Atsisiunciamas zodynas");
            _loader.LoadWords(_filePath, _processor);
            Console.WriteLine("Zodynas atsiustas. Ivesti 0 kad baigti.");

            while (true)
            {
                string input = "";
                bool lengthCheck = false;
                int anagramCount = 0;

                while (!lengthCheck)
                {
                    Console.WriteLine("\nIveskite zodi kurio anagramas norite suzinot(bent 3 raides): ");
                    input = Console.ReadLine()?.Trim() ?? "";
                    if (input == "0") return;

                    if (input.Length >= _settings.MinWordLength)
                    {
                        lengthCheck = true;
                    }
                    else
                    {
                        Console.WriteLine($"Klaida: Zodis per trumpas!");
                    }
                }           

                List<Anagram> anagrams = _processor.GetAnagrams(input, _settings.MaxAnagramsToShow);
                if (anagrams.Any())
                {
                    Console.WriteLine($"Zodzio {input} anagramos: ");
                    foreach (var anagram in anagrams)
                    {
                            Console.WriteLine(anagram.Word);
                            anagramCount++;
                    }
                }
                else
                {
                    Console.WriteLine($"Anagramu zodziui {input} nera.");
                }
            }
        }
    }
}
