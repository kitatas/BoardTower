using System.Threading;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.Ports;
using BoardTower.Tests.EditMode.TestHelpers;
using Cysharp.Threading.Tasks;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Domain.Ports
{
    [TestFixture]
    public sealed class GameStatePortsTests
    {
        private GameStatePorts _ports;
        private FakeAsyncPublisher<GameState> _publisher;
        private FakeAsyncSubscriber<GameState> _subscriber;

        [SetUp]
        public void SetUp()
        {
            _publisher = new FakeAsyncPublisher<GameState>();
            _subscriber = new FakeAsyncSubscriber<GameState>();
            _ports = new GameStatePorts(_subscriber, _publisher);
        }

        [Test]
        public void Constructor_WithValidDependencies_SubscriberIsAssigned()
        {
            Assert.That(_ports.subscriber, Is.SameAs(_subscriber));
        }

        [Test]
        public async System.Threading.Tasks.Task PublishAsync_CallsPublisherOnce()
        {
            await _ports.PublishAsync(GameState.Init, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(1));
        }

        [TestCase(GameState.Init)]
        [TestCase(GameState.Input)]
        [TestCase(GameState.Finish)]
        public async System.Threading.Tasks.Task PublishAsync_PublishesGivenState(GameState state)
        {
            await _ports.PublishAsync(state, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished, Is.EqualTo(state));
        }
    }
}
