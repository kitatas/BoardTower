namespace BoardTower.Common.Application
{
    public enum SceneName
    {
        None,
        Boot,
        Game,
    }

    public enum LoadType
    {
        None,
        Direct,
        Fade,
    }

    public enum Fade
    {
        None,
        In,
        Out,
        InOut,
    }

    public enum BgmType
    {
        None = 0,
        Top = 1,
        Game = 2,
    }

    public enum SeType
    {
        None = 0,
        Decision = 1,
        Cancel = 2,
    }

    public enum AchievementType
    {
        None = 0,
        Play = 1,
        Score = 2,
        Clear = 3,
    }

    public enum AchievementRankType
    {
        None = 0,
        Normal = 1,
        Bronze = 2,
        Silver = 3,
        Gold = 4,
        Platinum = 5,
    }
}