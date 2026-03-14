using MasterMemory;
using MessagePack;

namespace BoardTower.Game.Data.DataStore
{
    [MemoryTable(nameof(RoundPlyMaster)), MessagePackObject(true)]
    public sealed class RoundPlyMaster
    {
        public RoundPlyMaster(int round, int plyCount)
        {
            Round = round;
            PlyCount = plyCount;
        }

        [PrimaryKey(keyOrder: 0)] public int Round { get; }
        public int PlyCount { get; }
    }
}