using MasterMemory;
using MessagePack;

namespace BoardTower.Game.Data.DataStore
{
    [MemoryTable(nameof(RoundClearMaster)), MessagePackObject(true)]
    public sealed class RoundClearMaster
    {
        public RoundClearMaster(int round, int gemCount)
        {
            Round = round;
            GemCount = gemCount;
        }

        [PrimaryKey(keyOrder: 0)] public int Round { get; }
        public int GemCount { get; }
    }
}