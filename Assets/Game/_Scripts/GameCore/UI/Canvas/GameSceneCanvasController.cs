using System;
using System.Linq;
using DG.Tweening;
using GameCore.HighScoreSystem;
using GameCore.InGame.TileSystem;
using GameCore.InGame.TileSystem.Managers;
using GameCore.InGame.TileSystem.Managers.Answer;
using GameCore.LevelSystem;
using GameCore.Managers;
using GameCore.PopupSystem;
using GameCore.TileSystem;
using GameCore.TileSystem.Architecture;
using GameCore.TileSystem.Controllers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameCore.UI.Canvas
{
    public class GameSceneCanvasController : UIPanel
    {
        [SerializeField] private GameObject _undoArea;
        [SerializeField] private GameObject _submitArea;

        [SerializeField] private TextMeshProUGUI _scoreText;

        public override void OnOpenPanel()
        {
            _scoreText.text = $"0";
            TileActions.OnAnswerTilesChanged += OnAnswerTilesChanged;
            TileActions.OnScoreChanged += OnScoreChanged;
        }

        public override void OnClosePanel()
        {
            TileActions.OnAnswerTilesChanged -= OnAnswerTilesChanged;
            TileActions.OnScoreChanged -= OnScoreChanged;
        }

        private void OnDestroy()
        {
            TileActions.OnAnswerTilesChanged -= OnAnswerTilesChanged;
            TileActions.OnScoreChanged -= OnScoreChanged;
        }

        private void OnAnswerTilesChanged()
        {
            switch (AnswerTilesManager.Instance.IsAnswerTilesEmpty())
            {
                case true:
                    _undoArea.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => { _undoArea.SetActive(false); });
                    _submitArea.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => { _submitArea.SetActive(false); }).SetDelay(0.2f);
                    break;
                case false:
                    _undoArea.SetActive(true);
                    _submitArea.SetActive(true);
                    _undoArea.transform.DOScale(Vector3.one, 0.2f);
                    _submitArea.transform.DOScale(Vector3.one, 0.2f).SetDelay(0.2f);
                    break;
            }
        }

        private void OnScoreChanged(int score)
        {
            _scoreText.text = $"{RuntimeScoreManager.Instance.GetCurrentScore()}";
            if (!DOTween.IsTweening(_scoreText.transform))
                _scoreText.transform.DOPunchScale(Vector3.one * 0.2f, 0.5f);
            _undoArea.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => { _undoArea.SetActive(false); });
            _submitArea.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => { _submitArea.SetActive(false); }).SetDelay(0.2f);
        }
    }
}
