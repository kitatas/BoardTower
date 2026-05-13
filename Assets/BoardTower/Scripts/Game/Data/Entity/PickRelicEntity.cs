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

        public IEnumerable<RelicType> relicTypes => relics.Select(x => x.type);

        public bool canMoveToBlock => IsContain(RelicType.Horseshoe);
        public bool isIgnoreCollapse => IsContain(RelicType.Greaves);
        public bool isIgnoreBelt => IsContain(RelicType.Scales);

        public void Add(RelicVO relic)
        {
            var vos = relics.Append(relic);
            Set(new PickRelicVO(vos));
        }

        private bool IsContain(params RelicType[] types)
        {
            return relicTypes.Any(types.Contains);
        }
    }
}