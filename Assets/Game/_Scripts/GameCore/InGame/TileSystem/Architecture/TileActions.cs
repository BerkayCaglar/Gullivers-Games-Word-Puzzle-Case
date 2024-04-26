using System;
using GameCore.InGame.TileSystem.Controllers;

namespace GameCore.TileSystem.Architecture
{
    public class TileActions
    {
        internal static Action<ITile> OnClickTile;

        public static void ClickTile(ITile tileController)
        {
            OnClickTile?.Invoke(tileController);
        }
    }
}
