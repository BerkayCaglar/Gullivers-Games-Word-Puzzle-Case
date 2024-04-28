using DG.Tweening;
using GameCore.InGame.TileSystem.Controllers;
using GameCore.TileSystem.Architecture;
using UnityEngine;

namespace GameCore.TileSystem.Controllers
{
    public class AnswerTileController : MonoBehaviour, ITile
    {
        private AnswerTileElements _answerTileElements = new()
        {
            CurrentTileController = null,
            TileEmptyState = TileEmptyState.Empty,
            TileType = TileType.AnswerTile,
            TileState = TileState.None,
            TileOnActionState = TileOnActionState.None
        };

        internal void SetCurrentTileController(TileController tileController)
        {
            _answerTileElements.CurrentTileController = tileController;
            _answerTileElements.TileEmptyState = TileEmptyState.Filled;
        }

        internal void RemoveCurrentTileController()
        {
            _answerTileElements.CurrentTileController = null;
            _answerTileElements.TileEmptyState = TileEmptyState.Empty;
        }

        internal string GetCharacter()
        {
            switch (_answerTileElements.GetTileEmptyState())
            {
                case TileEmptyState.Empty:
                    Debug.LogError(string.Format(AnswerTileElements.CHARACTER_EMPTY_ERROR, gameObject.name));
                    return string.Empty;
                case TileEmptyState.Filled:
                    return _answerTileElements.GetCurrentTileController().GetCharacter();
                default:
                    Debug.LogError(string.Format(AnswerTileElements.CHARACTER_STATE_ERROR, gameObject.name));
                    return string.Empty;
            }
        }

        internal void Shake()
        {
            StopShake();
            if (GetState() == TileEmptyState.Empty) return;
            transform.DOPunchScale(Vector3.one * 0.03f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        }

        internal void StopShake()
        {
            transform.DOKill();
            transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        }

        #region Getters

        Transform ITile.GetTileTransform() => transform;

        public TileState GetTileState() => _answerTileElements.GetTileState();
        public TileType GetTileType() => _answerTileElements.GetTileType();
        public TileEmptyState GetTileEmptyState() => _answerTileElements.GetTileEmptyState();
        public TileOnActionState GetTileOnActionState() => _answerTileElements.GetTileOnActionState();
        public TileEmptyState GetState() => _answerTileElements.GetTileEmptyState();

        internal TileController GetCurrentTileController() => _answerTileElements.GetCurrentTileController();

        #endregion

        #region Setters

        public void SetTileOnActionState(TileOnActionState state) => _answerTileElements.TileOnActionState = state;
        public void SetTileEmptyState(TileEmptyState state) => _answerTileElements.TileEmptyState = state;
        public void SetTileState(TileState state) => _answerTileElements.TileState = state;
        public void SetTileType(TileType type) => _answerTileElements.TileType = type;

        #endregion
    }
}
