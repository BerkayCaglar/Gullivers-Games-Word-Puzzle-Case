using System.Collections.Generic;
using UnityEngine;
using GameCore.SingletonSystem;
using MyBox;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace GameCore.LevelSystem
{
    [CreateAssetMenu(fileName = "LevelManager", menuName = "ScriptableObjects/LevelManager", order = 1)]
    public class LevelManager : ScriptableResourceSingleton<LevelManager>
    {
        public LevelData[] levels = new LevelData[0];

        public LevelData[] GetLevels()
        {
            return levels;
        }

        public LevelData GetLevelData(int level)
        {
            return levels[level];
        }

        public int GetLevelCount()
        {
            return levels.Length;
        }

        public void SetLevels(LevelData[] levels)
        {
            this.levels = levels;
        }

        public void Setup(List<LevelData> levelDatas)
        {
            levels = levelDatas.ToArray();
        }

        public LevelData GetLevelTiles(int level)
        {
            return levels[level - 1];
        }

        [ButtonMethod]
        public void GetLevelsFromResources()
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

            Setup(levelDatas);
        }

        [ButtonMethod]
        public async void SetLevelsCorrectAnswers()
        {
            await Task.Run(async () =>
            {
                var tasks = new List<Task>();
                Parallel.ForEach(levels, async (level) =>
                {
                    var characters = level.tiles.Select(x => x.character).ToArray();
                    Debug.Log($"Getting possible words for level {level.levelPoint}");
                    var possibleWords = await PossibleWordsSystem.PossibleWordsGenerator.GetPossibleWordsAsync(characters);

                    // write a json file for each level
                    var levelAnswerData = new PossibleWordsSystem.LevelAnswerData
                    {
                        level = level.levelPoint,
                        possibleWords = possibleWords
                    };
                    var json = JsonConvert.SerializeObject(levelAnswerData);
                    System.IO.File.WriteAllText($"Assets/Resources/LevelAnswersDatabase/Level_{level.levelPoint}_Answers.json", json);
                    Debug.Log($"<color=green>Level <color=yellow>{level.levelPoint}</color> answers are saved.</color> [Word count: {possibleWords.Length}]");
                });
                await Task.WhenAll(tasks);
            });
        }

        [ButtonMethod]
        public void LoadDictionary()
        {
            PossibleWordsSystem.PossibleWordsGenerator.LoadDictionary();
        }
    }
}
