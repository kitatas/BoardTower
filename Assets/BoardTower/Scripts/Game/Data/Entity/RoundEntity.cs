using BoardTower.Common.Data.Entity;

namespace BoardTower.Game.Data.Entity
{
    public sealed class RoundEntity : BaseEntity<int>
    {
        public void Add(int x) => Set(value + x);
        public void Reset() => Set(0);
    }
}