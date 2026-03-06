namespace BoardTower.Game.Application
{
    public enum GameState
    {
        None = 0,
        Init = 1,
        SetUp = 2,
        Input = 3,
        Event = 4,
    }

    public enum ChessmenType
    {
        None = 0,
        King = 1,
        Queen = 2,
        Rook = 3,
        Bishop = 4,
        Knight = 5,
    }

    public enum ChessmenMovementType
    {
        None = 0,
        Leaper = 1,
        Slider = 2,
    }

    public enum HighlightSquareType
    {
        None = 0,
        Movable = 1,
        Default = 2,
    }

    public enum SquareEventType
    {
        None = 0,
        Empty = 1,
        BeltUp = 2,
        BeltDown = 3,
        BeltLeft = 4,
        BeltRight = 5,
        Block = 6,
    }
}