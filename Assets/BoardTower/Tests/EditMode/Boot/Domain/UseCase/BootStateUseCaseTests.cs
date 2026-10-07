using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BoardTower.Boot.Application;
using BoardTower.Boot.Data.Entity;
using BoardTower.Boot.Domain.Ports;
using BoardTower.Boot.Domain.UseCase;
using BoardTower.Common.Application;
using BoardTower.Common.Data.Entity;
using BoardTower.Tests.EditMode.TestHelpers;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using R3;

namespace BoardTower.Tests.EditMode.Boot.Domain.UseCase
{
    [TestFixture]
    public sealed class BootStateUseCaseTests
    {
        private BootStateUseCase _useCase;
        private BootStateEntity _entity;
        private RetryCountEntity _retryCountEntity;
        private FakeAsyncPublisher<BootState> _publisher;

        [SetUp]
        public void SetUp()
        {
            _entity = new BootStateEntity();
            _retryCountEntity = new RetryCountEntity();
            _publisher = new FakeAsyncPublisher<BootState>();
            var subscriber = new FakeAsyncSubscriber<BootState>();
            var ports = new BootStatePorts(subscriber, _publisher);
            _useCase = new BootStateUseCase(_entity, _retryCountEntity, ports);
        }

        [TearDown]
        public void TearDown()
        {
            _useCase.Dispose();
        }

        // ---- InitAsync ----

        [Test]
        public async Task InitAsync_PublishesInitState()
        {
            await _useCase.InitAsync(CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished, Is.EqualTo(BootState.Init));
        }

        [Test]
        public async Task InitAsync_SetsEntityToInit()
        {
            await _useCase.InitAsync(CancellationToken.None).AsTask();

            Assert.That(_entity.value, Is.EqualTo(BootState.Init));
        }

        // ---- PublishAsync ----

        [TestCase(BootState.Init)]
        [TestCase(BootState.Load)]
        [TestCase(BootState.Splash)]
        [TestCase(BootState.Login)]
        public async Task PublishAsync_WithValidState_PublishesState(BootState state)
        {
            await _useCase.PublishAsync(state, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished, Is.EqualTo(state));
        }

        [TestCase(BootState.Init)]
        [TestCase(BootState.Load)]
        [TestCase(BootState.Splash)]
        [TestCase(BootState.Login)]
        public async Task PublishAsync_WithValidState_SetsEntity(BootState state)
        {
            await _useCase.PublishAsync(state, CancellationToken.None).AsTask();

            Assert.That(_entity.value, Is.EqualTo(state));
        }

        [Test]
        public void PublishAsync_WithNone_DoesNotPublish()
        {
            // NOTE: None 指定時は UniTask.Yield(token) を返す。EditMode では PlayerLoop に依存しないよう、
            //       キャンセル済みトークンで即時に完了させ、結果は待たずに破棄する
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            _useCase.PublishAsync(BootState.None, cts.Token).Forget();

            Assert.That(_publisher.PublishCount, Is.EqualTo(0));
        }

        [Test]
        public void PublishAsync_WithNone_DoesNotChangeEntity()
        {
            _entity.Set(BootState.Load);
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            _useCase.PublishAsync(BootState.None, cts.Token).Forget();

            Assert.That(_entity.value, Is.EqualTo(BootState.Load));
        }

        // ---- ForceChange ----

        [Test]
        public void ForceChangeState_Initially_IsNone()
        {
            Assert.That(_useCase.forceChangeState, Is.EqualTo(BootState.None));
        }

        [Test]
        public void ForceChange_WithDifferentStateFromEntity_UpdatesForceChangeState()
        {
            _entity.Set(BootState.Init);

            _useCase.ForceChange(BootState.Load);

            Assert.That(_useCase.forceChangeState, Is.EqualTo(BootState.Load));
        }

        [Test]
        public void ForceChange_WithSameStateAsEntity_DoesNotUpdateForceChangeState()
        {
            _entity.Set(BootState.Init);

            _useCase.ForceChange(BootState.Init);

            Assert.That(_useCase.forceChangeState, Is.EqualTo(BootState.None));
        }

        [Test]
        public void ForceChange_WithDifferentStateFromEntity_NotifiesObservers()
        {
            _entity.Set(BootState.Init);
            var observed = new List<BootState>();
            using var subscription = _useCase.forceChange.Subscribe(x => observed.Add(x));

            _useCase.ForceChange(BootState.Login);

            // BehaviorSubject のため、購読時の初期値 None に続いて通知される
            Assert.That(observed, Is.EqualTo(new[] { BootState.None, BootState.Login }));
        }

        // ---- IsMaxRetry ----

        [Test]
        public void IsMaxRetry_WithSameStateUpToMaxRetryCount_ReturnsFalse()
        {
            _entity.Set(BootState.Load);
            for (var i = 0; i < ExceptionConfig.MAX_RETRY_COUNT - 1; i++)
            {
                _useCase.IsMaxRetry(BootState.Load);
            }

            var result = _useCase.IsMaxRetry(BootState.Load);

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsMaxRetry_WithSameStateExceedingMaxRetryCount_ReturnsTrue()
        {
            _entity.Set(BootState.Load);
            for (var i = 0; i < ExceptionConfig.MAX_RETRY_COUNT; i++)
            {
                _useCase.IsMaxRetry(BootState.Load);
            }

            var result = _useCase.IsMaxRetry(BootState.Load);

            Assert.That(result, Is.True);
        }

        [Test]
        public void IsMaxRetry_WithDifferentStateAfterExceeding_ReturnsFalse()
        {
            _entity.Set(BootState.Load);
            for (var i = 0; i <= ExceptionConfig.MAX_RETRY_COUNT; i++)
            {
                _useCase.IsMaxRetry(BootState.Load);
            }

            var result = _useCase.IsMaxRetry(BootState.Login);

            Assert.That(result, Is.False);
        }
    }
}
