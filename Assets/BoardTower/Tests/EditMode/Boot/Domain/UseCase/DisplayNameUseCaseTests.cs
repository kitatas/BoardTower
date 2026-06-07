using System;
using System.Threading;
using System.Threading.Tasks;
using BoardTower.Boot.Application;
using BoardTower.Boot.Domain.Ports;
using BoardTower.Boot.Domain.UseCase;
using BoardTower.Common.Application;
using BoardTower.Tests.EditMode.TestHelpers;
using Cysharp.Threading.Tasks;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Boot.Domain.UseCase
{
    [TestFixture]
    public sealed class DisplayNameUseCaseTests
    {
        private DisplayNameUseCase _useCase;
        private FakeAsyncPublisher<DisplayNameTransitionVO> _publisher;

        [SetUp]
        public void SetUp()
        {
            _publisher = new FakeAsyncPublisher<DisplayNameTransitionVO>();
            var subscriber = new FakeAsyncSubscriber<DisplayNameTransitionVO>();
            var ports = new DisplayNamePorts(subscriber, _publisher);
            _useCase = new DisplayNameUseCase(ports);
        }

        [TearDown]
        public void TearDown()
        {
            ((IDisposable)_useCase).Dispose();
        }

        [Test]
        public void Transition_ReturnsNonNull()
        {
            Assert.That(_useCase.transition, Is.Not.Null);
        }

        [Test]
        public async Task InitAsync_PublishesFadeOutTransition()
        {
            await _useCase.InitAsync(CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(1));
            Assert.That(_publisher.LastPublished.transition.fade, Is.EqualTo(Fade.Out));
            Assert.That(_publisher.LastPublished.transition.duration, Is.EqualTo(0.0f));
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
        public async Task FadeAsync_PublishesDurationFromUiConfig()
        {
            await _useCase.FadeAsync(Fade.In, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.transition.duration, Is.EqualTo(UiConfig.DURATION));
        }

        [Test]
        public async Task FadeAsync_CalledMultipleTimes_IncrementsPublishCount()
        {
            await _useCase.FadeAsync(Fade.In, CancellationToken.None).AsTask();
            await _useCase.FadeAsync(Fade.Out, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(2));
        }

        [Test]
        public void HandleDisplayName_DoesNotThrow()
        {
            Assert.That(() => _useCase.HandleDisplayName("TestName"), Throws.Nothing);
        }

        [Test]
        public void HandleDisplayName_WithEmptyString_DoesNotThrow()
        {
            Assert.That(() => _useCase.HandleDisplayName(""), Throws.Nothing);
        }

        [Test]
        public void Dispose_DoesNotThrow()
        {
            Assert.That(() => ((IDisposable)_useCase).Dispose(), Throws.Nothing);
        }

        [Test]
        public void HandleDisplayName_AfterDispose_DoesNotThrow()
        {
            ((IDisposable)_useCase).Dispose();

            Assert.That(() => _useCase.HandleDisplayName("test"), Throws.Nothing);
        }
    }
}
