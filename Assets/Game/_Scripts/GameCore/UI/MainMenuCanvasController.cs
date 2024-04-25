using System.Collections;
using System.Collections.Generic;
using GameCore.LevelSystem;
using GameCore.UI.Segments;
using UnityEngine;

namespace GameCore.UI
{
    public class MainMenuCanvasController : MonoBehaviour
    {
        [BHeader("References")]
        [SerializeField] private Transform content;

        [BHeader("Prefabs")]
        [SerializeField] private LevelSegment levelSegmentPrefab;

        private void Awake()
        {
            var levels = LevelManager.Instance.GetLevels();
            foreach (var level in levels)
            {
                var levelSegment = Instantiate(levelSegmentPrefab, content);
                levelSegment.Setup(new SegmentData
                {
                    LevelPoint = level.level,
                    LevelName = level.title,
                    LevelHighScore = "0"
                });
            }
        }
    }
}
