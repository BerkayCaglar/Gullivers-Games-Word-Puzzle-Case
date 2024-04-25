using GameCore.Managers;
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

        [BHeader("Texts")]
        [SerializeField] private TextMeshProUGUI levelPointText;
        [SerializeField] private TextMeshProUGUI levelNameText;
        [SerializeField] private TextMeshProUGUI levelHighScoreText;

        [BHeader("Buttons")]
        [SerializeField] private Button playButton;

        [BHeader("Segment State")]
        [SerializeField] private SegmentState segmentState;

        public void Setup(SegmentData segmentData)
        {
            levelPointText.text = segmentData.LevelPoint.ToString();
            levelNameText.text = segmentData.LevelName;
            levelHighScoreText.text = segmentData.LevelHighScore;

            var isLevelEnoughToUnlock = PlayerManager.Instance.IsCurrentLevelEnoughToUnlockLevel(segmentData.LevelPoint);

            highScoreLockIcon.SetActive(!isLevelEnoughToUnlock);
            levelHighScoreText.gameObject.SetActive(isLevelEnoughToUnlock);

            playButtonLockIcon.SetActive(!isLevelEnoughToUnlock);
            playButtonTextObject.SetActive(isLevelEnoughToUnlock);

            playButton.interactable = isLevelEnoughToUnlock;

            segmentState = isLevelEnoughToUnlock ? SegmentState.Unlocked : SegmentState.Locked;
        }
    }
}
