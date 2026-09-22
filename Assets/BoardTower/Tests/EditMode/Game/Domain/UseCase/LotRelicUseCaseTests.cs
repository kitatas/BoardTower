using System;
using System.Threading;
using System.Threading.Tasks;
using BoardTower.Common.Application;
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
    public sealed class LotRelicUseCaseTests
    {
        private LotRelicUseCase _useCase;
        private LotRelicEntity _lotRelicEntity;
        private PickRelicEntity _pickRelicEntity;
        private FakeAsyncPublisher<LotRelicTransitionVO> _publisher;
        private FakeAsyncSubscriber<LotRelicTransitionVO> _subscriber;
        private LotRelicPorts _ports;

        [SetUp]
        public void SetUp()
        {
            _lotRelicEntity = new LotRelicEntity();
            _pickRelicEntity = new PickRelicEntity();
            _publisher = new FakeAsyncPublisher<LotRelicTransitionVO>();
            _subscriber = new FakeAsyncSubscriber<LotRelicTransitionVO>();
            _ports = new LotRelicPorts(_subscriber, _publisher);

            // LocaleRepository は UnityEngine.Localization 依存のため null を渡す
            // RelicRepository は MasterMemory 依存のため null を渡す
            // → Lot() は両リポジトリが sealed かつ非 virtual のためユニットテスト不可
            _useCase = new LotRelicUseCase(_lotRelicEntity, _pickRelicEntity, _ports, null, null);
        }

        [TearDown]
        public void TearDown()
        {
            ((IDisposable)_useCase).Dispose();
        }

        [Test]
        public void Transition_Property_ReturnsLotRelicTransitionSubscriber()
        {
            Assert.That(_useCase.transition, Is.EqualTo(_subscriber));
        }

        [Test]
        public void LotRelic_Observable_IsNotNull()
        {
            Assert.That(_useCase.lotRelic, Is.Not.Null);
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
        public async Task FadeAsync_SetsFadeDurationFromConfig()
        {
            await _useCase.FadeAsync(Fade.In, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.transition.duration, Is.EqualTo(RelicConfig.LOT_FADE_DURATION));
        }

        [Test]
        public void Dispose_DoesNotThrow()
        {
            Assert.That(() => ((IDisposable)_useCase).Dispose(), Throws.Nothing);
        }
    }
}
