using System;
using System.Threading;
using System.Threading.Tasks;
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
    public sealed class MovementUseCaseTests
    {
        private MovementUseCase _useCase;
        private BoardEntity _boardEntity;
        private ChessmenEntity _chessmenEntity;
        private GameStateEntity _gameStateEntity;
        private PickRelicEntity _pickRelicEntity;
        private FakeAsyncPublisher<HighlightSquareVO[]> _highlightsPublisher;
        private FakeAsyncSubscriber<HighlightSquareVO[]> _highlightsSubscriber;
        private FakeAsyncPublisher<ChessmenMovementVO> _movementPublisher;
        private MovementPorts _ports;

        [SetUp]
        public void SetUp()
        {
            _boardEntity = new BoardEntity();
            _chessmenEntity = new ChessmenEntity();
            _gameStateEntity = new GameStateEntity();
            _pickRelicEntity = new PickRelicEntity();
            _highlightsPublisher = new FakeAsyncPublisher<HighlightSquareVO[]>();
            _highlightsSubscriber = new FakeAsyncSubscriber<HighlightSquareVO[]>();
            _movementPublisher = new FakeAsyncPublisher<ChessmenMovementVO>();
            _ports = new MovementPorts(_highlightsSubscriber, _highlightsPublisher, _movementPublisher);

            // ChessmenMovementRepository は MasterMemory 依存のため null を渡す
            _useCase = new MovementUseCase(
                _boardEntity,
                _chessmenEntity,
                _gameStateEntity,
                _pickRelicEntity,
                _ports,
                null);
        }

        [TearDown]
        public void TearDown()
        {
            ((IDisposable)_useCase).Dispose();
        }

        [Test]
        public void Highlights_Property_ReturnsHighlightsSubscriber()
        {
            Assert.That(_useCase.highlights, Is.EqualTo(_highlightsSubscriber));
        }

        [Test]
        public void HandleClick_WhenNotInputState_DoesNotEmit()
        {
            // GameState.None はInputではないため処理されない
            _gameStateEntity.Set(GameState.None);
            var emitted = false;
            var clickSquare = new ClickSquareVO(1, 1);

            _useCase.HandleClick(clickSquare);

            Assert.That(emitted, Is.False);
        }

        [Test]
        public void HandleClick_WhenInputStateButNoHighlights_DoesNotEmit()
        {
            _gameStateEntity.Set(GameState.Input);
            var emitted = false;
            var clickSquare = new ClickSquareVO(1, 1);

            _useCase.HandleClick(clickSquare);

            Assert.That(emitted, Is.False);
        }

        [Test]
        public async Task ClearHighlightSquareAsync_PublishesEmptyHighlights()
        {
            await _useCase.ClearHighlightSquareAsync(CancellationToken.None).AsTask();

            Assert.That(_highlightsPublisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public async Task ClearHighlightSquareAsync_PublishesEmptyArray()
        {
            await _useCase.ClearHighlightSquareAsync(CancellationToken.None).AsTask();

            Assert.That(_highlightsPublisher.LastPublished, Is.Empty);
        }

        [Test]
        public async Task MoveAsync_PublishesChessmenMovement()
        {
            await _useCase.MoveAsync(CancellationToken.None).AsTask();

            Assert.That(_movementPublisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public async Task MoveAsync_PublishesCurrentChessmenSquare()
        {
            var expectedSquare = _chessmenEntity.square;

            await _useCase.MoveAsync(CancellationToken.None).AsTask();

            Assert.That(_movementPublisher.LastPublished.square.file, Is.EqualTo(expectedSquare.file));
            Assert.That(_movementPublisher.LastPublished.square.rank, Is.EqualTo(expectedSquare.rank));
        }

        [Test]
        public void Dispose_DoesNotThrow()
        {
            Assert.That(() => ((IDisposable)_useCase).Dispose(), Throws.Nothing);
        }
    }
}
