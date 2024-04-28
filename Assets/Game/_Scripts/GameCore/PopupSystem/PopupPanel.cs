using UnityEngine;

namespace GameCore.PopupSystem
{
    public abstract class PopupPanel : UIPanel
    {
        public GameObject ShadowBG;
        public GameObject PopupPanelGO;
        public PopupType PopupType;

        public virtual void ClosePopup()
        {
            PopupManager.Instance.ClosePopup(this);
        }
    }
}
