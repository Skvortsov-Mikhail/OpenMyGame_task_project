using System.Collections.Generic;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;
using Newtonsoft.Json;
using UnityEngine;

namespace App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel.ProviderWordLevel
{
    public class ProviderWordLevel : IProviderWordLevel
    {
        class JSONData
        {
            public List<string> words;
        }

        public LevelInfo LoadLevelData(int levelIndex)
        {
            LevelInfo info = new LevelInfo();

            string fileData = Resources.Load<TextAsset>($"WordSearch/Levels/{levelIndex}").text;

            info.words = JsonConvert.DeserializeObject<JSONData>(fileData).words;

            return info;
        }
    }
}