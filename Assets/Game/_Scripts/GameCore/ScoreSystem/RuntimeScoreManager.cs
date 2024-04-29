using GameCore.GameFlowSystem;
using GameCore.PlayerJourneySystem;
using GameCore.SingletonSystem;

namespace GameCore.ScoreSystem
{
    public class RuntimeScoreManager : AutoSingleton<RuntimeScoreManager>
    {
        private int _currentScore;
        private bool _isEndWithHighScore;

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
            SetHighScore(highScoreData);
        }

        private void SetHighScore(HighScoreData highScoreData)
        {
            HighScoreManager.Instance.SetHighScore(highScoreData, out _isEndWithHighScore);
        }

        public int GetCurrentScore()
        {
            return _currentScore;
        }

        public void SetCurrentScore(int score)
        {
            _currentScore = score;
        }

        public bool IsEndWithHighScore()
        {
            return _isEndWithHighScore;
        }
    }
}
