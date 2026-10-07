using System.Threading;
using System.Threading.Tasks;
using BoardTower.Boot.Application;
using BoardTower.Boot.Domain.Ports;
using BoardTower.Tests.EditMode.TestHelpers;
using Cysharp.Threading.Tasks;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Boot.Domain.Ports
{
    [TestFixture]
    public sealed class BootStatePortsTests
    {
        private BootStatePorts _ports;
        private FakeAsyncPublisher<BootState> _publisher;
        private FakeAsyncSubscriber<BootState> _subscriber;

        [SetUp]
        public void SetUp()
        {
            _publisher = new FakeAsyncPublisher<BootState>();
            _subscriber = new FakeAsyncSubscriber<BootState>();
            _ports = new BootStatePorts(_subscriber, _publisher);
        }

        [Test]
        public void Constructor_AssignsSubscriber()
        {
            Assert.That(_ports.subscriber, Is.SameAs(_subscriber));
        }

        [Test]
        public async Task PublishAsync_CallsPublisherOnce()
        {
            await _ports.PublishAsync(BootState.Init, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(1));
        }

        [TestCase(BootState.Init)]
        [TestCase(BootState.Load)]
        [TestCase(BootState.Splash)]
        [TestCase(BootState.Login)]
        public async Task PublishAsync_PublishesGivenState(BootState state)
        {
            await _ports.PublishAsync(state, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished, Is.EqualTo(state));
        }
    }
}
