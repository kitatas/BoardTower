using System.Collections.Generic;
using BoardTower.Game.Application;

namespace BoardTower.Game.Data.Entity
{
    public sealed class EventLogEntity
    {
        private readonly List<EventLogVO> _eventLogs;

        public EventLogEntity()
        {
            _eventLogs = new List<EventLogVO>();
        }

        public IEnumerable<EventLogVO> eventLogs => _eventLogs;

        public void Add(EventLogType type, int value)
        {
            _eventLogs.Add(new EventLogVO(type, value));
        }

        public void Clear()
        {
            _eventLogs.Clear();
        }
    }
}