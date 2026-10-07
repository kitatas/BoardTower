using System.Threading;
using System.Threading.Tasks;
using BoardTower.Common.Application;
using BoardTower.Common.Domain.Ports;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Tests.EditMode.TestHelpers;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using R3;

namespace BoardTower.Tests.EditMode.Common.Domain.UseCase
{
    [TestFixture]
    public sealed class ExceptionUseCaseTests
    {
        private ExceptionUseCase _useCase;
        private FakeAsyncPublisher<ExceptionNotifyVO> _notifyPublisher;
        private FakeAsyncPublisher<ExceptionActionVO> _actionPublisher;
        private FakeAsyncSubscriber<ExceptionNotifyVO> _notifySubscriber;
        private FakeAsyncSubscriber<ExceptionActionVO> _actionSubscriber;

        [SetUp]
        public void SetUp()
        {
            _notifyPublisher = new FakeAsyncPublisher<ExceptionNotifyVO>();
            _actionPublisher = new FakeAsyncPublisher<ExceptionActionVO>();
            _notifySubscriber = new FakeAsyncSubscriber<ExceptionNotifyVO>();
            _actionSubscriber = new FakeAsyncSubscriber<ExceptionActionVO>();
            var ports = new ExceptionPorts(_notifySubscriber, _actionSubscriber, _notifyPublisher, _actionPublisher);
            _useCase = new ExceptionUseCase(ports);
        }

        [TearDown]
        public void TearDown()
        {
            _useCase.Dispose();
        }

        // ThrowAsync を開始し、ユーザーの決定操作 (HandleDecision) まで完了させる
        private async Task CompleteThrowAsync(UniTask throwTask)
        {
            _useCase.HandleDecision(Unit.Default);
            await throwTask.AsTask();
        }

        [Test]
        public void ExceptionNotify_ReturnsPortsNotifySubscriber()
        {
            Assert.That(_useCase.exceptionNotify, Is.SameAs(_notifySubscriber));
        }

        [Test]
        public void ExceptionAction_ReturnsPortsActionSubscriber()
        {
            Assert.That(_useCase.exceptionAction, Is.SameAs(_actionSubscriber));
        }

        [Test]
        public async Task ThrowAsync_BeforeDecision_IsPending()
        {
            var task = _useCase.ThrowAsync(new RetryExceptionVO("e"), CancellationToken.None);

            Assert.That(task.Status, Is.EqualTo(UniTaskStatus.Pending));

            await CompleteThrowAsync(task);
        }

        [Test]
        public async Task ThrowAsync_BeforeDecision_PublishesFadeInNotify()
        {
            var task = _useCase.ThrowAsync(new RetryExceptionVO("e"), CancellationToken.None);

            Assert.That(_notifyPublisher.LastPublished.transition.fade, Is.EqualTo(Fade.In));

            await CompleteThrowAsync(task);
        }

        [Test]
        public async Task ThrowAsync_BeforeDecision_PublishesNotifyWithConfiguredDuration()
        {
            var task = _useCase.ThrowAsync(new RetryExceptionVO("e"), CancellationToken.None);

            Assert.That(_notifyPublisher.LastPublished.transition.duration,
                Is.EqualTo(ExceptionConfig.FADE_DURATION).Within(1e-6f));

            await CompleteThrowAsync(task);
        }

        [Test]
        public async Task ThrowAsync_BeforeDecision_PublishesNotifyWithGivenException()
        {
            var ex = new RetryExceptionVO("e");
            var task = _useCase.ThrowAsync(ex, CancellationToken.None);

            Assert.That(_notifyPublisher.LastPublished.exception, Is.SameAs(ex));

            await CompleteThrowAsync(task);
        }

        [Test]
        public async Task ThrowAsync_BeforeDecision_DoesNotPublishAction()
        {
            var task = _useCase.ThrowAsync(new RetryExceptionVO("e"), CancellationToken.None);

            Assert.That(_actionPublisher.PublishCount, Is.EqualTo(0));

            await CompleteThrowAsync(task);
        }

        [Test]
        public async Task ThrowAsync_WhenDecisionWasMadeBeforeThrow_StaysPending()
        {
            // 過去の決定操作は引き継がれず、Throw 後の決定を待つ
            _useCase.HandleDecision(Unit.Default);

            var task = _useCase.ThrowAsync(new RetryExceptionVO("e"), CancellationToken.None);

            Assert.That(task.Status, Is.EqualTo(UniTaskStatus.Pending));

            await CompleteThrowAsync(task);
        }

