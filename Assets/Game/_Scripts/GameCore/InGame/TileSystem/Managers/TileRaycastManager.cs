using GameCore.InGame.TileSystem.Controllers;
using GameCore.TileSystem.Architecture;
using GameCore.TileSystem.Controllers;
using UnityEngine;

namespace GameCore.TileSystem.Managers
{
    public class TileRaycastManager : MonoBehaviour
    {
        private Camera _camera;
        private bool _isTouching = false;

        public static bool LockTouch { get; set; } = false;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (LockTouch) return;
            if (!_isTouching && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                _isTouching = true;

                Ray ray = _camera.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(ray, Mathf.Infinity);

                foreach (var hit in hits)
                {
                    if (hit.collider != null && hit.collider.TryGetComponent(out ITile ITile))
                    {
                        if (IsValidTile(ITile))
                        {
                            OnClickTile(ITile);
                            break;
                        }
                    }
                }
            }
        }

        private bool IsValidTile(ITile tile)
        {
            return (tile.GetTileType() != TileType.Tile
                || tile.GetTileState() is not (TileState.Using or TileState.Locked or TileState.Used))
                && (tile.GetTileType() != TileType.AnswerTile || (tile.GetTileEmptyState() != TileEmptyState.Empty && tile.GetTileOnActionState() is not TileOnActionState.OnAction));
        }

        private void LateUpdate()
        {
            if (_isTouching && Input.touchCount == 0)
            {
                _isTouching = false;
            }
        }

        private void OnClickTile(ITile ITile)
        {
            TileActions.ClickTile(ITile);
        }
    }
}
