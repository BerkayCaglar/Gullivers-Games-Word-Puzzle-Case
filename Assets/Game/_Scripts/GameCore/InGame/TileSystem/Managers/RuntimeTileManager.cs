using System.Collections.Generic;
using System.Linq;
using GameCore.InGame.TileSystem.Controllers;
using GameCore.LevelSystem;
using GameCore.Managers;
using GameCore.SingletonSystem;
using GameCore.TileSystem.Architecture;
using UnityEngine;

namespace GameCore.TileSystem.Managers
{
    public class RuntimeTileManager : AutoSingleton<RuntimeTileManager>
    {
        [BHeader("UI Elements")]
        [SerializeField] private Transform _currentTilesParent;

        [BHeader("Prefabs")]
        [SerializeField] private TileController _tilePrefab;

        private List<TileController> _tempTiles = new();

        private void Start()
        {
            var levelData = LevelManager.Instance.GetLevelTiles(PlayerManager.Instance.GetCurrentPlayingLevel());

            _tempTiles.AddRange(levelData.tiles.Select(tile =>
            {
                var tileController = Instantiate(_tilePrefab, _currentTilesParent);
                tileController.Setup(tile.character, TileState.NotUsing, tile.position);
                tileController.SetTileLayer(0);
                return tileController;
            }));

            foreach (var tile in levelData.tiles)
            {
                foreach (var childId in tile.children)
                {
                    _tempTiles[tile.id].AddChild(_tempTiles[childId]);
                    _tempTiles[childId].AddParent(_tempTiles[tile.id]);
                    _tempTiles[childId].SetTileState(TileState.Locked);
                }
            }

            SetTileLayers(_tempTiles);
        }

        private void SetTileLayers(List<TileController> tiles)
        {
            int layer = 0;
            foreach (var tile in tiles)
            {
                if (tile.GetTileElements().GetParentTiles().Count == 0)
                {
                    tile.SetTileLayer(layer);
                    SetChildLayers(tile, layer + 1);
                }
            }
        }

        private void SetChildLayers(TileController tile, int v)
        {
            foreach (var child in tile.GetTileElements().GetChildTiles())
            {
                child.SetTileLayer(v);
                SetChildLayers(child, v + 1);
            }
        }
    }
}