        [Test]
        public async Task ThrowAsync_AfterDecision_PublishesFadeOutNotifyWithoutException()
        {
            var task = _useCase.ThrowAsync(new RetryExceptionVO("e"), CancellationToken.None);

            await CompleteThrowAsync(task);

            // NOTE: FadeOut 時は Exception 不要なので null で publish される
            Assert.That(_notifyPublisher.LastPublished.exception, Is.Null);
        }

        [Test]
        public async Task ThrowAsync_AfterDecision_PublishesFadeOutNotify()
        {
            var task = _useCase.ThrowAsync(new RetryExceptionVO("e"), CancellationToken.None);

            await CompleteThrowAsync(task);

            Assert.That(_notifyPublisher.LastPublished.transition.fade, Is.EqualTo(Fade.Out));
        }

        [Test]
        public async Task ThrowAsync_AfterDecision_PublishesNotifyTwice()
        {
            var task = _useCase.ThrowAsync(new RetryExceptionVO("e"), CancellationToken.None);

            await CompleteThrowAsync(task);

            Assert.That(_notifyPublisher.PublishCount, Is.EqualTo(2));
        }

        [Test]
        public async Task ThrowAsync_AfterDecision_PublishesActionWithGivenException()
        {
            var ex = new RetryExceptionVO("e");
            var task = _useCase.ThrowAsync(ex, CancellationToken.None);

            await CompleteThrowAsync(task);

            Assert.That(_actionPublisher.LastPublished.exception, Is.SameAs(ex));
        }

        [Test]
        public async Task ThrowAsync_AfterDecision_PublishesActionOnce()
        {
            var task = _useCase.ThrowAsync(new RetryExceptionVO("e"), CancellationToken.None);

            await CompleteThrowAsync(task);

            Assert.That(_actionPublisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public async Task ThrowRebootAsync_PublishesRebootExceptionAction()
        {
            var task = _useCase.ThrowRebootAsync("msg", CancellationToken.None);

            await CompleteThrowAsync(task);

            Assert.That(_actionPublisher.LastPublished.exception, Is.TypeOf<RebootExceptionVO>());
        }

        [Test]
        public async Task ThrowRetryAsync_PublishesRetryExceptionAction()
        {
            var task = _useCase.ThrowRetryAsync("msg", CancellationToken.None);

            await CompleteThrowAsync(task);

            Assert.That(_actionPublisher.LastPublished.exception, Is.TypeOf<RetryExceptionVO>());
        }

        [Test]
        public async Task ThrowQuitAsync_PublishesQuitExceptionAction()
        {
            var task = _useCase.ThrowQuitAsync("msg", CancellationToken.None);

            await CompleteThrowAsync(task);

            Assert.That(_actionPublisher.LastPublished.exception, Is.TypeOf<QuitExceptionVO>());
        }

        [Test]
        public async Task ThrowQuitAsync_PassesMessageToException()
        {
            var task = _useCase.ThrowQuitAsync("quit message", CancellationToken.None);

            await CompleteThrowAsync(task);

            Assert.That(_actionPublisher.LastPublished.exception.Message, Is.EqualTo("quit message"));
        }

        [Test]
        public async Task FadeOutAsync_PublishesFadeOutNotify()
        {
            await _useCase.FadeOutAsync(0.3f, CancellationToken.None).AsTask();

            Assert.That(_notifyPublisher.LastPublished.transition.fade, Is.EqualTo(Fade.Out));
        }

        [TestCase(0.0f)]
        [TestCase(0.3f)]
        public async Task FadeOutAsync_PublishesNotifyWithGivenDuration(float duration)
        {
            await _useCase.FadeOutAsync(duration, CancellationToken.None).AsTask();

            Assert.That(_notifyPublisher.LastPublished.transition.duration, Is.EqualTo(duration).Within(1e-6f));
        }

        [Test]
        public async Task FadeOutAsync_PublishesNotifyWithoutException()
        {
            await _useCase.FadeOutAsync(0.3f, CancellationToken.None).AsTask();

            Assert.That(_notifyPublisher.LastPublished.exception, Is.Null);
        }

        [Test]
        public void FadeOutAsync_WithNegativeDuration_ThrowsQuitExceptionVO()
        {
            Assert.That(() => _useCase.FadeOutAsync(-1.0f, CancellationToken.None), Throws.TypeOf<QuitExceptionVO>());
        }

        [Test]
        public void HandleDecision_WithoutPendingThrow_DoesNotThrow()
        {
            Assert.That(() => _useCase.HandleDecision(Unit.Default), Throws.Nothing);
        }
    }
}
