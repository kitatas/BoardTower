using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.Ports;
using BoardTower.Tests.EditMode.TestHelpers;
using Cysharp.Threading.Tasks;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Domain.Ports
{
    [TestFixture]
    public sealed class GameModalPortsTests
    {
        private GameModalPorts _ports;
        private FakeAsyncPublisher<BaseModalTransitionVO<GameModalType>> _publisher;
        private FakeAsyncSubscriber<BaseModalTransitionVO<GameModalType>> _subscriber;

        [SetUp]
        public void SetUp()
        {
            _publisher = new FakeAsyncPublisher<BaseModalTransitionVO<GameModalType>>();
            _subscriber = new FakeAsyncSubscriber<BaseModalTransitionVO<GameModalType>>();
            _ports = new GameModalPorts(_subscriber, _publisher);
        }

        [Test]
        public void Constructor_WithValidDependencies_SubscriberIsAssigned()
        {
            Assert.That(_ports.subscriber, Is.SameAs(_subscriber));
        }

        [Test]
        public async System.Threading.Tasks.Task PublishAsync_CallsPublisherOnce()
        {
            var transition = GameModalTransitionVO.Create(GameModalType.Menu, Fade.In, 0.25f);

            await _ports.PublishAsync(transition, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public async System.Threading.Tasks.Task PublishAsync_PublishesExactTransition()
        {
            var transition = GameModalTransitionVO.Create(GameModalType.Sound, Fade.Out, 0.5f);

            await _ports.PublishAsync(transition, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished, Is.SameAs(transition));
        }
    }
}
