using UnityEngine;

namespace GameCore.TileSystem.Architecture
{
    public interface ITile
    {
        internal Transform GetTileTransform();

        public TileState GetTileState();
        public TileType GetTileType();
        public TileEmptyState GetTileEmptyState();
        public TileOnActionState GetTileOnActionState();

        public void SetTileOnActionState(TileOnActionState state);
        public void SetTileState(TileState state);
        public void SetTileEmptyState(TileEmptyState state);
        public void SetTileType(TileType type);
    }
}
