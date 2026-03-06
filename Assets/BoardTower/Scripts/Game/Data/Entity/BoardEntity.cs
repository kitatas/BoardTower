using System.Collections.Generic;
using BoardTower.Game.Application;

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

        public void Add(EventSquareVO square)
        {
            _events.Add(square);
        }

        public EventSquareVO[] events => _events.ToArray();

        public SquareEventVO FindEvent(SquareVO square)
        {
            return _events.Find(x => x.square.IsEqual(square)).squareEvent;
        }

        public bool IsMovable(SquareVO square)
        {
            var squareEvent = FindEvent(square);
            if (squareEvent == null) return true;
            if (squareEvent.type == SquareEventType.Block) return false;

            return true;
        }
    }
}