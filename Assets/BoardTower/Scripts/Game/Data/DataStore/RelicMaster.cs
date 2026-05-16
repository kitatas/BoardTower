using BoardTower.Game.Application;
using MasterMemory;
using MessagePack;

namespace BoardTower.Game.Data.DataStore
{
    [MemoryTable(nameof(RelicMaster)), MessagePackObject(true)]
    public sealed class RelicMaster
    {
        public RelicMaster(int type, string name, string content, bool isUniq)
        {
            Type = type;
            Name = name;
            Content = content;
            IsUniq = isUniq;
        }

        [PrimaryKey(keyOrder: 0)] public int Type { get; }
        public string Name { get; }
        public string Content { get; }
        [SecondaryKey(indexNo: 0), NonUnique] public bool IsUniq { get; }

        public RelicVO ToVO() => new(Type, Name, Content, IsUniq);
    }
}