using BoardTower.Common.Data.Entity;
using UnityEngine;

namespace BoardTower.Game.Data.Entity
{
    public sealed class GemEntity : BaseEntity<int>
    {
        public override void Set(int t) => value = Mathf.Max(0, t);
        public void Reset() => Set(0);
        public void Add(int x) => Set(value + x);
        public void Subtract(int x) => Set(value - x);
    }
}