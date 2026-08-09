using System;
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
    public sealed class TapScreenUseCaseTests
    {
        private TapScreenUseCase _useCase;
        private FakeAsyncPublisher<TapScreenTransitionVO> _publisher;
        private FakeAsyncSubscriber<TapScreenTransitionVO> _subscriber;
        private TapScreenPorts _ports;

        [SetUp]
        public void SetUp()
        {
            _publisher = new FakeAsyncPublisher<TapScreenTransitionVO>();
            _subscriber = new FakeAsyncSubscriber<TapScreenTransitionVO>();
            _ports = new TapScreenPorts(_subscriber, _publisher);
            _useCase = new TapScreenUseCase(_ports);
        }

        [TearDown]
        public void TearDown()
        {
            ((IDisposable)_useCase).Dispose();
        }

        [Test]
        public void TapScreenTransition_Property_ReturnsTapScreenTransitionSubscriber()
        {
            Assert.That(_useCase.tapScreenTransition, Is.EqualTo(_subscriber));
        }

        [Test]
        public async Task FadeAsync_WithFadeOut_PublishesOnce()
        {
            await _useCase.FadeAsync(Fade.Out, CancellationToken.None).AsTask();

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
            await _useCase.FadeAsync(Fade.Out, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.transition.duration, Is.EqualTo(TapScreenConfig.FADE_DURATION));
        }

        [Test]
        public async Task FadeAsync_CalledMultipleTimes_IncrementsCount()
        {
            await _useCase.FadeAsync(Fade.Out, CancellationToken.None).AsTask();
            await _useCase.FadeAsync(Fade.In, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(2));
        }

        [Test]
        public async Task WaitForTapScreenAsync_AfterNotify_Completes()
        {
            using var cts = new CancellationTokenSource();
            var task = _useCase.WaitForTapScreenAsync(cts.Token).AsTask();

            _useCase.NotifyTapScreen();

            await task;
            Assert.That(task.IsCompletedSuccessfully, Is.True);
        }

        [Test]
        public void NotifyTapScreen_DoesNotThrow()
        {
            Assert.That(() => _useCase.NotifyTapScreen(), Throws.Nothing);
        }

        [Test]
        public void Dispose_DoesNotThrow()
        {
            Assert.That(() => ((IDisposable)_useCase).Dispose(), Throws.Nothing);
        }
    }
}
