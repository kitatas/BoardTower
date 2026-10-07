using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using BoardTower.Common.Application;
using BoardTower.Common.Domain.Repository;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.Ports;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Tests.EditMode.TestHelpers;
using Cysharp.Threading.Tasks;
using MessagePipe;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Domain.UseCase
{
    [TestFixture]
    public sealed class EventLogUseCaseTests
    {
        private EventLogUseCase _useCase;
        private EventLogEntity _eventLogEntity;
        private LocaleRepository _localeRepository;
        private RecordingPublisher _publisher;
        private FakeAsyncSubscriber<EventLogMessageVO> _subscriber;
        private EventLogPorts _ports;

        [SetUp]
        public void SetUp()
        {
            _eventLogEntity = new EventLogEntity();
            _localeRepository = new LocaleRepository();
            _publisher = new RecordingPublisher();
            _subscriber = new FakeAsyncSubscriber<EventLogMessageVO>();
            _ports = new EventLogPorts(_subscriber, _publisher);
            _useCase = new EventLogUseCase(_eventLogEntity, _ports, _localeRepository);

            // LocaleRepository.LoadAsync は LocalizationSettings に依存するため、辞書へ直接差し込む
            SetLocale(_localeRepository, "EventLog_Gem", "Gem +{0}");
            SetLocale(_localeRepository, "EventLog_Score", "Score +{0}");
            SetLocale(_localeRepository, "EventLog_Heart", "Heart +{0}");
        }

        [Test]
        public void EventLogMessage_Property_ReturnsPortsSubscriber()
        {
            Assert.That(_useCase.eventLogMessage, Is.SameAs(_subscriber));
        }

        [Test]
        public async Task PublishLogAsync_WithNoLogs_PublishesNothing()
        {
            await _useCase.PublishLogAsync(CancellationToken.None).AsTask();

            Assert.That(_publisher.published, Is.Empty);
        }

        [Test]
        public async Task PublishLogAsync_WithOneLog_PublishesLocalizedMessageWithValue()
        {
            _eventLogEntity.Add(EventLogType.Gem, 3);

            await _useCase.PublishLogAsync(CancellationToken.None).AsTask();

            Assert.That(_publisher.published[0].message, Is.EqualTo("Gem +3"));
        }

        [Test]
        public async Task PublishLogAsync_WithOneLog_PublishesConfiguredDuration()
        {
            _eventLogEntity.Add(EventLogType.Gem, 3);

            await _useCase.PublishLogAsync(CancellationToken.None).AsTask();

            Assert.That(_publisher.published[0].duration, Is.EqualTo(EventLogConfig.DURATION));
        }

        [Test]
        public async Task PublishLogAsync_WithMultipleLogs_PublishesOncePerLog()
        {
            _eventLogEntity.Add(EventLogType.Gem, 1);
            _eventLogEntity.Add(EventLogType.Score, 100);
            _eventLogEntity.Add(EventLogType.Heart, 2);

            await _useCase.PublishLogAsync(CancellationToken.None).AsTask();

            Assert.That(_publisher.published.Count, Is.EqualTo(3));
        }

        [Test]
        public async Task PublishLogAsync_WithMultipleLogs_PublishesInAddedOrder()
        {
            _eventLogEntity.Add(EventLogType.Gem, 1);
            _eventLogEntity.Add(EventLogType.Score, 100);
            _eventLogEntity.Add(EventLogType.Heart, 2);

            await _useCase.PublishLogAsync(CancellationToken.None).AsTask();

            var messages = _publisher.published.ConvertAll(x => x.message);
            Assert.That(messages, Is.EqualTo(new[] { "Gem +1", "Score +100", "Heart +2" }));
        }

        [Test]
        public async Task PublishLogAsync_AfterPublish_ClearsEventLogs()
        {
            _eventLogEntity.Add(EventLogType.Gem, 1);
            _eventLogEntity.Add(EventLogType.Score, 100);

            await _useCase.PublishLogAsync(CancellationToken.None).AsTask();

            Assert.That(_eventLogEntity.eventLogs, Is.Empty);
        }

        [Test]
        public async Task PublishLogAsync_CalledTwice_DoesNotRepublishPreviousLogs()
        {
            _eventLogEntity.Add(EventLogType.Gem, 1);
            await _useCase.PublishLogAsync(CancellationToken.None).AsTask();

            await _useCase.PublishLogAsync(CancellationToken.None).AsTask();

            Assert.That(_publisher.published.Count, Is.EqualTo(1));
        }

        [Test]
        public void PublishLogAsync_WithMissingLocaleKey_ThrowsQuitExceptionVO()
        {
            // Ride のロケールは登録していない
            _eventLogEntity.Add(EventLogType.Ride, 1);

            Assert.ThrowsAsync<QuitExceptionVO>(
                async () => await _useCase.PublishLogAsync(CancellationToken.None).AsTask());
        }

        private static void SetLocale(LocaleRepository repository, string key, string value)
        {
            var field = typeof(LocaleRepository)
                .GetField("_localeMap", BindingFlags.NonPublic | BindingFlags.Instance);
            var map = (Dictionary<string, string>)field.GetValue(repository);
            map[key] = value;
        }

        /// <summary>
        /// 発行されたメッセージを順番どおりに記録するだけの Publisher
        /// </summary>
        private sealed class RecordingPublisher : IAsyncPublisher<EventLogMessageVO>
        {
            public readonly List<EventLogMessageVO> published = new List<EventLogMessageVO>();

            public void Publish(EventLogMessageVO message, CancellationToken cancellationToken = default)
            {
            }

            public UniTask PublishAsync(EventLogMessageVO message, CancellationToken cancellationToken = default)
            {
                published.Add(message);
                return UniTask.CompletedTask;
            }

            public UniTask PublishAsync(EventLogMessageVO message, AsyncPublishStrategy publishStrategy,
                CancellationToken cancellationToken = default)
            {
                published.Add(message);
                return UniTask.CompletedTask;
            }
        }
    }
}
