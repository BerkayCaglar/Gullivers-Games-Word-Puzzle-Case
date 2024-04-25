using System.Collections;
using System.Collections.Generic;
using GameCore.LevelSystem;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

namespace GameCore.Managers
{
    public class SplashManager : MonoBehaviour
    {
        private void Awake()
        {
            var levelsDatabasePath = "LevelsDatabase/";
            //var stringWordsDatabasePath = "StringWordsDatabase/";
            var levels = Resources.LoadAll<TextAsset>(levelsDatabasePath);

            var levelDatas = new List<LevelData>();
            foreach (var level in levels)
            {
                var levelData = JsonConvert.DeserializeObject<LevelData>(level.text);
                levelData.level = int.Parse(level.name.Split('_').Last());
                levelDatas.Add(levelData);
            }

            levelDatas.Sort((a, b) => a.level.CompareTo(b.level));

            LevelManager.Instance.SetLevels(levelDatas.ToArray());
        }
    }
}
