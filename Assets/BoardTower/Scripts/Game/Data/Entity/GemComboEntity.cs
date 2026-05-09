using BoardTower.Common.Data.Entity;

namespace BoardTower.Game.Data.Entity
{
    public sealed class GemComboEntity : BaseEntity<int>
    {
        public void Reset() => Set(0);
        public void Add(int t) => Set(value + t);
    }
}