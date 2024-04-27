using GameCore.Managers;
using GameCore.SingletonSystem;
using GameCore.TileSystem.Architecture;

namespace GameCore.HighScoreSystem
{
    public class RuntimeScoreManager : AutoSingleton<RuntimeScoreManager>
    {
        private int _currentScore;
        private void Start()
        {
            TileActions.OnSubmitAnswer += OnSubmitAnswer;
        }

        private void OnDestroy()
        {
            TileActions.OnSubmitAnswer -= OnSubmitAnswer;
        }

        private void OnSubmitAnswer(int score)
        {
            _currentScore += score;
            TileActions.ScoreChanged(_currentScore);
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
