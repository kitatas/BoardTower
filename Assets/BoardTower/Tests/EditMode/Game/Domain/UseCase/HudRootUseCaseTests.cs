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
    public sealed class HudRootUseCaseTests
    {
        private HudRootUseCase _useCase;
        private FakeAsyncPublisher<HudRootTransitionVO> _publisher;
        private FakeAsyncSubscriber<HudRootTransitionVO> _subscriber;
        private HudRootPorts _ports;

        [SetUp]
        public void SetUp()
        {
            _publisher = new FakeAsyncPublisher<HudRootTransitionVO>();
            _subscriber = new FakeAsyncSubscriber<HudRootTransitionVO>();
            _ports = new HudRootPorts(_subscriber, _publisher);
            _useCase = new HudRootUseCase(_ports);
        }

        [Test]
        public void Transition_Property_ReturnsHudRootTransitionSubscriber()
        {
            Assert.That(_useCase.transition, Is.EqualTo(_subscriber));
        }

        [Test]
        public async Task InitAsync_PublishesOnce()
        {
            await _useCase.InitAsync(CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public async Task InitAsync_PublishesWithFadeOut()
        {
            await _useCase.InitAsync(CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.transition.fade, Is.EqualTo(Fade.Out));
        }

        [Test]
        public async Task InitAsync_PublishesWithZeroDuration()
        {
            await _useCase.InitAsync(CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.transition.duration, Is.EqualTo(0.0f));
        }

        [Test]
        public async Task FadeAsync_PublishesOnce()
        {
            await _useCase.FadeAsync(Fade.In, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(1));
        }

        [TestCase(Fade.In)]
        [TestCase(Fade.Out)]
        [TestCase(Fade.InOut)]
        public async Task FadeAsync_PublishesCorrectFade(Fade fade)
        {
            await _useCase.FadeAsync(fade, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.transition.fade, Is.EqualTo(fade));
        }

        [Test]
        public async Task FadeAsync_SetsCorrectDurationFromConfig()
        {
            await _useCase.FadeAsync(Fade.In, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.transition.duration, Is.EqualTo(HudRootConfig.FADE_DURATION));
        }

        [Test]
        public async Task FadeAsync_CalledMultipleTimes_IncrementsCount()
        {
            await _useCase.FadeAsync(Fade.Out, CancellationToken.None).AsTask();
            await _useCase.FadeAsync(Fade.In, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(2));
        }
    }
}
