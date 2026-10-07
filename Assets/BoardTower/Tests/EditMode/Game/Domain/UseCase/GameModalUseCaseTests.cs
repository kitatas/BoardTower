using System.Threading;
using System.Threading.Tasks;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.Ports;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Tests.EditMode.TestHelpers;
using Cysharp.Threading.Tasks;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Domain.UseCase
{
    [TestFixture]
    public sealed class GameModalUseCaseTests
    {
        private GameModalUseCase _useCase;
        private GameModalEntity _entity;
        private FakeAsyncPublisher<BaseModalTransitionVO<GameModalType>> _publisher;
        private FakeAsyncSubscriber<BaseModalTransitionVO<GameModalType>> _subscriber;
        private GameModalPorts _ports;

        [SetUp]
        public void SetUp()
        {
            _entity = new GameModalEntity();
            _publisher = new FakeAsyncPublisher<BaseModalTransitionVO<GameModalType>>();
            _subscriber = new FakeAsyncSubscriber<BaseModalTransitionVO<GameModalType>>();
            _ports = new GameModalPorts(_subscriber, _publisher);
            _useCase = new GameModalUseCase(_entity, _ports);
        }

        [TearDown]
        public void TearDown()
        {
            _useCase.Dispose();
        }

        [Test]
        public void Subscriber_Property_ReturnsPortsSubscriber()
        {
            Assert.That(_useCase.subscriber, Is.SameAs(_subscriber));
        }

        [Test]
        public async Task FadeAsync_PublishesOnce()
        {
            var modal = new GameModalVO(GameModalType.Menu, Fade.In);

            await _useCase.FadeAsync(modal, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(1));
        }

        [TestCase(GameModalType.Menu)]
        [TestCase(GameModalType.Policy)]
        [TestCase(GameModalType.HowTo)]
        public async Task FadeAsync_PublishesModalType(GameModalType type)
        {
            var modal = new GameModalVO(type, Fade.In);

            await _useCase.FadeAsync(modal, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.type, Is.EqualTo(type));
        }

        [TestCase(Fade.In)]
        [TestCase(Fade.Out)]
        [TestCase(Fade.InOut)]
        public async Task FadeAsync_PublishesFadeOfModal(Fade fade)
        {
            var modal = new GameModalVO(GameModalType.Menu, fade);

            await _useCase.FadeAsync(modal, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.transition.fade, Is.EqualTo(fade));
        }

        [Test]
        public async Task FadeAsync_PublishesFadeDurationFromConfig()
        {
            var modal = new GameModalVO(GameModalType.Menu, Fade.Out);

            await _useCase.FadeAsync(modal, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.transition.duration, Is.EqualTo(GameModalConfig.FADE_DURATION));
        }

        [Test]
        public async Task FadeAsync_StoresPublishedTransitionInEntity()
        {
            var modal = new GameModalVO(GameModalType.Sound, Fade.In);

            await _useCase.FadeAsync(modal, CancellationToken.None).AsTask();

            Assert.That(_entity.value, Is.SameAs(_publisher.LastPublished));
        }
    }
}
