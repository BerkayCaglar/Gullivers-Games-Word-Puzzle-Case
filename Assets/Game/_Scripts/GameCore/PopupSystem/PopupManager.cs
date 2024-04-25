using System.Collections.Generic;
using UnityEngine;
using System;
using GameCore.SingletonSystem;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

namespace GameCore.PopupSystem
{
    [CreateAssetMenu(fileName = "PopupManager", menuName = "ScriptableObjects/PopupManager")]
    public class PopupManager : ScriptableResourceSingleton<PopupManager>
    {
        public List<Popup> _popups = new List<Popup>();

        private GameObject GetPopupPrefab(PopupType popupType)
        {
            foreach (var popup in _popups)
            {
                if (popup.PopupType == popupType)
                {
                    return popup.PopupPrefab;
                }
            }
            Debug.LogError($"Popup with type {popupType} not found! PopupCount: {_popups.Count}");
            return null;
        }
        public void OpenPopup(string popupType, GameObject popupParent)
        {
            PopupType popupTypeEnum;
            if (Enum.TryParse(popupType, out popupTypeEnum))
            {
                var popupPrefab = GetPopupPrefab(popupTypeEnum);
                if (popupPrefab != null)
                {
                    var popup = Instantiate(popupPrefab, popupParent.transform);
                    var PopupPanel = popup.GetComponent<PopupPanel>();

                    PopupOpeningAnimation(PopupPanel.PopupPanelGO, PopupPanel.ShadowBG);
                }
            }
            else
            {
                Debug.LogError($"Popup with type {popupType} not found!");
            }
        }

        public void ClosePopup(PopupPanel popupPanel)
        {
            PopupClosingAnimation(popupPanel);
        }

        private void PopupOpeningAnimation(GameObject popup, GameObject shadowBG)
        {
            var shadowBGImage = shadowBG.GetComponent<Image>();

            shadowBGImage.DOFade(0, 0);
            popup.transform.DOScale(Vector3.zero, 0);

            var sequence = DOTween.Sequence();

            sequence.Append(shadowBGImage.DOFade(1f, 0.4f).SetEase(Ease.InFlash));

            sequence.Join(popup.transform.DOScale(Vector3.one, 0.4f));

            sequence.OnComplete(() => popup.transform.DOPunchScale(Vector3.one * 0.1f, 0.5f, 10, 1f));
        }

        private void PopupClosingAnimation(PopupPanel popupPanel)
        {
            var canvasGroup = popupPanel.GetComponent<CanvasGroup>();

            var sequence = DOTween.Sequence();

            sequence.Append(canvasGroup.DOFade(0, 0.4f).SetEase(Ease.InFlash));

            sequence.OnComplete(() => DestroyPopup(popupPanel));
        }

        private void DestroyPopup(PopupPanel popupPanel)
        {
            Destroy(popupPanel.gameObject);
        }
    }

    [Serializable]
    public class Popup
    {
        public PopupType PopupType;
        public GameObject PopupPrefab;
    }

    [Serializable]
    public enum PopupType
    {
        LevelsPopup
    }
}
