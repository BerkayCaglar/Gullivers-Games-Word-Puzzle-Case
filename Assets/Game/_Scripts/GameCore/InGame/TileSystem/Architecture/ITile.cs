using UnityEngine;

namespace GameCore.TileSystem.Architecture
{
    public interface ITile
    {
        internal Transform GetTileTransform();

        internal TileState GetTileState();
        internal TileType GetTileType();
        internal TileEmptyState GetTileEmptyState();
        internal TileOnActionState GetTileOnActionState();

        public void SetTileOnActionState(TileOnActionState state);
        public void SetTileState(TileState state);
        public void SetTileEmptyState(TileEmptyState state);
        public void SetTileType(TileType type);
    }
}
