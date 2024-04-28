using GameCore.GameFlowSystem;
using GameCore.InGame.TileSystem.Managers.Answer;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameCore.UI.ButtonControllers
{
    public class UndoButtonController : MonoBehaviour
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();

            GameActions.OnSubmitAnswer += OnSubmitAnswer;

            var pointerDownEvent = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
            pointerDownEvent.callback.AddListener((data) => { OnUndoButtonPressed(); });
            GetComponent<EventTrigger>().triggers.Add(pointerDownEvent);

            var pointerUpEvent = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
            pointerUpEvent.callback.AddListener((data) => { OnUndoButtonReleased(); });
            GetComponent<EventTrigger>().triggers.Add(pointerUpEvent);
        }

        private void OnDestroy()
        {
            GameActions.OnSubmitAnswer -= OnSubmitAnswer;
        }

        private void OnSubmitAnswer(int score)
        {
            _button.interactable = false;
        }

        private void OnEnable()
        {
            _button.interactable = true;
        }

        public void OnClickUndo()
        {
            if (AnswerTilesManager.Instance.IsAnswerTilesEmpty())
                return;
            AnswerTilesManager.Instance.UndoSingleAnswerTile();
        }

        public void OnUndoButtonPressed()
        {
            if (AnswerTilesManager.Instance.IsAnswerTilesEmpty())
                return;
            AnswerTilesManager.Instance.UndoMultipleAnswerTiles();
        }

        public void OnUndoButtonReleased()
        {
            AnswerTilesManager.Instance.StopUndoMultipleAnswerTiles();
        }
    }
}
