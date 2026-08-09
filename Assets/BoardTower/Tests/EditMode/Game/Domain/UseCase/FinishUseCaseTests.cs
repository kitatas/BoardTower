using System.Threading;
using System.Threading.Tasks;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.Ports;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Tests.EditMode.TestHelpers;
using Cysharp.Threading.Tasks;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Domain.UseCase
{
    [TestFixture]
    public sealed class FinishUseCaseTests
    {
        private FinishUseCase _useCase;
        private FakeAsyncPublisher<FinishTransitionVO> _publisher;
        private FakeAsyncSubscriber<FinishTransitionVO> _subscriber;
        private FinishPorts _ports;

        [SetUp]
        public void SetUp()
        {
            _publisher = new FakeAsyncPublisher<FinishTransitionVO>();
            _subscriber = new FakeAsyncSubscriber<FinishTransitionVO>();
            _ports = new FinishPorts(_subscriber, _publisher);
            _useCase = new FinishUseCase(_ports);
        }

        [Test]
        public void FinishTransition_Property_ReturnsFinishTransitionSubscriber()
        {
            Assert.That(_useCase.finishTransition, Is.EqualTo(_subscriber));
        }

        [Test]
        public async Task InitAsync_PublishesOnce()
        {
            await _useCase.InitAsync(FinishType.Clear, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public async Task InitAsync_PublishesWithFadeOut()
        {
            await _useCase.InitAsync(FinishType.Clear, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.transition.fade, Is.EqualTo(Fade.Out));
        }

        [TestCase(FinishType.Clear)]
        [TestCase(FinishType.Fail)]
        public async Task InitAsync_PublishesCorrectType(FinishType type)
        {
            await _useCase.InitAsync(type, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.type, Is.EqualTo(type));
        }

        [Test]
        public async Task FadeAsync_PublishesOnce()
        {
            await _useCase.FadeAsync(FinishType.Clear, Fade.In, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(1));
        }

        [TestCase(FinishType.Clear, Fade.In)]
        [TestCase(FinishType.Fail, Fade.Out)]
        public async Task FadeAsync_PublishesCorrectTypeAndFade(FinishType type, Fade fade)
        {
            await _useCase.FadeAsync(type, fade, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.type, Is.EqualTo(type));
            Assert.That(_publisher.LastPublished.transition.fade, Is.EqualTo(fade));
        }

        [Test]
        public async Task FadeAsync_SetsUiDurationFromConfig()
        {
            await _useCase.FadeAsync(FinishType.Clear, Fade.In, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.transition.duration, Is.EqualTo(UiConfig.DURATION));
        }

        [Test]
        public async Task InitAsync_CalledMultipleTimes_IncrementsCount()
        {
            await _useCase.InitAsync(FinishType.Clear, CancellationToken.None).AsTask();
            await _useCase.InitAsync(FinishType.Fail, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(2));
        }
    }
}
