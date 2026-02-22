using BoardTower.Game.Application;
using MasterMemory;
using MessagePack;

namespace BoardTower.Game.Data.DataStore
{
    [MemoryTable(nameof(ChessmenMovementRuleMaster)), MessagePackObject(true)]
    public sealed class ChessmenMovementRuleMaster
    {
        public ChessmenMovementRuleMaster(int type, int movement, (int dx, int dy)[] offsets)
        {
            Type = type;
            Movement = movement;
            Offsets = offsets;
        }

        [PrimaryKey(keyOrder: 0)] public int Type { get; }
        public int Movement { get; }
        public (int dx, int dy)[] Offsets { get; }

        public ChessmenMovementRuleVO ToVO() => new(Type, Movement, Offsets);
    }
}