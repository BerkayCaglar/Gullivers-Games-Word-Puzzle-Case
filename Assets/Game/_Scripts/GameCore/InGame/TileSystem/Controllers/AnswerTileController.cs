using System;
using System.Collections;
using System.Collections.Generic;
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

        #region Getters

        Transform ITile.GetTileTransform() => transform;

        TileState ITile.GetTileState() => _answerTileElements.GetTileState();
        TileType ITile.GetTileType() => _answerTileElements.GetTileType();
        TileEmptyState ITile.GetTileEmptyState() => _answerTileElements.GetTileEmptyState();
        TileOnActionState ITile.GetTileOnActionState() => _answerTileElements.GetTileOnActionState();

        internal TileEmptyState GetState() => _answerTileElements.GetTileEmptyState();

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
