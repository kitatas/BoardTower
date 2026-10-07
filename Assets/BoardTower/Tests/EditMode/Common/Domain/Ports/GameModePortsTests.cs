using System.Threading;
using System.Threading.Tasks;
using BoardTower.Common.Application;
using BoardTower.Common.Domain.Ports;
using BoardTower.Tests.EditMode.TestHelpers;
using Cysharp.Threading.Tasks;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Common.Domain.Ports
{
    [TestFixture]
    public sealed class GameModePortsTests
    {
        private GameModePorts _ports;
        private FakeAsyncPublisher<GameModeTransitionVO> _publisher;
        private FakeAsyncSubscriber<GameModeTransitionVO> _subscriber;

        [SetUp]
        public void SetUp()
        {
            _publisher = new FakeAsyncPublisher<GameModeTransitionVO>();
            _subscriber = new FakeAsyncSubscriber<GameModeTransitionVO>();
            _ports = new GameModePorts(_subscriber, _publisher);
        }

        [Test]
        public void Constructor_AssignsGameModeTransitionSubscriber()
        {
            Assert.That(_ports.gameModeTransitionSubscriber, Is.EqualTo(_subscriber));
        }

        [Test]
        public async Task PublishGameModeAsync_CallsPublisherOnce()
        {
            var vo = GameModeTransitionVO.Create(GameMode.Online, Fade.Out, 0.0f);

            await _ports.PublishGameModeAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public async Task PublishGameModeAsync_PublishesExactVO()
        {
            var vo = GameModeTransitionVO.Create(GameMode.Offline, Fade.In, 0.25f);

            await _ports.PublishGameModeAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished, Is.SameAs(vo));
        }
    }
}
