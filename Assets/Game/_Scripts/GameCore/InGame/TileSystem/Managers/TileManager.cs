using GameCore.InGame.TileSystem.Controllers;
using GameCore.TileSystem.Architecture;
using UnityEngine;
using GameCore.TileSystem.Controllers;
using System.Linq;
using GameCore.InGame.TileSystem.Managers.Answer;
using GameCore.AnimationSystem;

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
                await AnimationsManager.PlayPunchAnimation(AnswerTilesManager.Instance.transform);
                return;
            }

            TLC.SetTileState(TileState.Using);
            TLC.SetChildTiles();

            var ATC = AnswerTilesManager.Instance.SetAnswerTileController(TLC);
            if (ATC == null) return;

            TLC.transform.SetParent(ATC.transform);
            TLC.SetTileOnActionState(TileOnActionState.OnAction);
            ATC.SetTileOnActionState(TileOnActionState.OnAction);
            await AnimationsManager.PlayMoveAnimation(TLC.transform, Vector3.zero, Vector3.one, useLocalMove: true, useVFX: true);
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

            TileRaycastManager.LockTouch = true;
            foreach (var localTLC in allChildTiles)
            {
                if (localTLC.GetTileState() == TileState.Locked || localTLC.GetTileState() == TileState.NotUsing || localTLC.GetTileOnActionState() == TileOnActionState.OnAction) continue;

                var localATC = AnswerTilesManager.Instance.GetAnswerTileController(localTLC);
                if (localATC == null) continue;

                localTLC.transform.SetParent(localTLC.GetInit().GetParent());
                localTLC.SetTileState(TileState.NotUsing);
                localTLC.SetChildTiles();

                AnswerTilesManager.Instance.RemoveAnswerTileController(localATC);

                localTLC.SetTileOnActionState(TileOnActionState.OnAction);
                localTLC.SetTileOnActionState(TileOnActionState.OnAction);
                await AnimationsManager.PlayMoveAnimation(localTLC.transform, localTLC.GetInit().GetPosition(), localTLC.GetInit().GetScale(), useLocalMove: true, useVFX: false);
                localTLC.SetTileOnActionState(TileOnActionState.None);
                localATC.SetTileOnActionState(TileOnActionState.None);
            }
            TileRaycastManager.LockTouch = false;
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
