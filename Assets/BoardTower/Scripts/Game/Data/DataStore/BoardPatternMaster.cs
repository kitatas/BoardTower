using BoardTower.Game.Application;
using MasterMemory;
using MessagePack;

namespace BoardTower.Game.Data.DataStore
{
    [MemoryTable(nameof(BoardPatternMaster)), MessagePackObject(true)]
    public sealed class BoardPatternMaster
    {
        public BoardPatternMaster(int id, int[] types)
        {
            Id = id;
            Types = types;
        }

        [PrimaryKey(keyOrder: 0)] public int Id { get; }
        public int[] Types { get; }

        public BoardPatternVO ToVO() => new(Types);
    }
}