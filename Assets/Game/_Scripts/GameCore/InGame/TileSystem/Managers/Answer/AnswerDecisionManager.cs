using GameCore.TileSystem.Architecture;
using UnityEngine;

namespace GameCore.InGame.TileSystem.Managers.Answer
{
    public class AnswerDecisionManager : MonoBehaviour
    {
        private void Awake()
        {
            TileActions.OnAnswerTilesChanged += OnAnswerTilesChanged;
        }

        private void OnDestroy()
        {
            TileActions.OnAnswerTilesChanged -= OnAnswerTilesChanged;
        }

        private async void OnAnswerTilesChanged()
        {
            if (AnswerTilesManager.Instance.IsAnswerTilesEmpty()) return;
            var answer = AnswerTilesManager.Instance.GetAnswer();
            if (string.IsNullOrEmpty(answer)) return;
            var isOnDatabase = await PossibleWordsSystem.PossibleWordsGenerator.IsCorrectWordAsync(answer.ToLowerInvariant());
            Debug.Log($"Is '{answer}' on database: {isOnDatabase}");
            TileActions.AnswerCheck(isOnDatabase);
        }
    }
}
