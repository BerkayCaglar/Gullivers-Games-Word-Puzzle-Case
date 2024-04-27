using GameCore.InGame.TileSystem.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameCore.UI.ButtonControllers
{
    public class UndoButtonController : MonoBehaviour
    {
        private void Awake()
        {
            var pointerDownEvent = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
            pointerDownEvent.callback.AddListener((data) => { OnUndoButtonPressed(); });
            GetComponent<EventTrigger>().triggers.Add(pointerDownEvent);

            var pointerUpEvent = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
            pointerUpEvent.callback.AddListener((data) => { OnUndoButtonReleased(); });
            GetComponent<EventTrigger>().triggers.Add(pointerUpEvent);
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
