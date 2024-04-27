using GameCore.InGame.TileSystem.Managers.Answer;
using GameCore.TileSystem.Architecture;
using GameCore.TileSystem.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.UI.ButtonControllers
{
    public class SubmitButtonController : MonoBehaviour
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            TileActions.OnAnswerCheck += OnAnswerCheck;
        }

        private void OnDestroy()
        {
            TileActions.OnAnswerCheck -= OnAnswerCheck;
        }

        private void OnAnswerCheck(bool isCorrect)
        {
            switch (isCorrect)
            {
                case true:
                    _button.interactable = true;
                    break;
                case false:
                    _button.interactable = false;
                    break;
            }
        }

        public void OnClickSubmit()
        {
            if (AnswerTilesManager.Instance.IsAnswerTilesEmpty())
                return;
            var answer = AnswerTilesManager.Instance.GetAnswer();
            if (string.IsNullOrEmpty(answer))
                return;

            var score = TileScoreManager.GetWordScore(answer);
            Debug.Log($"Score: {score} for '{answer}'");
            TileActions.SubmitAnswer(score);
        }
    }
}
