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
        }

        private LevelTileData[] GetParentTiles(LevelTileData[] levelTileDatas)
        {
            return levelTileDatas.Where(tile => levelTileDatas.All(data => !data.children.Contains(tile.id))).ToArray();
        }

        private LevelTileData[] GetChildTiles(LevelTileData[] levelTileDatas)
        {
            return levelTileDatas.Where(data => data.children.Length > 0)
                                .SelectMany(data => data.children.Select(id => GetLevelTileDataFromChild(id, levelTileDatas)))
                                .ToArray();
        }

        private LevelTileData GetLevelTileDataFromChild(int id, LevelTileData[] levelTileDatas)
        {
            return levelTileDatas.FirstOrDefault(data => data.id == id);
        }
    }
}
