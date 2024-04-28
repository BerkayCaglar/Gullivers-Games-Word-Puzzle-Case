using GameCore.GameFlowSystem;
using GameCore.Managers;
using GameCore.SingletonSystem;
using UnityEngine;

namespace GameCore.ScoreSystem
{
    public class RuntimeScoreManager : AutoSingleton<RuntimeScoreManager>
    {
        private int _currentScore;
        private void Start()
        {
            GameActions.OnSubmitAnswer += OnSubmitAnswer;
            GameActions.OnGameOver += OnGameOver;
        }

        private void OnDestroy()
        {
            GameActions.OnSubmitAnswer -= OnSubmitAnswer;
            GameActions.OnGameOver -= OnGameOver;
        }

        private void OnSubmitAnswer(int score)
        {
            _currentScore += score;
            GameActions.ScoreChanged(_currentScore);
        }

        private void OnGameOver()
        {
            var highScoreData = new HighScoreData
            {
                score = _currentScore,
                level = PlayerManager.Instance.GetCurrentPlayingLevel()
            };
            Debug.Log($"HighScoreData: {highScoreData.score} - {highScoreData.level}");
            SetHighScore(highScoreData);
        }

        private void SetHighScore(HighScoreData highScoreData)
        {
            HighScoreManager.Instance.SetHighScore(highScoreData);
        }

        public int GetCurrentScore()
        {
            return _currentScore;
        }
    }
}
