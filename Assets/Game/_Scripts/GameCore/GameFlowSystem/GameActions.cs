using System;

namespace GameCore.GameFlowSystem
{
    public class GameActions
    {
        internal static Action<bool> OnAnswerCheck;
        internal static Action<int> OnSubmitAnswer;
        internal static Action<int> OnScoreChanged;

        internal static Action OnGameOver;

        public static void AnswerCheck(bool isCorrect)
        {
            OnAnswerCheck?.Invoke(isCorrect);
        }

        public static void SubmitAnswer(int score)
        {
            OnSubmitAnswer?.Invoke(score);
        }

        public static void ScoreChanged(int score)
        {
            OnScoreChanged?.Invoke(score);
        }

        public static void GameOver()
        {
            OnGameOver?.Invoke();
        }
    }
}
