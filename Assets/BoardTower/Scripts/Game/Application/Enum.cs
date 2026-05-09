namespace BoardTower.Game.Application
{
    public enum GameState
    {
        None = 0,
        Init = 1,
        SetUp = 2,
        Input = 3,
        Event = 4,
        Judge = 5,
        Clear = 6,
        Fail = 7,
        Finish = 8,
        Pick = 9,
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
        BeltRight = 2,
        BeltDown = 3,
        BeltLeft = 4,
        BeltUp = 5,
        Block = 6,
        Gem = 7,
        Ply = 8,
        Collapse = 9,
    }

    public enum RenderType
    {
        None = 0,
        Refresh = 1,
        Retain = 2,
    }

    public enum RotateType
    {
        None = 0,
        Angle0 = 1,
        Angle90 = 2,
        Angle180 = 3,
        Angle270 = 4,
    }

    public enum FinishType
    {
        None = 0,
        Clear = 1,
        Fail = 2,
    }

    public enum GameModalType
    {
        None = 0,
        Menu = 1,
        Policy = 2,
        License = 3,
        Credit = 4,
        Sound = 5,
        DeleteConfirm = 6,
        DeleteComplete = 7,
        GiveUpConfirm = 8,
        GiveUpComplete = 9,
    }

    public enum RelicType
    {
        None = 0,
        Scales = 1,
        Greaves = 2,
        Grace = 3,
        Lantern = 4,
        Compass = 5,
        Horseshoe = 6,
        Cross = 7,
        Scripture = 8,
        Bell = 9,
    }

    public enum ScoreRateType
    {
        None = 0,
    }
}