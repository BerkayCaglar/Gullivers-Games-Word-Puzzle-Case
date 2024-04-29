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
        [Header("References")]
        [SerializeField] private Button _levelsButton;

        public override void OnOpenPanel()
        {
            _levelsButton.onClick.AddListener(() =>
            {
                StartCoroutine(LevelsButtonInteractableCoroutine());
            });
        }

        public override void OnClosePanel()
        {

        }

        private void Start()
        {
            if (PlayerPrefs.HasKey("LevelUP"))
            {
                _levelsButton.interactable = false;
                StartCoroutine(OpenPopupCoroutine("LevelsPopup"));
            }
        }

        private IEnumerator OpenPopupCoroutine(string popupName)
        {
            yield return new WaitForSeconds(1f);
            OpenPopup(popupName);
            StartCoroutine(LevelsButtonInteractableCoroutine());
        }

        private IEnumerator LevelsButtonInteractableCoroutine()
        {
            _levelsButton.interactable = false;
            yield return new WaitForSeconds(1f);
            _levelsButton.interactable = true;
        }
    }
}
