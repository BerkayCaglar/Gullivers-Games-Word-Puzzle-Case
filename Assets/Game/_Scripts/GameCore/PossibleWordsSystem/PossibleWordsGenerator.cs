using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace GameCore.PossibleWordsSystem
{
    public class PossibleWordsGenerator
    {
        private static HashSet<string> dictionaryWords = new();

        public static void LoadDictionary()
        {
            var textAssetResourcePath = "StringWordsDatabase/en";
            string[] lines = Resources.Load<TextAsset>(textAssetResourcePath).text.Split('\n');
            dictionaryWords = new HashSet<string>(lines);
            Debug.Log($"Dictionary loaded. Word count: {dictionaryWords.Count}");
            dictionaryWords.RemoveWhere(x => x.Length > 7);
            Debug.Log($"Dictionary cleaned. New word count: {dictionaryWords.Count}");
        }

        public static async Task<string[]> GetPossibleWordsAsync(string[] letters)
        {
            if (dictionaryWords.Count == 0)
            {
                Debug.LogError("Dictionary is not loaded. Please call LoadDictionary() method first.");
                LoadDictionary(); // Consider making LoadDictionary async as well for real-world applications
            }
            //Debug.Log($"Letters: {string.Join(", ", letters)}");
            var possibleWordsFromLetters = await GetPossibleWordsFromLettersAsync(letters);

            return possibleWordsFromLetters.ToArray();
        }

        public static async Task<string[]> GetPossibleWordsAsync(string[] letters, bool checkOneTime)
        {
            if (dictionaryWords.Count == 0)
            {
                Debug.LogError("Dictionary is not loaded. Please call LoadDictionary() method first.");
                LoadDictionary(); // Consider making LoadDictionary async as well for real-world applications
            }
            //Debug.Log($"Letters: {string.Join(", ", letters)}");
            var possibleWordsFromLetters = await GetPossibleWordsFromLettersAsync(letters, checkOneTime);

            return possibleWordsFromLetters.ToArray();
        }

        public static async Task<string[]> GetPossibleWordsAsync(string startsWith, string[] letters)
        {
            if (dictionaryWords.Count == 0)
            {
                Debug.LogError("Dictionary is not loaded. Please call LoadDictionary() method first.");
                LoadDictionary(); // Consider making LoadDictionary async as well for real-world applications
            }

            Debug.Log($"Starts With: {startsWith} - Letters: {string.Join(", ", letters)}");
            var possibleWordsFromLetters = await GetPossibleWordsFromLettersAsync(letters);

            return possibleWordsFromLetters.Where(x => x.StartsWith(startsWith.ToLowerInvariant())).ToArray();
        }

        public static async Task<string[]> GetPossibleWordsAsync(string startsWith, string[] letters, bool checkOneTime)
        {
            if (dictionaryWords.Count == 0)
            {
                Debug.LogError("Dictionary is not loaded. Please call LoadDictionary() method first.");
                LoadDictionary(); // Consider making LoadDictionary async as well for real-world applications
            }

            Debug.Log($"Starts With: {startsWith} - Letters: {string.Join(", ", letters)}");
            var possibleWordsFromLetters = await GetPossibleWordsFromLettersAsync(startsWith.ToLowerInvariant(), letters, checkOneTime);

            return possibleWordsFromLetters.ToArray();
        }

        public static async Task<string[]> GetPossibleWordsAsync(List<string[]> wordCharacters, bool checkOneTime)
        {
            // wordCharacters: [ ["a", "b", "c"], ["d", "e", "f"], ["g", "h", "i"] ]
            // her bir array kelimenin olası harflerini temsil eder
            // her bir array'in ilk elemanı kelimenin başlangıç harfidir
            // her bir array'in geri kalan elemanları kelimenin geri kalan harfleridir

            if (dictionaryWords.Count == 0)
            {
                Debug.LogError("Dictionary is not loaded. Please call LoadDictionary() method first.");
                LoadDictionary(); // Consider making LoadDictionary async as well for real-world applications
            }

            var possibleWords = new List<string>();
            foreach (var word in wordCharacters)
            {
                var startsWith = word[0];
                var letters = word.Skip(1).ToArray();
                var possibleWordsFromLetters = await GetPossibleWordsFromLettersAsync(startsWith, letters, checkOneTime);

                possibleWords.AddRange(possibleWordsFromLetters);
            }

            return possibleWords.ToArray();
        }

        private static async Task<HashSet<string>> GetPossibleWordsFromLettersAsync(string[] letters, bool checkOneTime = false)
        {
            HashSet<string> possibleWords = new();
            var permutations = await GetPermutationsAsync(letters, 1, Math.Min(7, letters.Length));

            foreach (var word in permutations)
            {
                var lowerCaseWord = word.ToLowerInvariant();
                if (dictionaryWords.Contains(lowerCaseWord))
                {
                    possibleWords.Add(lowerCaseWord);
                    if (checkOneTime)
                        break;
                }
            }

            return possibleWords;
        }

        private static async Task<HashSet<string>> GetPossibleWordsFromLettersAsync(string startsWith, string[] letters, bool checkOneTime = false)
        {
            HashSet<string> possibleWords = new();
            var permutations = await GetPermutationsAsync(letters, 1, Math.Min(7, letters.Length));

            foreach (var word in permutations)
            {
                var lowerCaseWord = word.ToLowerInvariant();
                if (dictionaryWords.Contains(lowerCaseWord) && lowerCaseWord.StartsWith(startsWith.ToLowerInvariant()))
                {
                    possibleWords.Add(lowerCaseWord);
                    if (checkOneTime)
                        break;
                }
            }

            return possibleWords;
        }

        private static async Task<HashSet<string>> GetPossibleWordsFromLettersAsync(string[] startsWith, string[] letters, bool checkOneTime = false)
        {
            HashSet<string> possibleWords = new();
            var permutations = await GetPermutationsAsync(letters, 1, Math.Min(7, letters.Length));

            foreach (var word in permutations)
            {
                var lowerCaseWord = word.ToLowerInvariant();
                if (dictionaryWords.Contains(lowerCaseWord) && startsWith.Any(x => lowerCaseWord.StartsWith(x.ToLowerInvariant())))
                {
                    possibleWords.Add(lowerCaseWord);
                    if (checkOneTime)
                        break;
                }
            }

            return possibleWords;
        }

        private static Task<HashSet<string>> GetPermutationsAsync(string[] letters, int minWordLength, int maxWordLength)
        {
            return Task.Run(() =>
            {
                HashSet<string> permutations = new HashSet<string>();
                for (int i = minWordLength; i <= maxWordLength; i++)
                {
                    GetPermutationsRecursive(letters, i, "", new bool[letters.Length], permutations);
                }
                return permutations;
            });
        }

        private static void GetPermutationsRecursive(string[] letters, int wordLength, string currentWord, bool[] used, HashSet<string> permutations)
        {
            if (currentWord.Length == wordLength)
            {
                permutations.Add(currentWord);
                return;
            }

            for (int i = 0; i < letters.Length; i++)
            {
                if (!used[i])
                {
                    used[i] = true;
                    GetPermutationsRecursive(letters, wordLength, currentWord + letters[i], used, permutations);
                    used[i] = false;
                }
            }
        }

        public static async Task<bool> IsCorrectWordAsync(string word)
        {
            if (dictionaryWords.Count == 0)
            {
                Debug.LogError("Dictionary is not loaded. Please call LoadDictionary() method first.");
                LoadDictionary(); // Consider making LoadDictionary async as well for real-world applications
            }

            return await Task.Run(() => dictionaryWords.Contains(word));
        }
    }
}
