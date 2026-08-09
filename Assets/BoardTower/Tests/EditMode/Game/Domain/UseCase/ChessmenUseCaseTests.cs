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
    public sealed class ChessmenUseCaseTests
    {
        private ChessmenUseCase _useCase;
        private ChessmenEntity _chessmenEntity;
        private FakeAsyncPublisher<ChessmenTransitionVO> _transitionPublisher;
        private FakeAsyncSubscriber<ChessmenTransitionVO> _transitionSubscriber;
        private FakeAsyncSubscriber<ChessmenMovementVO> _movementSubscriber;
        private ChessmenPorts _ports;

        [SetUp]
        public void SetUp()
        {
            _chessmenEntity = new ChessmenEntity();
            _transitionPublisher = new FakeAsyncPublisher<ChessmenTransitionVO>();
            _transitionSubscriber = new FakeAsyncSubscriber<ChessmenTransitionVO>();
            _movementSubscriber = new FakeAsyncSubscriber<ChessmenMovementVO>();
            _ports = new ChessmenPorts(_transitionSubscriber, _movementSubscriber, _transitionPublisher);
            _useCase = new ChessmenUseCase(_chessmenEntity, _ports);
        }

        [Test]
        public void Transition_Property_ReturnsChessmenTransitionSubscriber()
        {
            Assert.That(_useCase.transition, Is.EqualTo(_transitionSubscriber));
        }

        [Test]
        public void Movement_Property_ReturnsChessmenMovementSubscriber()
        {
            Assert.That(_useCase.movement, Is.EqualTo(_movementSubscriber));
        }

        [Test]
        public void Init_ResetsChessmenToDefaultSquare()
        {
            _chessmenEntity.Set(new SquareVO(5, 5));

            _useCase.Init();

            Assert.That(_chessmenEntity.square.file, Is.EqualTo(BoardConfig.MIN_FILE));
            Assert.That(_chessmenEntity.square.rank, Is.EqualTo(BoardConfig.MIN_RANK));
        }

        [Test]
        public void Init_ResetsChessmenTypeToDefault()
        {
            _useCase.Init();

            Assert.That(_chessmenEntity.chessmenType, Is.EqualTo(ChessmenConfig.DEFAULT_TYPE));
        }

        [Test]
        public async Task FadeAsync_PublishesOnce()
        {
            await _useCase.FadeAsync(Fade.Out, CancellationToken.None).AsTask();

            Assert.That(_transitionPublisher.PublishCount, Is.EqualTo(1));
        }

        [TestCase(Fade.In)]
        [TestCase(Fade.Out)]
        [TestCase(Fade.InOut)]
        public async Task FadeAsync_PublishesCorrectFade(Fade fade)
        {
            await _useCase.FadeAsync(fade, CancellationToken.None).AsTask();

            Assert.That(_transitionPublisher.LastPublished.transition.fade, Is.EqualTo(fade));
        }

        [Test]
        public async Task FadeAsync_PublishesChessmenCurrentSquare()
        {
            var expectedSquare = new SquareVO(3, 4);
            _chessmenEntity.Set(expectedSquare);

            await _useCase.FadeAsync(Fade.Out, CancellationToken.None).AsTask();

            Assert.That(_transitionPublisher.LastPublished.square.file, Is.EqualTo(expectedSquare.file));
            Assert.That(_transitionPublisher.LastPublished.square.rank, Is.EqualTo(expectedSquare.rank));
        }

        [Test]
        public async Task FadeAsync_CalledMultipleTimes_IncrementsCount()
        {
            await _useCase.FadeAsync(Fade.Out, CancellationToken.None).AsTask();
            await _useCase.FadeAsync(Fade.In, CancellationToken.None).AsTask();

            Assert.That(_transitionPublisher.PublishCount, Is.EqualTo(2));
        }
    }
}
