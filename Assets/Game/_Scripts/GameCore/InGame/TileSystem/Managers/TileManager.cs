using System;
using GameCore.InGame.TileSystem.Controllers;
using GameCore.InGame.TileSystem.Managers;
using GameCore.TileSystem.Architecture;
using UnityEngine;
using DG.Tweening;
using GameCore.TileSystem.Controllers;
using System.Threading.Tasks;
using MyBox;
using GameCore.PopupSystem.VFXPoolSystem;
using GameCore.CameraSystem;
using System.Linq;

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
            if (ITile.GetTileOnActionState() == TileOnActionState.OnAction) return;
            SendLog(ITile);
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

            if (AnswerTilesManager.Instance.IsAnswerTilesFull())
            {
                await PlayFullAnimation(AnswerTilesManager.Instance.transform);
                return;
            }

            var ATC = AnswerTilesManager.Instance.SetAnswerTileController(TLC);
            if (ATC == null) return;

            TLC.transform.SetParent(ATC.transform);
            TLC.SetTileState(TileState.Using);
            TLC.SetChildTiles();
            TLC.SetTileOnActionState(TileOnActionState.OnAction);
            ATC.SetTileOnActionState(TileOnActionState.OnAction);
            await PlayMoveAnimation(TLC.transform, Vector3.zero, Vector3.one, useLocalMove: true, useVFX: true);
            TLC.SetTileOnActionState(TileOnActionState.None);
            ATC.SetTileOnActionState(TileOnActionState.None);
        }

        private async void GoingATC(AnswerTileController ATC)
        {
            if (ATC == null) return;
            var TLC = ATC.GetCurrentTileController();
            if (TLC == null) return;

            var allChildTiles = ATC.GetCurrentTileController().GetTileElements().GetAllChildTiles();
            allChildTiles.Add(TLC);

            allChildTiles = allChildTiles.OrderBy(x => x.GetTileElements().GetLayer()).ToList();
            allChildTiles.Reverse();

            foreach (var localTLC in allChildTiles)
            {
                if (localTLC.GetTileState() == TileState.Locked || localTLC.GetTileState() == TileState.NotUsing || localTLC.GetTileOnActionState() == TileOnActionState.OnAction) continue;

                var localATC = AnswerTilesManager.Instance.GetAnswerTileController(localTLC);
                if (localATC == null) continue;

                localATC.RemoveCurrentTileController();
                localTLC.SetTileEmptyState(TileEmptyState.Empty);

                localTLC.transform.SetParent(localTLC.GetInit().GetParent());
                localTLC.SetTileState(TileState.NotUsing);
                localTLC.SetChildTiles();
                localTLC.SetTileOnActionState(TileOnActionState.OnAction);
                localTLC.SetTileOnActionState(TileOnActionState.OnAction);
                await PlayMoveAnimation(localTLC.transform, localTLC.GetInit().GetPosition(), localTLC.GetInit().GetScale(), useLocalMove: true, useVFX: false);
                localTLC.SetTileOnActionState(TileOnActionState.None);
                localATC.SetTileOnActionState(TileOnActionState.None);
            }
        }

        private async Task PlayMoveAnimation(Transform obj, Vector3 targetPos, Vector3 targetScale, bool useLocalMove, bool useVFX)
        {
            await obj.DOPunchScale(Vector3.one * 0.1f, 0.1f).AsyncWaitForCompletion();

            switch (useLocalMove)
            {
                case true:
                    await obj.DOLocalJump(targetPos, UnityEngine.Random.Range(0.1f, 0.2f), UnityEngine.Random.Range(1, 2), 0.1f).SetEase(Ease.InOutQuad).AsyncWaitForCompletion();
                    break;
                case false:
                    await obj.DOJump(targetPos, UnityEngine.Random.Range(0.1f, 0.2f), UnityEngine.Random.Range(1, 2), 0.1f).SetEase(Ease.InOutQuad).AsyncWaitForCompletion();
                    break;
            }

            if (useVFX)
            {
                VFXPooler.Instance.SpawnFromPool(VFXType.PuffVFX, obj.position, 0.5f);
                float[] randoms = Enumerable.Range(0, 3)
                    .Select(_ => UnityEngine.Random.Range(0.1f, 0.2f))
                    .ToArray();
                CameraShaker.ShakeCamera(new Vector3(randoms[0], randoms[1], randoms[2]));
            }

            await obj.DOScale(targetScale, 0.1f).AsyncWaitForCompletion();
            await obj.DOPunchScale(Vector3.one * 0.1f, 0.1f).AsyncWaitForCompletion();

            obj.localScale = targetScale;
        }

        private Tween _fullAnimation = null;
        private async Task PlayFullAnimation(Transform obj)
        {
            if (_fullAnimation != null)
            {
                _fullAnimation.Kill();
                _fullAnimation = null;
                obj.localScale = Vector3.one;
            }

            _fullAnimation = obj.DOPunchScale(Vector3.one * 0.1f, 0.1f);
            _fullAnimation.OnComplete(() => _fullAnimation = null);
            await _fullAnimation.AsyncWaitForCompletion();
        }

        private void SendLog(ITile ITile)
        {
            Debug.Log($"<color=green>[TILE MANAGER]</color> Clicked Tile: <color=yellow>{ITile.GetTileTransform().name}</color>");

            /*
            // FOR DEBUGGING
            Debug.Log($"<color=yellow> --------------------- </color>");

            Debug.Log($"<color=green>[TILE MANAGER]</color> Tile State: <color=yellow>{ITile.GetTileState()}</color>");
            Debug.Log($"<color=green>[TILE MANAGER]</color> Tile Type: <color=yellow>{ITile.GetTileType()}</color>");
            Debug.Log($"<color=green>[TILE MANAGER]</color> Tile Empty State: <color=yellow>{ITile.GetTileEmptyState()}</color>");
            Debug.Log($"<color=green>[TILE MANAGER]</color> Tile On Action State: <color=yellow>{ITile.GetTileOnActionState()}</color>");
            */
        }
    }
}
