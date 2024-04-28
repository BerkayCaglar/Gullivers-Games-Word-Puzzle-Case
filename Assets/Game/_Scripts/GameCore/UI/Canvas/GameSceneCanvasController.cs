using System.Threading.Tasks;
using DG.Tweening;
using GameCore.GameFlowSystem;
using GameCore.InGame.TileSystem.Managers.Answer;
using GameCore.LevelSystem;
using GameCore.Managers;
using GameCore.PlayerJourneySystem;
using GameCore.PopupSystem;
using GameCore.ScoreSystem;
using GameCore.TileSystem.Architecture;
using TMPro;
using UnityEngine;

namespace GameCore.UI.Canvas
{
    public class GameSceneCanvasController : UIPanel
    {
        [SerializeField] private GameObject _undoArea;
        [SerializeField] private GameObject _submitArea;

        [SerializeField] private TextMeshProUGUI _scoreTitleText;

        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _titleText;

        [SerializeField] private Animator _gameOverAnimator;

        public override void OnOpenPanel()
        {
            _titleText.text = $"{LevelManager.Instance.GetLevelData(PlayerManager.Instance.GetCurrentPlayingLevel()).title}";
            _scoreText.text = $"0";
            TileActions.OnAnswerTilesChanged += OnAnswerTilesChanged;
            GameActions.OnScoreChanged += OnScoreChanged;
            GameActions.OnSubmitAnswer += OnSubmitAnswer;
            GameActions.OnGameOver += OnGameOver;
        }

        public override void OnClosePanel()
        {
            TileActions.OnAnswerTilesChanged -= OnAnswerTilesChanged;
            GameActions.OnScoreChanged -= OnScoreChanged;
            GameActions.OnSubmitAnswer -= OnSubmitAnswer;
            GameActions.OnGameOver -= OnGameOver;
        }

        private void OnDestroy()
        {
            TileActions.OnAnswerTilesChanged -= OnAnswerTilesChanged;
            GameActions.OnScoreChanged -= OnScoreChanged;
            GameActions.OnSubmitAnswer -= OnSubmitAnswer;
            GameActions.OnGameOver -= OnGameOver;
        }

        private async void OnGameOver()
        {
            HideButtons();
            await Task.Delay(1500);
            var endWithHighScore = RuntimeScoreManager.Instance.IsEndWithHighScore();
            _scoreTitleText.text = endWithHighScore ? "NEW HIGH SCORE" : "SCORE";
            _gameOverAnimator.SetTrigger(endWithHighScore ? "HighScoreCelebrate" : "NormalCelebrate");
        }

        private void OnSubmitAnswer(int score)
        {
            HideButtons();
        }

        private void OnAnswerTilesChanged()
        {
            switch (AnswerTilesManager.Instance.IsAnswerTilesEmpty())
            {
                case true:
                    HideButtons();
                    break;
                case false:
                    ShowButtons();
                    break;
            }
        }

        private void OnScoreChanged(int score)
        {
            _scoreText.text = $"{RuntimeScoreManager.Instance.GetCurrentScore()}";
            if (!DOTween.IsTweening(_scoreText.transform))
                _scoreText.transform.DOPunchScale(Vector3.one * 0.2f, 0.5f);
            HideButtons();
        }

        private void HideButtons()
        {
            if (DOTween.IsTweening(_undoArea.transform)) { _undoArea.transform.DOKill(); }
            if (DOTween.IsTweening(_submitArea.transform)) { _submitArea.transform.DOKill(); }
            if (!_undoArea.activeSelf && !_submitArea.activeSelf)
            {
                _undoArea.transform.localScale = Vector3.zero;
                _submitArea.transform.localScale = Vector3.zero;
                return;
            }
            _undoArea.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => { _undoArea.SetActive(false); });
            _submitArea.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => { _submitArea.SetActive(false); }).SetDelay(0.2f);
        }

        private void ShowButtons()
        {
            if (DOTween.IsTweening(_undoArea.transform)) { _undoArea.transform.DOKill(); }
            if (DOTween.IsTweening(_submitArea.transform)) { _submitArea.transform.DOKill(); }
            if (_undoArea.activeSelf && _submitArea.activeSelf)
            {
                _undoArea.transform.localScale = Vector3.one;
                _submitArea.transform.localScale = Vector3.one;
                return;
            }
            _undoArea.SetActive(true);
            _submitArea.SetActive(true);
            _undoArea.transform.DOScale(Vector3.one, 0.2f);
            _submitArea.transform.DOScale(Vector3.one, 0.2f).SetDelay(0.2f);
        }

        public async void OnOKButtonClicked()
        {
            await SceneControlManager.Instance.LoadSceneWithFadeInAnimation(SceneName.MainMenu, useLoadingScene: false);
        }
    }
}
