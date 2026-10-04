using System.Threading;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.Ports;
using BoardTower.Tests.EditMode.TestHelpers;
using Cysharp.Threading.Tasks;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Domain.Ports
{
    [TestFixture]
    public sealed class EventLogPortsTests
    {
        private EventLogPorts _ports;
        private FakeAsyncPublisher<EventLogMessageVO> _publisher;
        private FakeAsyncSubscriber<EventLogMessageVO> _subscriber;

        [SetUp]
        public void SetUp()
        {
            _publisher = new FakeAsyncPublisher<EventLogMessageVO>();
            _subscriber = new FakeAsyncSubscriber<EventLogMessageVO>();
            _ports = new EventLogPorts(_subscriber, _publisher);
        }

        [Test]
        public void Constructor_WithValidDependencies_SubscriberIsAssigned()
        {
            Assert.That(_ports.eventLogMessageSubscriber, Is.EqualTo(_subscriber));
        }

        [Test]
        public async System.Threading.Tasks.Task PublishEventLogMessageAsync_CallsPublisherOnce()
        {
            var vo = new EventLogMessageVO("test", 1.0f);

            await _ports.PublishEventLogMessageAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public async System.Threading.Tasks.Task PublishEventLogMessageAsync_PublishesExactVO()
        {
            var vo = new EventLogMessageVO("hello", 2.5f);

            await _ports.PublishEventLogMessageAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished, Is.EqualTo(vo));
        }

        [Test]
        public async System.Threading.Tasks.Task PublishEventLogMessageAsync_CalledMultipleTimes_IncrementsCount()
        {
            var vo = new EventLogMessageVO("test", 1.0f);

            await _ports.PublishEventLogMessageAsync(vo, CancellationToken.None).AsTask();
            await _ports.PublishEventLogMessageAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(2));
        }
    }
}
