using BoardTower.Base.Data.Entity;
using UnityEngine;

namespace BoardTower.Game.Data.Entity
{
    public sealed class PlyEntity : BaseEntity<int>
    {
        public int maxValue { get; private set; }

        public void SetUp(int x)
        {
            SetMax(x);
            Set(x);
        }

        public void SetMax(int x) => maxValue = Mathf.Max(0, x);

        public override void Set(int t) => value = Mathf.Clamp(t, 0, maxValue + 1);
        public void Add(int x) => Set(value + x);
        public void Subtract(int x) => Set(value - x);
    }
}