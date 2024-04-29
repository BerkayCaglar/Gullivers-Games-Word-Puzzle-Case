using System.Collections.Generic;
using System.Threading.Tasks;
using GameCore.LevelSystem;
using GameCore.PopupSystem;
using GameCore.ScoreSystem;
using GameCore.UI.Segments;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameCore.UI.Popups
{
    public class LevelsPopupController : PopupPanel
    {
        [BHeader("References")]
        [SerializeField] private RectTransform content;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private Button closeButton;

        [BHeader("Prefabs")]
        [SerializeField] private LevelSegment levelSegmentPrefab;

        private List<LevelSegment> _levelSegments = new List<LevelSegment>();

        private bool _isLevelUp;

        public override async void OnOpenPanel()
        {
            _isLevelUp = PlayerPrefs.HasKey("LevelUP");
            PlayerPrefs.DeleteKey("LevelUP");
            closeButton.interactable = !_isLevelUp;

            var levels = LevelManager.Instance.GetLevels();
            await SpawnLevelSegments(levels);

            if (_isLevelUp)
            {
                PlayLevelUpSequence();
            }
        }

        public override void OnClosePanel()
        {

        }

        private async Task SpawnLevelSegments(LevelData[] levels)
        {
            await Task.Delay(300);
            foreach (var level in levels)
            {
                var levelSegment = Instantiate(levelSegmentPrefab, content);
                levelSegment.transform.localScale = Vector3.zero;
                levelSegment.Setup(new SegmentData
                {
                    LevelPoint = level.levelPoint,
                    LevelName = level.title,
                    LevelHighScore = HighScoreManager.Instance.GetHighScore(level.levelPoint)
                }, _isLevelUp);
                _levelSegments.Add(levelSegment);
                levelSegment.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
                await Task.Delay(50);
            }
        }

        private async void PlayLevelUpSequence()
        {
            var newUnlockedLevel = _levelSegments.Find(x => x.SegmentState == SegmentState.Unlocked);
            if (newUnlockedLevel == null)
            {
                Debug.LogError("New unlocked level not found!");
                return;
            }

            var targetWorldPos = newUnlockedLevel.transform.position;
            var targetLocalY = -content.InverseTransformPoint(targetWorldPos).y;
            targetLocalY -= scrollRect.viewport.rect.height / 2;
            targetLocalY = Mathf.Clamp(targetLocalY, 0, scrollRect.content.rect.height - scrollRect.viewport.rect.height);

            await content.DOAnchorPosY(targetLocalY, 1f).SetEase(Ease.InOutSine).AsyncWaitForCompletion();

            await newUnlockedLevel.PlayUnlockTheLevelAnimation();
            newUnlockedLevel.transform.DOPunchScale(Vector3.one * 0.05f, 1f, 10, 1f).SetLoops(3, LoopType.Incremental);

            await Task.Delay(1000);
            closeButton.interactable = true;
        }

        [ContextMenu("SET LEVEL UP")]
        private void SetLevelUp()
        {
            PlayerPrefs.SetInt("LevelUP", 1);
        }
    }
}
