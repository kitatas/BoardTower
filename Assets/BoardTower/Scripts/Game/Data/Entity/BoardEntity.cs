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
    }
}