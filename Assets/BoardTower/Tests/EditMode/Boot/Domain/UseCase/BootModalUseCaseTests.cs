using System.Threading;
using System.Threading.Tasks;
using BoardTower.Boot.Application;
using BoardTower.Boot.Data.Entity;
using BoardTower.Boot.Domain.Ports;
using BoardTower.Boot.Domain.UseCase;
using BoardTower.Common.Application;
using BoardTower.Tests.EditMode.TestHelpers;
using Cysharp.Threading.Tasks;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Boot.Domain.UseCase
{
    [TestFixture]
    public sealed class BootModalUseCaseTests
    {
        private BootModalUseCase _useCase;
        private BootModalEntity _entity;
        private FakeAsyncPublisher<BaseModalTransitionVO<BootModalType>> _publisher;
        private FakeAsyncSubscriber<BaseModalTransitionVO<BootModalType>> _subscriber;

        [SetUp]
        public void SetUp()
        {
            _entity = new BootModalEntity();
            _publisher = new FakeAsyncPublisher<BaseModalTransitionVO<BootModalType>>();
            _subscriber = new FakeAsyncSubscriber<BaseModalTransitionVO<BootModalType>>();
            var ports = new BootModalPorts(_subscriber, _publisher);
            _useCase = new BootModalUseCase(_entity, ports);
        }

        [TearDown]
        public void TearDown()
        {
            _useCase.Dispose();
        }

        [Test]
        public void Subscriber_ReturnsPortsSubscriber()
        {
            Assert.That(_useCase.subscriber, Is.SameAs(_subscriber));
        }

        [Test]
        public async Task FadeAsync_PublishesOnce()
        {
            var modal = new BootModalVO(BootModalType.Update, Fade.In);

            await _useCase.FadeAsync(modal, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(1));
        }

        [TestCase(BootModalType.Update)]
        [TestCase(BootModalType.Name)]
        public async Task FadeAsync_PublishesTransitionWithModalType(BootModalType type)
        {
            var modal = new BootModalVO(type, Fade.In);

            await _useCase.FadeAsync(modal, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.type, Is.EqualTo(type));
        }

        [TestCase(Fade.In)]
        [TestCase(Fade.Out)]
        [TestCase(Fade.InOut)]
        public async Task FadeAsync_PublishesTransitionWithModalFade(Fade fade)
        {
            var modal = new BootModalVO(BootModalType.Update, fade);

            await _useCase.FadeAsync(modal, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.transition.fade, Is.EqualTo(fade));
        }

        [Test]
        public async Task FadeAsync_PublishesTransitionWithConfiguredDuration()
        {
            var modal = new BootModalVO(BootModalType.Update, Fade.In);

            await _useCase.FadeAsync(modal, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.transition.duration, Is.EqualTo(BootModalConfig.FADE_DURATION));
        }

        [Test]
        public async Task FadeAsync_StoresPublishedTransitionInEntity()
        {
            var modal = new BootModalVO(BootModalType.Name, Fade.Out);

            await _useCase.FadeAsync(modal, CancellationToken.None).AsTask();

            Assert.That(_entity.value, Is.SameAs(_publisher.LastPublished));
        }
    }
}
