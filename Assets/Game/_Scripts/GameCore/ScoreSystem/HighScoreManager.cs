using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameCore.LevelSystem;
using GameCore.SingletonSystem;
using MyBox;
using UnityEngine;

namespace GameCore.ScoreSystem
{
    [CreateAssetMenu(fileName = "HighScoreManager", menuName = "ScriptableObjects/HighScoreManager", order = 1)]
    public class HighScoreManager : ScriptableResourceSingleton<HighScoreManager>
    {
        public List<HighScoreData> highScores = new();

        public void SetHighScore(HighScoreData highScoreData)
        {
            int score = highScoreData.score;
            int level = highScoreData.level;

            var levelHighScoreData = highScores.FirstOrDefault(x => x.level == level);
            if (levelHighScoreData.Equals(default(HighScoreData)))
            {
                Debug.LogError("Level not found in highScores list");
                return;
            }
            else
            {
                Debug.Log($"Level Score: {levelHighScoreData.score} - Your Score: {score}");
                if (score > levelHighScoreData.score)
                {
                    Debug.Log("New High Score!");
                    highScores.Remove(levelHighScoreData);
                    highScores.Add(highScoreData);
                    highScores = highScores.OrderByDescending(x => x.score).ToList();
                }
            }
        }

        public int GetHighScore(int level)
        {
            var highScore = highScores.FirstOrDefault(x => x.level == level);
            if (highScore.Equals(default(HighScoreData)))
            {
                highScore = new HighScoreData
                {
                    level = level,
                    score = 0
                };
            }
            return highScore.score;
        }

        public void Setup(List<LevelData> levelDatas)
        {
            var isOutdated = highScores.Count < levelDatas.Count;
            var isRedundancy = highScores.Count > levelDatas.Count;

            if (isOutdated)
            {
                highScores.AddRange(levelDatas
                    .Where(levelData => highScores.All(x => x.level != levelData.levelPoint))
                    .Select(levelData => new HighScoreData
                    {
                        level = levelData.levelPoint,
                        score = 0
                    }));
            }

            if (isRedundancy)
            {
                highScores = highScores.Except(highScores
                    .Where(x => levelDatas.All(levelData => x.level != levelData.levelPoint)))
                    .ToList();
            }
        }

        [ButtonMethod]
        public void ResetHighScores()
        {
            var convertedList = highScores.Select(x => new HighScoreData
            {
                level = x.level,
                score = 0
            }).ToList();
            highScores = convertedList;
        }
    }
}
