using System.Threading;
using System.Threading.Tasks;
using BoardTower.Common.Application;
using BoardTower.Common.Data.Entity;
using BoardTower.Common.Domain.Ports;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Tests.EditMode.TestHelpers;
using Cysharp.Threading.Tasks;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Common.Domain.UseCase
{
    [TestFixture]
    public sealed class GameModeUseCaseTests
    {
        private GameModeUseCase _useCase;
        private GameModeEntity _entity;
        private FakeAsyncPublisher<GameModeTransitionVO> _publisher;
        private FakeAsyncSubscriber<GameModeTransitionVO> _subscriber;

        [SetUp]
        public void SetUp()
        {
            _entity = new GameModeEntity();
            _publisher = new FakeAsyncPublisher<GameModeTransitionVO>();
            _subscriber = new FakeAsyncSubscriber<GameModeTransitionVO>();
            var ports = new GameModePorts(_subscriber, _publisher);
            _useCase = new GameModeUseCase(_entity, ports);
        }

        [Test]
        public void GameModeTransition_ReturnsPortsSubscriber()
        {
            Assert.That(_useCase.gameModeTransition, Is.SameAs(_subscriber));
        }

        [TestCase(GameMode.Online, true)]
        [TestCase(GameMode.Offline, false)]
        public void IsOnlineMode_WithEntityMode_ReturnsExpected(GameMode mode, bool expected)
        {
            _entity.Set(new GameModeVO(mode));

            Assert.That(_useCase.isOnlineMode, Is.EqualTo(expected));
        }

        [Test]
        public async Task InitAsync_PublishesOfflineMode()
        {
            await _useCase.InitAsync(CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.gameMode, Is.EqualTo(GameMode.Offline));
        }

        [Test]
        public async Task InitAsync_WhenEntityIsOnline_StillPublishesOfflineMode()
        {
            // NOTE: 初期化前なので Entity の状態に関わらず Offline 固定
            _entity.Set(new GameModeVO(GameMode.Online));

            await _useCase.InitAsync(CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.gameMode, Is.EqualTo(GameMode.Offline));
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

        [TestCase(GameMode.Online)]
        [TestCase(GameMode.Offline)]
        public async Task FadeAsync_PublishesCurrentEntityMode(GameMode mode)
        {
            _entity.Set(new GameModeVO(mode));

            await _useCase.FadeAsync(Fade.In, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.gameMode, Is.EqualTo(mode));
        }

        [TestCase(Fade.In)]
        [TestCase(Fade.Out)]
        [TestCase(Fade.InOut)]
        public async Task FadeAsync_PublishesGivenFade(Fade fade)
        {
            _entity.Set(new GameModeVO(GameMode.Online));

            await _useCase.FadeAsync(fade, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.transition.fade, Is.EqualTo(fade));
        }

        [Test]
        public async Task FadeAsync_PublishesConfiguredDuration()
        {
            _entity.Set(new GameModeVO(GameMode.Online));

            await _useCase.FadeAsync(Fade.In, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.transition.duration,
                Is.EqualTo(GameModeConfig.FADE_DURATION).Within(1e-6f));
        }

        [Test]
        public void FadeAsync_WithFadeNone_ThrowsQuitExceptionVO()
        {
            _entity.Set(new GameModeVO(GameMode.Online));

            Assert.That(() => _useCase.FadeAsync(Fade.None, CancellationToken.None), Throws.TypeOf<QuitExceptionVO>());
        }

        [Test]
        public async Task JudgeGameMode_SetsModeMatchingInternetReachability()
        {
            // NOTE: 実行環境のネットワーク状態に依存するため、期待値も同じ API から算出する
            var expected = UnityEngine.Application.internetReachability != UnityEngine.NetworkReachability.NotReachable
                ? GameMode.Online
                : GameMode.Offline;

            await _useCase.JudgeGameMode(CancellationToken.None).AsTask();

            Assert.That(_entity.value.mode, Is.EqualTo(expected));
        }
    }
}
