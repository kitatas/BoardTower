using System;
using System.Reflection;
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
        public async Task HandleClick_WhenNotInputState_DoesNotEmit()
        {
            // GameState.None はInputではないため処理されない
            _gameStateEntity.Set(GameState.None);
            SetLastHighlights(new HighlightSquareVO(new SquareVO(1, 1), HighlightSquareType.Movable));
            using var cts = new CancellationTokenSource();
            var inputTask = _useCase.InputAsync(cts.Token);

            _useCase.HandleClick(new ClickSquareVO(1, 1));

            Assert.That(inputTask.Status, Is.EqualTo(UniTaskStatus.Pending), "Input以外の状態ではクリックが通知されないこと");
            await CancelAsync(cts, inputTask);
        }

        [Test]
        public async Task HandleClick_WhenInputStateButNoHighlights_DoesNotEmit()
        {
            _gameStateEntity.Set(GameState.Input);
            using var cts = new CancellationTokenSource();
            var inputTask = _useCase.InputAsync(cts.Token);

            _useCase.HandleClick(new ClickSquareVO(1, 1));

            Assert.That(inputTask.Status, Is.EqualTo(UniTaskStatus.Pending), "移動可能マスが未設定ならクリックが通知されないこと");
            await CancelAsync(cts, inputTask);
        }

        [Test]
        public async Task HandleClick_WhenClickedOutsideHighlights_DoesNotEmit()
        {
            _gameStateEntity.Set(GameState.Input);
            SetLastHighlights(new HighlightSquareVO(new SquareVO(1, 1), HighlightSquareType.Movable));
            using var cts = new CancellationTokenSource();
            var inputTask = _useCase.InputAsync(cts.Token);

            _useCase.HandleClick(new ClickSquareVO(2, 2));

            Assert.That(inputTask.Status, Is.EqualTo(UniTaskStatus.Pending), "移動可能範囲外のクリックが通知されないこと");
            await CancelAsync(cts, inputTask);
        }

        [Test]
        public async Task HandleClick_WhenClickedInsideHighlights_CompletesInputAndMovesChessmen()
        {
            _gameStateEntity.Set(GameState.Input);
            SetLastHighlights(new HighlightSquareVO(new SquareVO(3, 4), HighlightSquareType.Movable));
            var inputTask = _useCase.InputAsync(CancellationToken.None);

            _useCase.HandleClick(new ClickSquareVO(3, 4));
            await inputTask.AsTask();

            Assert.That(_chessmenEntity.square.file, Is.EqualTo(3), "クリックしたマスのfileに駒が移動すること");
            Assert.That(_chessmenEntity.square.rank, Is.EqualTo(4), "クリックしたマスのrankに駒が移動すること");
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

        // Repository が null のため PublishMovableSquaresAsync を使えず、移動可能マスはリフレクションで設定する
        private void SetLastHighlights(params HighlightSquareVO[] highlights)
        {
            var field = typeof(MovementUseCase)
                .GetField("_lastHighlights", BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(_useCase, highlights);
        }

        private static async Task CancelAsync(CancellationTokenSource cts, UniTask inputTask)
        {
            cts.Cancel();
            try
            {
                await inputTask.AsTask();
            }
            catch (OperationCanceledException)
            {
                // 待機を打ち切るためのキャンセルなので無視する
            }
        }
    }
}
