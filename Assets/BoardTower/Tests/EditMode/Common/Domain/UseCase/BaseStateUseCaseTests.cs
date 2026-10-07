using System.Collections.Generic;
using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Data.Entity;
using BoardTower.Common.Domain.Ports;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Tests.EditMode.TestHelpers;
using Cysharp.Threading.Tasks;
using MessagePipe;
using NUnit.Framework;
using R3;

namespace BoardTower.Tests.EditMode.Common.Domain.UseCase
{
    /// <summary>
    /// 抽象クラス BaseStateUseCase&lt;T&gt; のテスト。最小限のテスト用サブクラスで検証する。
    /// BaseStatePorts / BaseStateEntity の結線も兼ねる。
    /// </summary>
    [TestFixture]
    public sealed class BaseStateUseCaseTests
    {
        public enum TestState
        {
            None,
            First,
            Second,
        }

        private sealed class TestStateEntity : BaseStateEntity<TestState>
        {
        }

        private sealed class TestStatePorts : BaseStatePorts<TestState>
        {
            public TestStatePorts(IAsyncSubscriber<TestState> subscriber, IAsyncPublisher<TestState> publisher) : base(
                subscriber, publisher)
            {
            }
        }

        private sealed class TestStateUseCase : BaseStateUseCase<TestState>
        {
            public TestStateUseCase(BaseEntity<TestState> entity, RetryCountEntity retryCountEntity,
                BaseStatePorts<TestState> ports) : base(entity, retryCountEntity, ports)
            {
            }

            public override UniTask InitAsync(CancellationToken token) => UniTask.CompletedTask;
        }

        private TestStateUseCase _useCase;
        private TestStateEntity _entity;
        private RetryCountEntity _retryCountEntity;
        private CompositeDisposable _disposables;

        [SetUp]
        public void SetUp()
        {
            _entity = new TestStateEntity();
            _retryCountEntity = new RetryCountEntity();
            var publisher = new FakeAsyncPublisher<TestState>();
            var subscriber = new FakeAsyncSubscriber<TestState>();
            var ports = new TestStatePorts(subscriber, publisher);
            _useCase = new TestStateUseCase(_entity, _retryCountEntity, ports);
            _disposables = new CompositeDisposable();
        }

        [TearDown]
        public void TearDown()
        {
            _disposables.Dispose();
            _useCase.Dispose();
        }

        [Test]
        public void ForceChangeState_Initially_IsDefault()
        {
            Assert.That(_useCase.forceChangeState, Is.EqualTo(TestState.None));
        }

        [Test]
        public void ForceChange_WithDifferentState_UpdatesForceChangeState()
        {
            _useCase.ForceChange(TestState.First);

            Assert.That(_useCase.forceChangeState, Is.EqualTo(TestState.First));
        }

        [Test]
        public void ForceChange_WithDifferentState_NotifiesSubscriber()
        {
            var received = new List<TestState>();
            _disposables.Add(_useCase.forceChange.Subscribe(x => received.Add(x)));

            _useCase.ForceChange(TestState.First);

            // BehaviorSubject のため、購読直後の初期値 (None) に続いて通知される
            Assert.That(received, Is.EqualTo(new[] { TestState.None, TestState.First }));
        }

        [Test]
        public void ForceChange_WithSameStateAsEntity_DoesNotNotify()
        {
            _entity.Set(TestState.First);
            var received = new List<TestState>();
            _disposables.Add(_useCase.forceChange.Subscribe(x => received.Add(x)));

            _useCase.ForceChange(TestState.First);

            // 購読直後の初期値 (None) のみで、追加の通知は無い
            Assert.That(received, Is.EqualTo(new[] { TestState.None }));
        }

        [Test]
        public void ForceChange_WithSameStateAsEntity_KeepsForceChangeState()
        {
            _entity.Set(TestState.First);

            _useCase.ForceChange(TestState.First);

            Assert.That(_useCase.forceChangeState, Is.EqualTo(TestState.None));
        }

        [Test]
        public void IsMaxRetry_FirstCall_ReturnsFalse()
        {
            _entity.Set(TestState.First);

            Assert.That(_useCase.IsMaxRetry(TestState.First), Is.False);
        }

        [Test]
        public void IsMaxRetry_WhenCalledMaxRetryCountTimesWithSameState_ReturnsFalse()
        {
            _entity.Set(TestState.First);
            var result = false;

            for (var i = 0; i < ExceptionConfig.MAX_RETRY_COUNT; i++)
            {
                result = _useCase.IsMaxRetry(TestState.First);
            }

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsMaxRetry_WhenCalledOverMaxRetryCountWithSameState_ReturnsTrue()
        {
            _entity.Set(TestState.First);
            var result = false;

            for (var i = 0; i < ExceptionConfig.MAX_RETRY_COUNT + 1; i++)
            {
                result = _useCase.IsMaxRetry(TestState.First);
            }

            Assert.That(result, Is.True);
        }

        [Test]
        public void IsMaxRetry_WhenStateDiffersFromEntity_ResetsRetryCountToOne()
        {
            _entity.Set(TestState.First);
            _useCase.IsMaxRetry(TestState.First);
            _useCase.IsMaxRetry(TestState.First);

            _useCase.IsMaxRetry(TestState.Second);

            Assert.That(_retryCountEntity.value, Is.EqualTo(1));
        }

        [Test]
        public void IsMaxRetry_WhenStateDiffersAfterManyRetries_ReturnsFalse()
        {
            _entity.Set(TestState.First);
            for (var i = 0; i < ExceptionConfig.MAX_RETRY_COUNT + 1; i++)
            {
                _useCase.IsMaxRetry(TestState.First);
            }

            Assert.That(_useCase.IsMaxRetry(TestState.Second), Is.False);
        }
    }
}
