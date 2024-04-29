using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCore.PopupSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameCore.UI.Canvas
{
    public class MainMenuCanvasController : UIPanel
    {
        public override void OnOpenPanel()
        {

        }

        public override void OnClosePanel()
        {

        }

        private void Start()
        {
            if (PlayerPrefs.HasKey("LevelUP"))
            {
                StartCoroutine(OpenPopupCoroutine("LevelsPopup"));
            }
        }

        private IEnumerator OpenPopupCoroutine(string popupName)
        {
            yield return new WaitForSeconds(1f);
            OpenPopup(popupName);
        }
    }
}
