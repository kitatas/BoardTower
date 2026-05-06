using System.Collections.Generic;
using System.Linq;
using BoardTower.Common.Data.Entity;
using BoardTower.Game.Application;

namespace BoardTower.Game.Data.Entity
{
    public sealed class PickRelicEntity : BaseEntity<PickRelicVO>
    {
        private IEnumerable<RelicVO> relics
        {
            get
            {
                if (value != null && value.relics.Any())
                {
                    return value.relics;
                }
                else
                {
                    return Enumerable.Empty<RelicVO>();
                }
            }
        }

        public void Add(RelicVO relic)
        {
            var vos = relics.Append(relic);
            Set(new PickRelicVO(vos));
        }
    }
}