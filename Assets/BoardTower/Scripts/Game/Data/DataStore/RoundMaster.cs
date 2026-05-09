using BoardTower.Game.Application;
using MasterMemory;
using MessagePack;

namespace BoardTower.Game.Data.DataStore
{
    [MemoryTable(nameof(RoundMaster)), MessagePackObject(true)]
    public sealed class RoundMaster
    {
        public RoundMaster(int round, int plyCount, int gemCount)
        {
            Round = round;
            PlyCount = plyCount;
            GemCount = gemCount;
        }

        [PrimaryKey(keyOrder: 0)] public int Round { get; }
        public int PlyCount { get; }
        public int GemCount { get; }

        public RoundVO ToVO() => new(Round, PlyCount, GemCount);
    }
}