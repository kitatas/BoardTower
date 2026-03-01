using System.Collections.Generic;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Data.DataStore;

namespace BoardTower.Game.Domain.Repository
{
    public sealed class SquareEventRepository
    {
        private readonly Dictionary<SquareEventType, SquareEventVO> _squareEventMap;

        public SquareEventRepository(SquareEventTable squareEventTable)
        {
            _squareEventMap = new Dictionary<SquareEventType, SquareEventVO>();
            foreach (var s in squareEventTable.records) _squareEventMap.TryAdd(s.type, s.ToVO());
        }

        public SquareEventVO Find(SquareEventType type)
        {
            if (_squareEventMap.TryGetValue(type, out var vo))
            {
                return vo;
            }
            else
            {
                throw new QuitExceptionVO(ExceptionConfig.INVALID_SQUARE_EVENT);
            }
        }
    }
}