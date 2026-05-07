using System.Collections.Generic;
using System.Linq;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Data.DataStore;

namespace BoardTower.Game.Domain.Repository
{
    public sealed class RelicRepository
    {
        private readonly Dictionary<RelicType, RelicVO> _relicMap;

        public RelicRepository(RelicTable relicTable)
        {
            _relicMap = relicTable.all.ToDictionary(x => x.type, x => x.ToVO());
        }

        public RelicVO Find(RelicType type)
        {
            if (_relicMap.TryGetValue(type, out var vo))
            {
                return vo;
            }
            else
            {
                throw new QuitExceptionVO(ExceptionConfig.INVALID_RELIC);
            }
        }

        public IEnumerable<RelicVO> FindsByTypeNotIn(IEnumerable<RelicType> types)
        {
            return _relicMap.Values
                .Where(x => !types.Contains(x.type));
        }
    }
}