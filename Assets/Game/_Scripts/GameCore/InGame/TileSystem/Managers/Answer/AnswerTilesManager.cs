using System.Collections;
using System.Linq;
using System.Text;
using GameCore.AnimationSystem;
using GameCore.GameFlowSystem;
using GameCore.InGame.TileSystem.Controllers;
using GameCore.SingletonSystem;
using GameCore.TileSystem.Architecture;
using GameCore.TileSystem.Controllers;
using GameCore.TileSystem.Managers;
using MyBox;
using UnityEngine;

namespace GameCore.InGame.TileSystem.Managers.Answer
{
    public class AnswerTilesManager : AutoSingleton<AnswerTilesManager>
    {
        [SerializeField] private AnswerTileController[] _answerTileControllers;

        #region Events
        private void Start()
        {
            GameActions.OnAnswerCheck += OnAnswerCheck;
            GameActions.OnScoreChanged += OnScoreChanged;
            GameActions.OnGameOver += OnGameOver;
        }

        private void OnDestroy()
        {
            GameActions.OnAnswerCheck -= OnAnswerCheck;
            GameActions.OnScoreChanged -= OnScoreChanged;
            GameActions.OnGameOver -= OnGameOver;
        }

        private async void OnGameOver()
        {
            foreach (var answerTileController in _answerTileControllers.Reverse())
            {
                answerTileController.StopShake();
                await AnimationsManager.PlayExplosionAnimation(answerTileController.transform);
            }
        }

        private void OnAnswerCheck(bool isCorrect)
        {
            ShakeAnswerTiles(isCorrect);
        }

        private void ShakeAnswerTiles(bool isCorrect)
        {
            switch (isCorrect)
            {
                case true:
                    ShakeAnswerTiles(true);
                    break;
                case false:
                    ShakeAnswerTiles(false);
                    break;
            }

            void ShakeAnswerTiles(bool shake)
            {
                _answerTileControllers.ForEach(x =>
                {
                    if (shake)
                        x.Shake();
                    else
                        x.StopShake();
                });
            }
        }

        private async void OnScoreChanged(int score)
        {
            if (score <= 0) return;

            TileRaycastManager.LockTouch = true;
            foreach (var answerTileController in _answerTileControllers.Reverse())
            {
                answerTileController.StopShake();
                var TLC = answerTileController.GetCurrentTileController();
                if (TLC == null) continue;
                TLC.SetTileState(TileState.Used);
                await AnimationsManager.PlayExplosionAnimation(TLC.transform);
            }

            ResetAnswerTiles();
            TileRaycastManager.LockTouch = false;
        }

        #endregion

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

        internal string GetAnswer()
        {
            StringBuilder answerBuilder = new();

            for (int i = 0; i < _answerTileControllers.Length; i++)
            {
                if (_answerTileControllers[i].GetState() == TileEmptyState.Filled)
                {
                    var character = _answerTileControllers[i].GetCharacter();
                    answerBuilder.Append(character);
                }
            }

            return answerBuilder.ToString();
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
