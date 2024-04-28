using System;

namespace GameCore.TileSystem.Architecture
{
    public class TileActions
    {
        internal static Action<ITile> OnClickTile;
        internal static Action OnAnswerTilesChanged;

        public static void ClickTile(ITile tileController)
        {
            OnClickTile?.Invoke(tileController);
        }

        public static void AnswerTilesChanged()
        {
            OnAnswerTilesChanged?.Invoke();
        }
    }
}
