using System.Threading;
using BoardTower.Game.Application;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Game.Domain.Ports
{
    public sealed class EventLogPorts
    {
        public readonly IAsyncSubscriber<EventLogMessageVO> eventLogMessageSubscriber;
        private readonly IAsyncPublisher<EventLogMessageVO> _eventLogMessagePublisher;

        public EventLogPorts(IAsyncSubscriber<EventLogMessageVO> eventLogMessageSubscriber,
            IAsyncPublisher<EventLogMessageVO> eventLogMessagePublisher)
        {
            this.eventLogMessageSubscriber = eventLogMessageSubscriber;
            _eventLogMessagePublisher = eventLogMessagePublisher;
        }

        public UniTask PublishEventLogMessageAsync(EventLogMessageVO eventLogMessage, CancellationToken token)
        {
            return _eventLogMessagePublisher.PublishAsync(eventLogMessage, token);
        }
    }
}