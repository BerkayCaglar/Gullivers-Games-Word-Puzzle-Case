using System.Collections.Generic;
using GameCore.TileSystem.Architecture;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.InGame.TileSystem.Controllers
{
    public class TileController : MonoBehaviour, ITile
    {
        [SerializeField] private TextMeshPro _characterText;
        [SerializeField] private Image _tileBackground;
        [SerializeField] private SpriteRenderer _tileSpriteRenderer;

        private TileElements _tileElements = new()
        {
            ChildTiles = new List<TileController>(),
            ParentTiles = new List<TileController>(),
            Character = string.Empty,
            InitialTransform = null,
            TileState = TileState.Locked,
            TileType = TileType.Tile,
            TileEmptyState = TileEmptyState.Filled,
            TileOnActionState = TileOnActionState.None
        };

        internal void Setup(string character, TileState tileState, Vector3 position)
        {
            _characterText.text = character;
            transform.localPosition = new Vector3(position.x / 10, position.y / 10, position.z / 10);

            _tileElements.Character = character;
            _tileElements.TileState = tileState;
            _tileElements.InitialTransform = transform;
        }

        #region Getters

        internal void AddChild(TileController tileController) => _tileElements.ChildTiles.Add(tileController);
        internal void AddParent(TileController tileController) => _tileElements.ParentTiles.Add(tileController);

        internal Transform GetInit() => _tileElements.GetInitialTransform();
        internal string GetCharacter() => _tileElements.GetCharacter();

        TileState ITile.GetTileState() => _tileElements.GetTileState();
        TileType ITile.GetTileType() => _tileElements.GetTileType();
        TileEmptyState ITile.GetTileEmptyState() => _tileElements.GetTileEmptyState();
        TileOnActionState ITile.GetTileOnActionState() => _tileElements.GetTileOnActionState();

        Transform ITile.GetTileTransform() => transform;

        #endregion

        #region Setters

        public void SetTileOnActionState(TileOnActionState state) => _tileElements.TileOnActionState = state;
        public void SetTileEmptyState(TileEmptyState state) => _tileElements.TileEmptyState = state;
        public void SetTileType(TileType type) => _tileElements.TileType = type;
        public void SetTileState(TileState state)
        {
            _tileElements.TileState = state;
            SetTileVisuals();
        }
        private void SetTileVisuals()
        {
            switch (_tileElements.GetTileState())
            {
                case TileState.Locked:
                    _tileSpriteRenderer.color = Color.gray;
                    break;
                case TileState.NotUsing:
                    _tileSpriteRenderer.color = new Color(1, 0.95f, 0.65f, 1);
                    break;
            }
        }

        #endregion
    }
}
