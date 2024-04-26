using System;
using GameCore.InGame.TileSystem.Controllers;
using GameCore.InGame.TileSystem.Managers;
using GameCore.TileSystem.Architecture;
using UnityEngine;
using DG.Tweening;
using GameCore.TileSystem.Controllers;
using System.Threading.Tasks;

namespace GameCore.TileSystem.Managers
{
    public class TileManager : MonoBehaviour
    {
        private void Awake()
        {
            TileActions.OnClickTile += OnClickTile;
        }

        private void OnDestroy()
        {
            TileActions.OnClickTile -= OnClickTile;
        }

        private void OnClickTile(ITile ITile)
        {
            Debug.Log($"TileManager: {ITile.GetTileTransform().name} clicked!");

            switch (ITile.GetTileType())
            {
                case TileType.Tile:
                    GoingTLC(ITile as TileController);
                    break;
                case TileType.AnswerTile:
                    GoingATC(ITile as AnswerTileController);
                    break;
            }
        }

        private async void GoingTLC(TileController TLC)
        {
            if (TLC == null) return;
            var ATC = AnswerTilesManager.Instance.SetAnswerTileController(TLC);
            if (ATC == null) return;

            TLC.SetTileState(TileState.Using);
            TLC.SetTileOnActionState(TileOnActionState.OnAction);
            await PlayMoveAnimation(TLC.transform, ATC.transform);
            TLC.SetTileOnActionState(TileOnActionState.None);
        }

        private async void GoingATC(AnswerTileController ATC)
        {
            if (ATC == null) return;
            var TLC = ATC.GetCurrentTileController();
            if (TLC == null) return;

            ATC.RemoveCurrentTileController();
            ATC.SetTileEmptyState(TileEmptyState.Empty);

            TLC.SetTileState(TileState.NotUsing);
            TLC.SetTileOnActionState(TileOnActionState.OnAction);
            await PlayMoveAnimation(TLC.transform, TLC.GetInit());
            TLC.SetTileOnActionState(TileOnActionState.None);
        }

        private async Task PlayMoveAnimation(Transform obj, Transform target)
        {
            await obj.DOPunchScale(Vector3.one * 0.1f, 0.1f).OnComplete(() =>
            {
                obj.DOMove(target.position, 0.1f).OnComplete(() =>
                {
                    obj.DOScale(target.localScale, 0.1f).OnComplete(() =>
                    {
                        obj.DOPunchScale(Vector3.one * 0.1f, 0.1f);
                    });
                });
            }).AsyncWaitForCompletion();
        }
    }
}
