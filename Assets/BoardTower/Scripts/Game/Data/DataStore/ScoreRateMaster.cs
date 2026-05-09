using BoardTower.Game.Application;
using MasterMemory;
using MessagePack;

namespace BoardTower.Game.Data.DataStore
{
    [MemoryTable(nameof(ScoreRateMaster)), MessagePackObject(true)]
    public sealed class ScoreRateMaster
    {
        public ScoreRateMaster(int type, int threshold, float value)
        {
            Type = type;
            Threshold = threshold;
            Value = value;
        }

        [PrimaryKey(keyOrder: 0)] public int Type { get; }
        [PrimaryKey(keyOrder: 1)] public int Threshold { get; }
        public float Value { get; }

        public ScoreRateVO ToVO() => new(Type, Threshold, Value);
    }
}