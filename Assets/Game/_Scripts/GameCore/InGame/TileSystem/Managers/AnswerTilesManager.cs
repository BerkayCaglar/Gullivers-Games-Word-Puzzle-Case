using System;
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

        internal void UndoSingleAnswerTile()
        {
            // Find the last filled answer tile
            var lastFilledAnswerTile = _answerTileControllers.LastOrDefault(x => x.GetState() == TileEmptyState.Filled);

            // If there is no filled answer tile, return
            if (lastFilledAnswerTile == null) return;

            TileActions.ClickTile(lastFilledAnswerTile);
        }

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
