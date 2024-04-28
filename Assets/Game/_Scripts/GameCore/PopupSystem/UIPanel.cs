using UnityEngine;

namespace GameCore.PopupSystem
{
    public abstract class UIPanel : MonoBehaviour
    {
        public abstract void OnOpenPanel();
        public abstract void OnClosePanel();

        private void OnEnable()
        {
            OnOpenPanel();
        }

        private void OnDisable()
        {
            OnClosePanel();
        }

        public void OpenPopup(string popupType)
        {
            PopupManager.Instance.OpenPopup(popupType, popupParent: this.gameObject);
        }
    }
}
