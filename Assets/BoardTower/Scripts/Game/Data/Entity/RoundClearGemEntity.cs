using BoardTower.Base.Data.Entity;
using UnityEngine;

namespace BoardTower.Game.Data.Entity
{
    public sealed class RoundClearGemEntity : BaseEntity<int>
    {
        public override void Set(int t) => value = Mathf.Max(0, t);
    }
}