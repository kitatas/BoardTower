using System.Threading;
using System.Threading.Tasks;
using BoardTower.Common.Application;
using BoardTower.Common.Data.Entity;
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
    public sealed class GameStateUseCaseTests
    {
        private GameStateUseCase _useCase;
        private GameStateEntity _entity;
        private RetryCountEntity _retryCountEntity;
        private FakeAsyncPublisher<GameState> _publisher;
        private FakeAsyncSubscriber<GameState> _subscriber;
        private GameStatePorts _ports;

        [SetUp]
        public void SetUp()
        {
            _entity = new GameStateEntity();
            _retryCountEntity = new RetryCountEntity();
            _publisher = new FakeAsyncPublisher<GameState>();
            _subscriber = new FakeAsyncSubscriber<GameState>();
            _ports = new GameStatePorts(_subscriber, _publisher);
            _useCase = new GameStateUseCase(_entity, _retryCountEntity, _ports);
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
        public async Task InitAsync_PublishesInitState()
        {
            await _useCase.InitAsync(CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished, Is.EqualTo(GameState.Init));
        }

        [Test]
        public async Task InitAsync_PublishesOnce()
        {
            await _useCase.InitAsync(CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public async Task InitAsync_StoresInitStateInEntity()
        {
            await _useCase.InitAsync(CancellationToken.None).AsTask();

            Assert.That(_entity.value, Is.EqualTo(GameState.Init));
        }

        [TestCase(GameState.SetUp)]
        [TestCase(GameState.Input)]
        [TestCase(GameState.Send)]
        public async Task PublishAsync_WithState_PublishesGivenState(GameState state)
        {
            await _useCase.PublishAsync(state, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished, Is.EqualTo(state));
        }

        [Test]
        public async Task PublishAsync_WithState_StoresStateInEntity()
        {
            await _useCase.PublishAsync(GameState.Judge, CancellationToken.None).AsTask();

            Assert.That(_entity.value, Is.EqualTo(GameState.Judge));
        }

        [Test]
        public void PublishAsync_WithNone_DoesNotPublish()
        {
            // None は UniTask.Yield を返すため await せず、キャンセル済みトークンで PlayerLoop 非依存にする
            _ = _useCase.PublishAsync(GameState.None, new CancellationToken(true));

            Assert.That(_publisher.PublishCount, Is.EqualTo(0));
        }

        [Test]
        public void PublishAsync_WithNone_KeepsEntityValue()
        {
            _entity.Set(GameState.Input);

            _ = _useCase.PublishAsync(GameState.None, new CancellationToken(true));

            Assert.That(_entity.value, Is.EqualTo(GameState.Input));
        }

        [Test]
        public void ForceChange_WithDifferentState_UpdatesForceChangeState()
        {
            _entity.Set(GameState.Input);

            _useCase.ForceChange(GameState.Fail);

            Assert.That(_useCase.forceChangeState, Is.EqualTo(GameState.Fail));
        }

        [Test]
        public void ForceChange_WithCurrentState_DoesNotUpdateForceChangeState()
        {
            _entity.Set(GameState.Input);

            _useCase.ForceChange(GameState.Input);

            Assert.That(_useCase.forceChangeState, Is.EqualTo(GameState.None));
        }

        [Test]
        public void IsMaxRetry_WithSameStateWithinMaxRetryCount_ReturnsFalse()
        {
            _entity.Set(GameState.Input);
            for (int i = 0; i < ExceptionConfig.MAX_RETRY_COUNT - 1; i++)
            {
                _useCase.IsMaxRetry(GameState.Input);
            }

            Assert.That(_useCase.IsMaxRetry(GameState.Input), Is.False);
        }

        [Test]
        public void IsMaxRetry_WithSameStateOverMaxRetryCount_ReturnsTrue()
        {
            _entity.Set(GameState.Input);
            for (int i = 0; i < ExceptionConfig.MAX_RETRY_COUNT; i++)
            {
                _useCase.IsMaxRetry(GameState.Input);
            }

            Assert.That(_useCase.IsMaxRetry(GameState.Input), Is.True);
        }

        [Test]
        public void IsMaxRetry_WithDifferentState_ResetsRetryCount()
        {
            _entity.Set(GameState.Input);
            _retryCountEntity.Set(ExceptionConfig.MAX_RETRY_COUNT + 1);

            Assert.That(_useCase.IsMaxRetry(GameState.Fail), Is.False);
        }
    }
}
