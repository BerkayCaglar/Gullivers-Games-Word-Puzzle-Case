using System.Collections.Generic;
using System.Linq;
using GameCore.AnimationSystem;
using GameCore.GameFlowSystem;
using GameCore.InGame.TileSystem.Controllers;
using GameCore.LevelSystem;
using GameCore.PlayerJourneySystem;
using GameCore.SingletonSystem;
using GameCore.TileSystem.Architecture;
using UnityEngine;

namespace GameCore.TileSystem.Managers
{
    public class RuntimeTileManager : AutoSingleton<RuntimeTileManager>
    {
        [BHeader("Elements")]
        [SerializeField] private Transform _currentTilesParent;
        [SerializeField] private Transform _answerTilesParent;

        [BHeader("Prefabs")]
        [SerializeField] private TileController _tilePrefab;

        private List<TileController> _tiles = new();

        private void Start()
        {
            GameActions.OnGameOver += OnGameOver;

            var levelData = LevelManager.Instance.GetLevelTiles(PlayerManager.Instance.GetCurrentPlayingLevel());

            _tiles.AddRange(levelData.tiles.Select(tile =>
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
                    _tiles[tile.id].AddChild(_tiles[childId]);
                    _tiles[childId].AddParent(_tiles[tile.id]);
                    _tiles[childId].SetTileState(TileState.Locked);
                }
            }

            SetTileLayers(_tiles);
        }

        private void OnDestroy()
        {
            GameActions.OnGameOver -= OnGameOver;
        }

        private async void OnGameOver()
        {
            var activeTiles = GetActiveTiles();
            foreach (var tile in activeTiles)
            {
                tile.SetTileState(TileState.Used);
                await AnimationsManager.PlayExplosionAnimation(tile.transform);
            }
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

        public List<TileController> GetUnlockedTiles()
        {
            return _tiles.Where(tile => tile.GetTileState() == TileState.NotUsing || tile.GetTileState() == TileState.Using).ToList();
        }

        public List<TileController> GetActiveTiles()
        {
            return _tiles.Where(tile => tile.GetTileState() == TileState.Using || tile.GetTileState() == TileState.NotUsing || tile.GetTileState() == TileState.Locked).ToList();
        }

        public List<TileController> GetAllTiles()
        {
            return _tiles;
        }

        public bool IsAllTilesUsed()
        {
            return _tiles.All(tile => tile.GetTileState() == TileState.Used);
        }
    }
}
