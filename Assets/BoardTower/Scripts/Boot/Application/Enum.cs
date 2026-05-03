namespace BoardTower.Boot.Application
{
    public enum BootState
    {
        None = 0,
        Init = 1,
        Load = 2,
        Splash = 3,
        Login = 4,
    }

    public enum SplashType
    {
        None = 0,
        Developer = 1,
        PlayFab = 2,
    }
}