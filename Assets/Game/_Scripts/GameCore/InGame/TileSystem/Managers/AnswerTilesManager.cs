using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCore.InGame.TileSystem.Controllers;
using GameCore.SingletonSystem;
using GameCore.TileSystem.Architecture;
using GameCore.TileSystem.Controllers;
using MyBox;
using UnityEngine;

namespace GameCore.InGame.TileSystem.Managers
{
    public class AnswerTilesManager : AutoSingleton<AnswerTilesManager>
    {
        [SerializeField] private AnswerTileController[] _answerTileControllers;

        internal AnswerTileController SetAnswerTileController(TileController tileController)
        {
            for (int i = 0; i < _answerTileControllers.Length; i++)
            {
                if (_answerTileControllers[i].GetState() == TileEmptyState.Empty && _answerTileControllers[i].GetTileOnActionState() == TileOnActionState.None)
                {
                    _answerTileControllers[i].SetCurrentTileController(tileController);
                    TileActions.AnswerTilesChanged();
                    return _answerTileControllers[i];
                }
            }

            return null;
        }

        internal void RemoveAnswerTileController(AnswerTileController answerTileController)
        {
            answerTileController.RemoveCurrentTileController();
            TileActions.AnswerTilesChanged();
        }

        internal void ResetAnswerTiles()
        {
            for (int i = 0; i < _answerTileControllers.Length; i++)
            {
                _answerTileControllers[i].RemoveCurrentTileController();
            }

            TileActions.AnswerTilesChanged();
        }

        #region Single Undo

        internal void UndoSingleAnswerTile()
        {
            var lastFilledAnswerTile = _answerTileControllers.LastOrDefault(x => x.GetState() == TileEmptyState.Filled);
            if (lastFilledAnswerTile == null) return;
            StopUndoMultipleAnswerTiles();
            TileActions.ClickTile(lastFilledAnswerTile);
        }

        #endregion

        #region Multiple Undo

        private Coroutine _undoMultipleAnswerTilesCoroutine;
        internal void UndoMultipleAnswerTiles()
        {
            StopUndoMultipleAnswerTiles();
            _undoMultipleAnswerTilesCoroutine = StartCoroutine(MultipleUndoSequence());
        }

        internal void StopUndoMultipleAnswerTiles()
        {
            if (_undoMultipleAnswerTilesCoroutine != null)
            {
                StopCoroutine(_undoMultipleAnswerTilesCoroutine);
                _undoMultipleAnswerTilesCoroutine = null;
            }
        }

        internal IEnumerator MultipleUndoSequence()
        {
            yield return new WaitForSeconds(0.5f);
            var lastFilledAnswerTile = _answerTileControllers.LastOrDefault(x => x.GetState() == TileEmptyState.Filled);
            if (lastFilledAnswerTile == null) yield break;

            while (lastFilledAnswerTile != null)
            {
                TileActions.ClickTile(lastFilledAnswerTile);
                yield return new WaitForSeconds(0.1f);
                lastFilledAnswerTile = _answerTileControllers.LastOrDefault(x => x.GetState() == TileEmptyState.Filled);
            }
        }

        #endregion

        internal (string, string[]) GetAnswer()
        {
            StringBuilder answerBuilder = new();
            string[] answerChars = new string[_answerTileControllers.Length];

            for (int i = 0; i < _answerTileControllers.Length; i++)
            {
                if (_answerTileControllers[i].GetState() == TileEmptyState.Filled)
                {
                    var character = _answerTileControllers[i].GetCharacter();
                    answerBuilder.Append(character);
                    answerChars.Append(character);
                }
            }

            return (answerBuilder.ToString(), answerChars);
        }

        internal AnswerTileController GetAnswerTileController(TileController tileController)
        {
            return _answerTileControllers.FirstOrDefault(x => x.GetState() == TileEmptyState.Filled && x.GetCurrentTileController() == tileController);
        }

        internal bool IsAnswerTilesFull()
        {
            return _answerTileControllers.All(x => x.GetState() == TileEmptyState.Filled);
        }

        internal bool IsAnswerTilesEmpty()
        {
            return _answerTileControllers.All(x => x.GetState() == TileEmptyState.Empty);
        }

        [ButtonMethod]
        private void GetChildAnswerTileControllers()
        {
            _answerTileControllers = GetComponentsInChildren<AnswerTileController>();
        }
    }
}
