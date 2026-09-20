using BoardTower.Boot.Application;

namespace BoardTower.Boot.Data.DataStore
{
    public sealed class AppVersionDTO
    {
        public int major;
        public int minor;

        public AppVersionVO ToVO() => new(major, minor);
    }
}