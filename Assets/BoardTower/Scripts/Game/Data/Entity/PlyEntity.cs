using BoardTower.Base.Data.Entity;
using UnityEngine;

namespace BoardTower.Game.Data.Entity
{
    public sealed class PlyEntity : BaseEntity<int>
    {
        public void Add(int x) => Set(value + x);
        public void Subtract(int x) => Set(Mathf.Max(0, value - x));
    }
}