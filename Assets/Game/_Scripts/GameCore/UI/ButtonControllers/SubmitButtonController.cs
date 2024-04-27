using GameCore.InGame.TileSystem.Managers;
using UnityEngine;

namespace GameCore.UI.ButtonControllers
{
    public class SubmitButtonController : MonoBehaviour
    {
        public void OnClickSubmit()
        {
            if (AnswerTilesManager.Instance.IsAnswerTilesEmpty())
                return;
            //TileActions.SubmitAnswer();
        }
    }
}
