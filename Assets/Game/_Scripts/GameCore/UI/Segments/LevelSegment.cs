using System.Threading.Tasks;
using DG.Tweening;
using GameCore.Managers;
using GameCore.PlayerJourneySystem;
using GameCore.ScoreSystem;
using MyBox;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.UI.Segments
{
    public class LevelSegment : MonoBehaviour
    {
        [BHeader("GameObjects")]
        [SerializeField] private GameObject highScoreLockIcon;
        [SerializeField] private GameObject playButtonLockIcon;
        [SerializeField] private GameObject playButtonTextObject;
        [SerializeField] private GameObject shadowObject;

        [BHeader("Texts")]
        [SerializeField] private TextMeshProUGUI levelPointText;
        [SerializeField] private TextMeshProUGUI levelNameText;
        [SerializeField] private TextMeshProUGUI levelHighScoreText;

        [BHeader("Buttons")]
        [SerializeField] private Button playButton;

        [BHeader("Segment State")]
        [SerializeField] private SegmentState segmentState;

        public SegmentState SegmentState => segmentState;
        public int LevelPoint => int.Parse(levelPointText.text);

        public void Setup(SegmentData segmentData, bool isLevelUp)
        {
            levelPointText.text = segmentData.LevelPoint.ToString();
            levelNameText.text = segmentData.LevelName;
            levelHighScoreText.text = segmentData.LevelHighScore.ToString();

            var isLevelEnoughToUnlock = PlayerManager.Instance.IsCurrentPlayerLevelEnoughToUnlockLevel(segmentData.LevelPoint);
            var isLevelHasHighScore = HighScoreManager.Instance.IsLevelHasHighScore(segmentData.LevelPoint);

            segmentState = (isLevelHasHighScore, isLevelEnoughToUnlock) switch
            {
                (true, true) => SegmentState.Completed,
                (false, true) => SegmentState.Unlocked,
                _ => SegmentState.Locked,
            };

            if (segmentState == SegmentState.Unlocked && isLevelUp)
            {
                shadowObject.SetActive(true);

                highScoreLockIcon.SetActive(true);
                levelHighScoreText.gameObject.SetActive(false);

                playButtonLockIcon.SetActive(true);
                playButtonTextObject.SetActive(false);

                playButton.interactable = false;
                return;
            }

            shadowObject.SetActive(!isLevelEnoughToUnlock);

            highScoreLockIcon.SetActive(!isLevelEnoughToUnlock);
            levelHighScoreText.gameObject.SetActive(isLevelEnoughToUnlock);

            playButtonLockIcon.SetActive(!isLevelEnoughToUnlock);
            playButtonTextObject.SetActive(isLevelEnoughToUnlock);

            playButton.interactable = isLevelEnoughToUnlock;
        }

        public async Task PlayUnlockTheLevelAnimation()
        {
            if (segmentState != SegmentState.Unlocked) return;

            var shadowImage = shadowObject.GetComponent<Image>();
            shadowImage.DOFade(0, 0.4f).SetEase(Ease.InBack).OnComplete(() =>
            {
                shadowObject.SetActive(false);
            });

            await highScoreLockIcon.transform.DOPunchScale(Vector3.one * 0.1f, 0.5f, 10, 1f).OnComplete(() =>
            {
                highScoreLockIcon.transform.DOScale(Vector3.one * 0.2f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
                {
                    highScoreLockIcon.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
                    {
                        levelHighScoreText.transform.localScale = Vector3.zero;
                        levelHighScoreText.gameObject.SetActive(true);
                        levelHighScoreText.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
                        highScoreLockIcon.SetActive(false);
                    });
                });
            }).AsyncWaitForCompletion();

            await playButtonLockIcon.transform.DOPunchScale(Vector3.one * 0.1f, 0.5f, 10, 1f).OnComplete(() =>
            {
                playButtonLockIcon.transform.DOScale(Vector3.one * 0.2f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
                {
                    playButtonLockIcon.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
                    {
                        playButtonTextObject.transform.localScale = Vector3.zero;
                        playButtonTextObject.SetActive(true);
                        playButtonTextObject.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
                        playButtonLockIcon.SetActive(false);
                    });
                });
            }).AsyncWaitForCompletion();

            await Task.Delay(1000);

            playButton.interactable = true;
        }

        public async void OnClickPlayButton()
        {
            if (segmentState == SegmentState.Locked)
            {
                Debug.Log("Level is locked!");
                return;
            }

            await PlayerManager.Instance.SetCurrentPlayingLevel(int.Parse(levelPointText.text));
            await SceneControlManager.Instance.LoadSceneWithFadeInAnimation(SceneName.GameScene, useLoadingScene: false);
        }
    }
}
