using BoardTower.Game.Application;
using MasterMemory;
using MessagePack;

namespace BoardTower.Game.Data.DataStore
{
    [MemoryTable(nameof(AchievementMaster)), MessagePackObject(true)]
    public sealed class AchievementMaster
    {
        public AchievementMaster(int type, int rank, int value)
        {
            Type = type;
            Rank = rank;
            Value = value;
        }

        [PrimaryKey(keyOrder: 0)] public int Type { get; }
        [PrimaryKey(keyOrder: 1)] public int Rank { get; }
        public int Value { get; }

        public AchievementVO ToVO() => new(Type, Rank, Value);
    }
}