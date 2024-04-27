using System.Collections.Generic;
using DG.Tweening;
using GameCore.TileSystem.Architecture;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.InGame.TileSystem.Controllers
{
    public class TileController : MonoBehaviour, ITile
    {
        [SerializeField] private TextMeshPro _characterText;
        [SerializeField] private SpriteRenderer _tileSpriteRenderer;

        private TileElements _tileElements = new()
        {
            ChildTiles = new List<TileController>(),
            ParentTiles = new List<TileController>(),
            Character = string.Empty,
            InitialValues = new TileElements._InitialValues(null, Vector3.zero, Vector3.zero),
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
            _tileElements.InitialValues = new TileElements._InitialValues(transform.parent, transform.localPosition, transform.localScale);
        }

        #region Getters

        internal void AddChild(TileController tileController) => _tileElements.ChildTiles.Add(tileController);
        internal void AddParent(TileController tileController) => _tileElements.ParentTiles.Add(tileController);

        internal TileElements._InitialValues GetInit() => _tileElements.GetInitialValues();
        internal string GetCharacter() => _tileElements.GetCharacter();

        public TileState GetTileState() => _tileElements.GetTileState();
        public TileType GetTileType() => _tileElements.GetTileType();
        public TileEmptyState GetTileEmptyState() => _tileElements.GetTileEmptyState();
        public TileOnActionState GetTileOnActionState() => _tileElements.GetTileOnActionState();

        Transform ITile.GetTileTransform() => transform;

        internal TileElements GetTileElements() => _tileElements;

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

        private Tween _colorTween = null;
        private void SetTileVisuals()
        {
            switch (_tileElements.GetTileState())
            {
                case TileState.Locked:
                    if (_colorTween != null)
                    {
                        _colorTween.Kill();
                        _colorTween = null;
                        _tileSpriteRenderer.color = new Color(1, 0.95f, 0.65f, 1);
                    }
                    _colorTween = DOTween.To(() => _tileSpriteRenderer.color, x => _tileSpriteRenderer.color = x, Color.gray, 0.1f);
                    _colorTween.OnComplete(() => _colorTween = null);
                    break;
                case TileState.NotUsing:
                    if (_colorTween != null)
                    {
                        _colorTween.Kill();
                        _colorTween = null;
                        _tileSpriteRenderer.color = Color.gray;
                    }
                    _colorTween = DOTween.To(() => _tileSpriteRenderer.color, x => _tileSpriteRenderer.color = x, new Color(1, 0.95f, 0.65f, 1), 0.2f).SetEase(Ease.InCubic);
                    _colorTween.OnComplete(() => _colorTween = null);
                    break;
            }
        }

        internal void SetChildTiles()
        {
            TileState currentState = _tileElements.GetTileState();

            foreach (var child in _tileElements.GetAllChildTiles())
            {
                if (currentState == TileState.Using)
                {
                    //Debug.Log($"<color=red>[TILE MANAGER]</color> Child: <color=yellow>{child.GetCharacter()}</color>");
                    child._tileElements.ParentTiles.Remove(this);

                    if (child._tileElements.GetParentTiles().Count == 0)
                        child.SetTileState(TileState.NotUsing);
                }
                else if (currentState == TileState.NotUsing)
                {
                    //Debug.Log($"<color=green>[TILE MANAGER]</color> Child: <color=yellow>{child.GetCharacter()}</color>");
                    child._tileElements.ParentTiles.Add(this);

                    if (child._tileElements.GetParentTiles().Count > 0)
                        child.SetTileState(TileState.Locked);
                }
            }
        }

        #endregion
    }
}
