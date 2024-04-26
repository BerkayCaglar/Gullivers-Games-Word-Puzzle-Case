using System.Collections;
using System.Collections.Generic;
using GameCore.LevelSystem;
using GameCore.PopupSystem;
using GameCore.UI.Segments;
using UnityEngine;

namespace GameCore.UI.Popups
{
    public class LevelsPopupController : PopupPanel
    {
        [BHeader("References")]
        [SerializeField] private Transform content;

        [BHeader("Prefabs")]
        [SerializeField] private LevelSegment levelSegmentPrefab;

        public override void OnOpenPanel()
        {
            var levels = LevelManager.Instance.GetLevels();
            foreach (var level in levels)
            {
                var levelSegment = Instantiate(levelSegmentPrefab, content);
                levelSegment.Setup(new SegmentData
                {
                    LevelPoint = level.levelPoint,
                    LevelName = level.title,
                    LevelHighScore = HighScoreSystem.HighScoreManager.Instance.GetHighScore(level.levelPoint)
                });
            }
        }

        public override void OnClosePanel()
        {

        }
    }
}
