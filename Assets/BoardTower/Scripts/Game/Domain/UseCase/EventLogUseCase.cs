using System.Threading;
using BoardTower.Common.Domain.Repository;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.Ports;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using FastEnumUtility;
using MessagePipe;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class EventLogUseCase
    {
        private readonly EventLogEntity _eventLogEntity;
        private readonly EventLogPorts _eventLogPorts;
        private readonly LocaleRepository _localeRepository;

        public EventLogUseCase(EventLogEntity eventLogEntity, EventLogPorts eventLogPorts,
            LocaleRepository localeRepository)
        {
            _eventLogEntity = eventLogEntity;
            _eventLogPorts = eventLogPorts;
            _localeRepository = localeRepository;
        }

        public IAsyncSubscriber<EventLogMessageVO> eventLogMessage => _eventLogPorts.eventLogMessageSubscriber;

        public async UniTask PublishLogAsync(CancellationToken token)
        {
            foreach (var eventLog in _eventLogEntity.eventLogs)
            {
                var key = ZString.Format(EventLogConfig.LOCALE_KEY, eventLog.type.FastToString());
                var message = ZString.Format(_localeRepository.Get(key), eventLog.value);
                var logMessage = new EventLogMessageVO(message, EventLogConfig.DURATION);
                await _eventLogPorts.PublishEventLogMessageAsync(logMessage, token);
            }

            _eventLogEntity.Clear();
        }
    }
}