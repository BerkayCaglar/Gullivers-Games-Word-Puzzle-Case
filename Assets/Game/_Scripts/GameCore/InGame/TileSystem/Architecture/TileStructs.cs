using System.Collections.Generic;
using System.Linq;
using GameCore.InGame.TileSystem.Controllers;
using UnityEngine;

namespace GameCore.TileSystem.Architecture
{
    [System.Serializable]
    public struct TileElements
    {
        public List<TileController> ChildTiles;
        public List<TileController> ParentTiles;
        public string Character;
        public int Layer;
        public _InitialValues InitialValues;
        public TileState TileState;
        public TileType TileType;
        public TileEmptyState TileEmptyState;
        public TileOnActionState TileOnActionState;

        public TileElements(List<TileController> childTiles, List<TileController> parentTiles, string character, int layer, _InitialValues initialValues, TileState tileState, TileType tileType, TileEmptyState tileEmptyState, TileOnActionState tileOnActionState)
        {
            ChildTiles = childTiles;
            ParentTiles = parentTiles;
            Character = character;
            Layer = layer;
            InitialValues = initialValues;
            TileState = tileState;
            TileType = tileType;
            TileEmptyState = tileEmptyState;
            TileOnActionState = tileOnActionState;
        }

        public readonly List<TileController> GetChildTiles() => ChildTiles;
        public readonly List<TileController> GetParentTiles() => ParentTiles;
        public readonly string GetCharacter() => Character;
        public readonly int GetLayer() => Layer;
        public readonly _InitialValues GetInitialValues() => InitialValues;
        public readonly TileState GetTileState() => TileState;
        public readonly TileType GetTileType() => TileType;
        public readonly TileEmptyState GetTileEmptyState() => TileEmptyState;
        public readonly TileOnActionState GetTileOnActionState() => TileOnActionState;

        public readonly List<TileController> GetAllChildTiles()
        {
            List<TileController> allChildTiles = new();
            //Debug.Log($"Character: {Character} ChildTiles: {ChildTiles.Count}");
            allChildTiles.AddRange(ChildTiles);

            for (int i = 0; i < allChildTiles.Count; i++)
            {
                //Debug.Log($"Character: {allChildTiles[i].GetTileElements().GetCharacter()} ChildTiles: {allChildTiles[i].GetTileElements().GetChildTiles().Count}");
                allChildTiles.AddRange(allChildTiles[i].GetTileElements().GetChildTiles());
            }

            return allChildTiles.Distinct().ToList();
        }

        public struct _InitialValues
        {
            public Transform Parent;
            public Vector3 Position;
            public Vector3 Scale;

            public _InitialValues(Transform parent, Vector3 position, Vector3 scale)
            {
                Parent = parent;
                Position = position;
                Scale = scale;
            }

            public readonly Transform GetParent() => Parent;
            public readonly Vector3 GetPosition() => Position;
            public readonly Vector3 GetScale() => Scale;
        }
    }

    [System.Serializable]
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
