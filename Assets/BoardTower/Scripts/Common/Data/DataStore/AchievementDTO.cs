using BoardTower.Common.Application;

namespace BoardTower.Common.Data.DataStore
{
    public sealed class AchievementDTO
    {
        public AchievementType Type;
        public AchievementRankType Rank;
        public int Value;

        public AchievementVO ToVO() => new(Type, Rank, Value);
    }
}