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
    }

    public enum SeType
    {
        None = 0,
        Decision = 1,
        Cancel = 2,
    }
}