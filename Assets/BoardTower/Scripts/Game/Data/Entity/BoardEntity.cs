using System.Collections.Generic;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using UniEx;

namespace BoardTower.Game.Data.Entity
{
    public sealed class BoardEntity
    {
        private readonly List<EventSquareVO> _events;

        public BoardEntity()
        {
            _events = new List<EventSquareVO>(BoardConfig.MAX_FILE * BoardConfig.MAX_RANK);
        }

        public void Clear()
        {
            _events.Clear();
        }

        public void Set(int index, SquareEventVO squareEvent)
        {
            if (_events.TryGetValue(index, out _))
            {
                _events[index] = new EventSquareVO(_events[index].square, squareEvent);
            }
            else
            {
                throw new QuitExceptionVO(ExceptionConfig.INVALID_SQUARE_INDEX);
            }
        }

        public void Add(EventSquareVO square)
        {
            _events.Add(square);
        }

        public EventSquareVO[] events => _events.ToArray();

        public (SquareEventVO squareEvent, int index) FindEvent(SquareVO square)
        {
            var index = _events.FindIndex(x => x.square.IsEqual(square));
            return index == -1
                ? (null, index)
                : (_events[index].squareEvent, index);
        }

        public bool IsMovable(SquareVO square)
        {
            var (squareEvent, _) = FindEvent(square);
            if (squareEvent == null) return true;
            if (squareEvent.type == SquareEventType.Block) return false;

            return true;
        }
    }
}