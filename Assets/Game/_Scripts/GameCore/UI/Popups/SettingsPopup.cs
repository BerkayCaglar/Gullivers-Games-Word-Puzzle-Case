using System.Threading.Tasks;
using GameCore.GameFlowSystem;
using GameCore.PopupSystem;
using GameCore.TileSystem.Managers;

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
            await Task.Delay(500);
            GameActions.GameOver();
        }
    }
}
