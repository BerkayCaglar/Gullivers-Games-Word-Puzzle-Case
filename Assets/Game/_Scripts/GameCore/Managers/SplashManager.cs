using System.Collections.Generic;
using GameCore.LevelSystem;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;
using GameCore.ScoreSystem;
using GameCore.PlayerJourneySystem;

namespace GameCore.Managers
{
    public class SplashManager : MonoBehaviour
    {
        private void Awake()
        {
            SetupScriptableObjectInstances();
            GetLevels();
            GetEnglishDictionary();
        }

        private void SetupScriptableObjectInstances()
        {
            var scriptableObjects = Resources.LoadAll<ScriptableObject>("ScriptableObjects/");
            foreach (var scriptableObject in scriptableObjects)
            {
                var type = scriptableObject.GetType();
                var property = type.GetProperty("Instance");
                var instance = property.GetValue(null); // Create instance
            }
        }

        private void GetLevels()
        {
            var levelsDatabasePath = "LevelsDatabase/";
            var levels = Resources.LoadAll<TextAsset>(levelsDatabasePath);

            var levelDatas = new List<LevelData>();
            foreach (var level in levels)
            {
                var levelData = JsonConvert.DeserializeObject<LevelData>(level.text);
                levelData.levelPoint = int.Parse(level.name.Split('_').Last());
                levelDatas.Add(levelData);
            }

            levelDatas.Sort((a, b) => a.levelPoint.CompareTo(b.levelPoint));

            SetupManagers(levelDatas);
        }

        private void GetEnglishDictionary()
        {
            PossibleWordsSystem.PossibleWordsGenerator.LoadDictionary();
        }

        private async void SetupManagers(List<LevelData> levelDatas)
        {
            LevelManager.Instance.Setup(levelDatas);
            HighScoreManager.Instance.Setup(levelDatas);
            await PlayerManager.Instance.Setup();
        }
    }
}
