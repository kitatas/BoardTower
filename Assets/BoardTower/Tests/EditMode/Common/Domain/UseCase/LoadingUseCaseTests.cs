using System.Threading;
using System.Threading.Tasks;
using BoardTower.Common.Application;
using BoardTower.Common.Domain.Ports;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Tests.EditMode.TestHelpers;
using Cysharp.Threading.Tasks;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Common.Domain.UseCase
{
    [TestFixture]
    public sealed class LoadingUseCaseTests
    {
        private LoadingUseCase _useCase;
        private FakeAsyncPublisher<LoadingTransitionVO> _publisher;
        private FakeAsyncSubscriber<LoadingTransitionVO> _subscriber;

        [SetUp]
        public void SetUp()
        {
            _publisher = new FakeAsyncPublisher<LoadingTransitionVO>();
            _subscriber = new FakeAsyncSubscriber<LoadingTransitionVO>();
            var ports = new LoadingPorts(_subscriber, _publisher);
            _useCase = new LoadingUseCase(ports);
        }

        [Test]
        public void Transition_ReturnsPortsSubscriber()
        {
            Assert.That(_useCase.transition, Is.SameAs(_subscriber));
        }

        [Test]
        public async Task InitAsync_PublishesFadeOut()
        {
            await _useCase.InitAsync(CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.transition.fade, Is.EqualTo(Fade.Out));
        }

        [Test]
        public async Task InitAsync_PublishesZeroDuration()
        {
            await _useCase.InitAsync(CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.transition.duration, Is.EqualTo(0.0f).Within(1e-6f));
        }

        [Test]
        public async Task InitAsync_CallsPublisherOnce()
        {
            await _useCase.InitAsync(CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(1));
        }

        [TestCase(Fade.In)]
        [TestCase(Fade.Out)]
        [TestCase(Fade.InOut)]
        public async Task FadeAsync_PublishesGivenFade(Fade fade)
        {
            await _useCase.FadeAsync(fade, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.transition.fade, Is.EqualTo(fade));
        }

        [Test]
        public async Task FadeAsync_PublishesConfiguredDuration()
        {
            await _useCase.FadeAsync(Fade.In, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.transition.duration,
                Is.EqualTo(LoadingConfig.FADE_DURATION).Within(1e-6f));
        }

        [Test]
        public async Task FadeAsync_CallsPublisherOnce()
        {
            await _useCase.FadeAsync(Fade.In, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public void FadeAsync_WithFadeNone_ThrowsQuitExceptionVO()
        {
            Assert.That(() => _useCase.FadeAsync(Fade.None, CancellationToken.None), Throws.TypeOf<QuitExceptionVO>());
        }
    }
}
