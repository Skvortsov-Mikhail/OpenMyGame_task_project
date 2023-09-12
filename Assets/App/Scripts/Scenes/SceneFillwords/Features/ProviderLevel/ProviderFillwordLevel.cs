using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels;

namespace App.Scripts.Scenes.SceneFillwords.Features.ProviderLevel
{
    public class ProviderFillwordLevel : IProviderFillwordLevel
    {
        private readonly string[] vocabulary = File.ReadAllLines("Assets/App/Resources/Fillwords/words_list.txt");
        private readonly List<String> levelsData = File.ReadAllLines("Assets/App/Resources/Fillwords/pack_0.txt").ToList();

        public GridFillWords LoadModel(int index)
        {
            string currentLevelData = levelsData[index - 1];

            List<ProviderLevelLetter> currentLevelLetters = ParseDataToLetters(currentLevelData.Split(' '), index);

            if (!IsLevelValid(currentLevelLetters)) return SkipInvalidLevel(index);

            int gridSize = (int)Mathf.Sqrt(currentLevelLetters.Count);

            return FillGridFillWords(gridSize, currentLevelLetters);
        }

        private List<ProviderLevelLetter> ParseDataToLetters(string[] data, int index)
        {
            List<ProviderLevelLetter> parsedData = new List<ProviderLevelLetter>();

            for (int i = 0; i < data.Length; i += 2)
            {
                int wordIndex = Int32.Parse(data[i]);

                char[] letters = vocabulary[wordIndex].ToCharArray();

                int[] indexes = data[i + 1].Split(';').Select(int.Parse).ToArray();

                if (letters.Length != indexes.Length) return null;

                for (int j = 0; j < letters.Length; j++)
                {
                    parsedData.Add(new ProviderLevelLetter(letters[j], indexes[j]));
                }
            }

            return parsedData;
        }

        private bool IsLevelValid(List<ProviderLevelLetter> levelData)
        {
            if (levelData == null || Mathf.Sqrt(levelData.Count) % 1 != 0) return false;

            List<int> indexes = new List<int>();

            foreach (ProviderLevelLetter letter in levelData)
            {
                if (letter.index >= levelData.Count) return false;

                indexes.Add(letter.index);
            }

            HashSet<int> setIndexes = new HashSet<int>(indexes);

            if (setIndexes.Count != indexes.Count) return false;

            return true;
        }

        private GridFillWords SkipInvalidLevel(int index)
        {
            if (levelsData.Count > 1)
            {
                levelsData.RemoveAt(index - 1);
                return LoadModel(index);
            }
            else
            {
                throw new Exception();
            }
        }

        private GridFillWords FillGridFillWords(int gridSize, List<ProviderLevelLetter> currentLevelLetters)
        {
            GridFillWords gridFillWords = new GridFillWords(new Vector2Int(gridSize, gridSize));

            List<ProviderLevelGrid> grids = CreateGrid(gridSize);

            foreach (ProviderLevelGrid grid in grids)
            {
                foreach (ProviderLevelLetter letter in currentLevelLetters)
                {
                    if (grid.index == letter.index)
                    {
                        gridFillWords.Set(grid.coordinates.x, grid.coordinates.y, new CharGridModel(letter.letter));
                    }
                }
            }

            return gridFillWords;
        }

        private List<ProviderLevelGrid> CreateGrid(int gridSize)
        {
            List<ProviderLevelGrid> grids = new List<ProviderLevelGrid>();

            int gridCount = 0;

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    grids.Add(new ProviderLevelGrid(new Vector2Int(i, j), gridCount));
                    gridCount++;
                }
            }

            return grids;
        }
    }
}

