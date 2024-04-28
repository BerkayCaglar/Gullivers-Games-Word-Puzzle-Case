namespace GameCore.TileSystem.Architecture
{
    public enum TileState
    {
        Locked,
        NotUsing,
        Using,
        Used,
        None
    }

    public enum TileType
    {
        AnswerTile,
        Tile
    }

    public enum TileEmptyState
    {
        Empty,
        Filled
    }

    public enum TileOnActionState
    {
        None,
        OnAction
    }
}
