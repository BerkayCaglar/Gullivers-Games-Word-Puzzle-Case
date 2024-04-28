using GameCore.GameFlowSystem;
using GameCore.TileSystem.Architecture;
using UnityEngine;

namespace GameCore.TileSystem.Managers
{
    public class TileRaycastManager : MonoBehaviour
    {
        private Camera _camera;
        private bool _isTouching = false;

        private static bool _lockTouch = false;

        public static bool LockTouch
        {
            get => _lockTouch;
            set
            {
                Debug.Log($"LockTouch Setted to {value}");
                _lockTouch = value;
            }
        }

        private void Start()
        {
            _camera = Camera.main;
            GameActions.OnGameOver += () => LockTouch = true;
            LockTouch = false;
        }

        private void Update()
        {
            if (GameManager.GetGameState() == GameState.GameOver) return;
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
