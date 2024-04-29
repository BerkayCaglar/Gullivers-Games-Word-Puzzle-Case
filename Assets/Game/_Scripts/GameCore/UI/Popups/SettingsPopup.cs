using System.Threading.Tasks;
using GameCore.GameFlowSystem;
using GameCore.PopupSystem;
using GameCore.ScoreSystem;
using GameCore.TileSystem.Managers;
using UnityEngine;

namespace GameCore.UI.Popups
{
    public class SettingsPopup : PopupPanel
    {
        public override void OnOpenPanel()
        {
            TileRaycastManager.LockTouch = true;
        }

        public override void OnClosePanel()
        {
            TileRaycastManager.LockTouch = false;
        }

        public async void OnClickEndTheGame()
        {
            PopupManager.Instance.ClosePopup(this);
            RuntimeScoreManager.Instance.SetCurrentScore(Random.Range(100, 500));
            await Task.Delay(500);
            GameManager.Instance.EndTheGame();
        }
    }
}
