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
    public sealed class BoardUseCaseTests
    {
        private BoardUseCase _useCase;
        private BoardEntity _boardEntity;
        private FakeAsyncPublisher<BoardTransitionVO> _boardTransitionPublisher;
        private FakeAsyncSubscriber<BoardTransitionVO> _boardTransitionSubscriber;
        private FakeAsyncPublisher<RenderEventSquareVO> _renderEventSquarePublisher;
        private FakeAsyncSubscriber<RenderEventSquareVO> _renderEventSquareSubscriber;
        private BoardPorts _ports;

        [SetUp]
        public void SetUp()
        {
            _boardEntity = new BoardEntity();
            _boardTransitionPublisher = new FakeAsyncPublisher<BoardTransitionVO>();
            _boardTransitionSubscriber = new FakeAsyncSubscriber<BoardTransitionVO>();
            _renderEventSquarePublisher = new FakeAsyncPublisher<RenderEventSquareVO>();
            _renderEventSquareSubscriber = new FakeAsyncSubscriber<RenderEventSquareVO>();
            _ports = new BoardPorts(
                _boardTransitionSubscriber,
                _renderEventSquareSubscriber,
                _boardTransitionPublisher,
                _renderEventSquarePublisher);

            // BoardPatternRepository / SquareEventRepository は MasterMemory 依存のため null を渡す
            _useCase = new BoardUseCase(_boardEntity, _ports, null, null);
        }

        [Test]
        public void Transition_Property_ReturnsBoardTransitionSubscriber()
        {
            Assert.That(_useCase.transition, Is.EqualTo(_boardTransitionSubscriber));
        }

        [Test]
        public void RenderEvent_Property_ReturnsRenderEventSquareSubscriber()
        {
            Assert.That(_useCase.renderEvent, Is.EqualTo(_renderEventSquareSubscriber));
        }

        [Test]
        public async Task FadeAsync_PublishesOnce()
        {
            await _useCase.FadeAsync(Fade.Out, CancellationToken.None).AsTask();

            Assert.That(_boardTransitionPublisher.PublishCount, Is.EqualTo(1));
        }

        [TestCase(Fade.In)]
        [TestCase(Fade.Out)]
        [TestCase(Fade.InOut)]
        public async Task FadeAsync_PublishesCorrectFade(Fade fade)
        {
            await _useCase.FadeAsync(fade, CancellationToken.None).AsTask();

            Assert.That(_boardTransitionPublisher.LastPublished.transition.fade, Is.EqualTo(fade));
        }

        [Test]
        public async Task FadeAsync_SetsFadeDurationFromConfig()
        {
            await _useCase.FadeAsync(Fade.Out, CancellationToken.None).AsTask();

            Assert.That(_boardTransitionPublisher.LastPublished.transition.duration, Is.EqualTo(BoardConfig.FADE_DURATION));
        }

        [Test]
        public async Task FadeAsync_CalledMultipleTimes_IncrementsCount()
        {
            await _useCase.FadeAsync(Fade.Out, CancellationToken.None).AsTask();
            await _useCase.FadeAsync(Fade.In, CancellationToken.None).AsTask();

            Assert.That(_boardTransitionPublisher.PublishCount, Is.EqualTo(2));
        }
    }
}
