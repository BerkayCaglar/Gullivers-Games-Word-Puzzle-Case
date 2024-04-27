using System;
using GameCore.InGame.TileSystem.Controllers;

namespace GameCore.TileSystem.Architecture
{
    public class TileActions
    {
        internal static Action<ITile> OnClickTile;
        internal static Action OnAnswerTilesChanged;
        internal static Action<bool> OnAnswerCheck;
        internal static Action<int> OnSubmitAnswer;
        internal static Action<int> OnScoreChanged;

        public static void ClickTile(ITile tileController)
        {
            OnClickTile?.Invoke(tileController);
        }

        public static void AnswerTilesChanged()
        {
            OnAnswerTilesChanged?.Invoke();
        }

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
    }
}
