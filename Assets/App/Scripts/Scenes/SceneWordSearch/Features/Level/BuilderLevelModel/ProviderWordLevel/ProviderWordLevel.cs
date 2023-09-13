using System.Collections.Generic;
using System.IO;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;
using Newtonsoft.Json;

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

            string fileData = File.ReadAllText($"Assets/App/Resources/WordSearch/Levels/{levelIndex}.json");

            info.words = JsonConvert.DeserializeObject<JSONData>(fileData).words;

            return info;
        }
    }
}