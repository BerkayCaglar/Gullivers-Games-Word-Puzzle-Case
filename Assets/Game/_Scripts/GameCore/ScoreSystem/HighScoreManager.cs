using System;
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
        public void SetHighScore(HighScoreData highScoreData, out bool isHighScore)
        {
            isHighScore = false;
            int score = highScoreData.score;
            int level = highScoreData.level;

            var highScores = HighScoreJsonUtility.GetHighScoresFromJson();
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
                    HighScoreJsonUtility.WriteToJson(highScores);
                    isHighScore = true;
                }
            }
        }

        public int GetHighScore(int level)
        {
            var highScores = HighScoreJsonUtility.GetHighScoresFromJson();
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

        public void Setup(List<LevelData> newLevelDatas)
        {
            var currentHighScores = HighScoreJsonUtility.GetHighScoresFromJson();
            if (currentHighScores == null || currentHighScores.Count == 0)
                CreateNewHighScores(newLevelDatas);
            else
                UpdateHighScores(currentHighScores, newLevelDatas);
        }

        private void CreateNewHighScores(List<LevelData> newLevelDatas)
        {
            Debug.Log("Creating new high scores. Level count: " + newLevelDatas.Count);
            var highScores = new List<HighScoreData>();
            foreach (var levelData in newLevelDatas)
            {
                highScores.Add(new HighScoreData
                {
                    level = levelData.levelPoint,
                    score = 0
                });
            }
            HighScoreJsonUtility.WriteToJson(highScores);
        }

        private void UpdateHighScores(List<HighScoreData> currentHighScores, List<LevelData> newLevelDatas)
        {
            var isOutdated = currentHighScores.Count < newLevelDatas.Count;
            var isRedundancy = currentHighScores.Count > newLevelDatas.Count;

            var newHighScores = new List<HighScoreData>();

            if (isOutdated)
            {
                newHighScores.AddRange(newLevelDatas
                    .Where(levelData => currentHighScores.All(x => x.level != levelData.levelPoint))
                    .Select(levelData => new HighScoreData
                    {
                        level = levelData.levelPoint,
                        score = 0
                    }));
            }

            if (isRedundancy)
            {
                newHighScores = newHighScores.Except(newHighScores
                    .Where(x => newLevelDatas.All(levelData => x.level != levelData.levelPoint)))
                    .ToList();
            }

            if (newHighScores.Count > 0)
            {
                HighScoreJsonUtility.WriteToJson(newHighScores);
            }
        }

        [ButtonMethod]
        public void ResetHighScores()
        {
            var highScores = HighScoreJsonUtility.GetHighScoresFromJson();
            var resetedHighScores = new List<HighScoreData>();
            foreach (var highScore in highScores)
            {
                resetedHighScores.Add(new HighScoreData
                {
                    level = highScore.level,
                    score = 0
                });
            }
            HighScoreJsonUtility.WriteToJson(resetedHighScores);
        }
    }
}
