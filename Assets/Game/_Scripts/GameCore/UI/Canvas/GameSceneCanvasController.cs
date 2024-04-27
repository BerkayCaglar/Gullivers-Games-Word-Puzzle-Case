using System;
using System.Linq;
using DG.Tweening;
using GameCore.InGame.TileSystem;
using GameCore.InGame.TileSystem.Managers;
using GameCore.LevelSystem;
using GameCore.Managers;
using GameCore.PopupSystem;
using GameCore.TileSystem;
using GameCore.TileSystem.Architecture;
using GameCore.TileSystem.Controllers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameCore.UI.Canvas
{
    public class GameSceneCanvasController : UIPanel
    {
        [SerializeField] private GameObject _undoArea;

        public override void OnOpenPanel()
        {
            TileActions.OnAnswerTilesChanged += OnAnswerTilesChanged;
        }

        public override void OnClosePanel()
        {
            TileActions.OnAnswerTilesChanged -= OnAnswerTilesChanged;
        }

        private void OnDestroy()
        {
            TileActions.OnAnswerTilesChanged -= OnAnswerTilesChanged;
        }

        private Tween _undoTween;
        private void OnAnswerTilesChanged()
        {
            switch (AnswerTilesManager.Instance.IsAnswerTilesEmpty())
            {
                case true:
                    if (_undoTween != null) _undoTween.Kill();
                    _undoTween = _undoArea.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => { _undoArea.SetActive(false); _undoTween = null; });
                    break;
                case false:
                    if (_undoTween != null) _undoTween.Kill();
                    _undoArea.SetActive(true);
                    _undoTween = _undoArea.transform.DOScale(Vector3.one, 0.2f).OnComplete(() => _undoTween = null);
                    break;
            }
        }
    }
}
