using System;
using System.Collections.Generic;
using UnityEngine;
using App.Scripts.Libs.Factory;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;

namespace App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel
{
    public class FactoryLevelModel : IFactory<LevelModel, LevelInfo, int>
    {
        public LevelModel Create(LevelInfo value, int levelNumber)
        {
            var model = new LevelModel();

            model.LevelNumber = levelNumber;

            model.Words = value.words;
            model.InputChars = BuildListChars(value.words);

            return model;
        }

        private List<char> BuildListChars(List<string> words)
        {
            Dictionary<char, int> letterPairsInLevel = new Dictionary<char, int>();

            foreach (string word in words)
            {
                Dictionary<char, int> letterPairsInWord = ParseWordToLetters(word);

                foreach (var letter in letterPairsInWord.Keys)
                {
                    if (letterPairsInLevel.ContainsKey(letter))
                    {
                        int letterCountInLevelDictionary = letterPairsInLevel.GetValueOrDefault(letter);
                        int letterCountInWordDictionary = letterPairsInWord.GetValueOrDefault(letter);

                        letterPairsInLevel[letter] = Mathf.Max(letterCountInLevelDictionary, letterCountInWordDictionary);
                    }
                    else
                    {
                        letterPairsInLevel[letter] = letterPairsInWord.GetValueOrDefault(letter);
                    }
                }
            }

            List<char> builtList = FillLettersList(letterPairsInLevel);

            Shuffle(builtList);

            return builtList;
        }

        private Dictionary<char, int> ParseWordToLetters(string word)
        {
            Dictionary<char, int> dictionary = new Dictionary<char, int>();

            foreach (char letter in word.ToLower().ToCharArray())
            {
                if (!Char.IsLetter(letter)) continue;

                if (dictionary.ContainsKey(letter))
                {
                    dictionary[letter] = dictionary.GetValueOrDefault(letter) + 1;
                }
                else
                {
                    dictionary.Add(letter, 1);
                }
            }

            return dictionary;
        }

        private List<char> FillLettersList(Dictionary<char, int> letters)
        {
            List<char> list = new List<char>();

            foreach (char letter in letters.Keys)
            {
                for (int i = 0; i < letters.GetValueOrDefault(letter); i++)
                {
                    list.Add(letter);
                }
            }

            return list;
        }

        private void Shuffle(List<char> array)
        {
            var random = new System.Random();
            int n = array.Count;

            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                char value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
        }
    }
}