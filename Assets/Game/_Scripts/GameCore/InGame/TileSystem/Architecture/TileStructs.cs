using System.Collections.Generic;
using GameCore.InGame.TileSystem.Controllers;
using UnityEngine;

namespace GameCore.TileSystem.Architecture
{
    public struct TileElements
    {
        public List<TileController> ChildTiles;
        public List<TileController> ParentTiles;
        public string Character;
        public Transform InitialTransform;
        public TileState TileState;
        public TileType TileType;
        public TileEmptyState TileEmptyState;
        public TileOnActionState TileOnActionState;

        public TileElements(List<TileController> childTiles, List<TileController> parentTiles, string character, Transform initialTransform, TileState tileState, TileType tileType, TileEmptyState tileEmptyState, TileOnActionState tileOnActionState)
        {
            ChildTiles = childTiles;
            ParentTiles = parentTiles;
            Character = character;
            InitialTransform = initialTransform;
            TileState = tileState;
            TileType = tileType;
            TileEmptyState = tileEmptyState;
            TileOnActionState = tileOnActionState;
        }

        public readonly List<TileController> GetChildTiles() => ChildTiles;
        public readonly List<TileController> GetParentTiles() => ParentTiles;
        public readonly string GetCharacter() => Character;
        public readonly Transform GetInitialTransform() => InitialTransform;
        public readonly TileState GetTileState() => TileState;
        public readonly TileType GetTileType() => TileType;
        public readonly TileEmptyState GetTileEmptyState() => TileEmptyState;
        public readonly TileOnActionState GetTileOnActionState() => TileOnActionState;
    }

    public struct AnswerTileElements
    {
        public TileController CurrentTileController;
        public TileEmptyState TileEmptyState;
        public TileType TileType;
        public TileState TileState;
        public TileOnActionState TileOnActionState;

        public AnswerTileElements(TileController currentTileController, TileEmptyState tileEmptyState, TileType tileType, TileState tileState, TileOnActionState tileOnActionState)
        {
            CurrentTileController = currentTileController;
            TileEmptyState = tileEmptyState;
            TileType = tileType;
            TileState = tileState;
            TileOnActionState = tileOnActionState;
        }

        public readonly TileController GetCurrentTileController() => CurrentTileController;
        public readonly TileEmptyState GetTileEmptyState() => TileEmptyState;
        public readonly TileType GetTileType() => TileType;
        public readonly TileState GetTileState() => TileState;
        public readonly TileOnActionState GetTileOnActionState() => TileOnActionState;

        public const string CHARACTER_EMPTY_ERROR = "<color=red>[ANSWER TILE]</color> <color=yellow>{0}</color> is empty!";
        public const string CHARACTER_STATE_ERROR = "<color=red>[ANSWER TILE]</color> <color=yellow>{0}</color> has no state!";
    }
}
