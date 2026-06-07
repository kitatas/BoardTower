using System.Threading;
using System.Threading.Tasks;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.Ports;
using BoardTower.Tests.EditMode.TestHelpers;
using Cysharp.Threading.Tasks;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Domain.Ports
{
    [TestFixture]
    public sealed class MovementPortsTests
    {
        private MovementPorts _ports;
        private FakeAsyncPublisher<HighlightSquareVO[]> _highlightsPublisher;
        private FakeAsyncSubscriber<HighlightSquareVO[]> _highlightsSubscriber;
        private FakeAsyncPublisher<ChessmenMovementVO> _chessmenMovementPublisher;

        [SetUp]
        public void SetUp()
        {
            _highlightsPublisher = new FakeAsyncPublisher<HighlightSquareVO[]>();
            _highlightsSubscriber = new FakeAsyncSubscriber<HighlightSquareVO[]>();
            _chessmenMovementPublisher = new FakeAsyncPublisher<ChessmenMovementVO>();

            _ports = new MovementPorts(
                _highlightsSubscriber,
                _highlightsPublisher,
                _chessmenMovementPublisher);
        }

        [Test]
        public void Constructor_AssignsHighlightsSubscriber()
        {
            Assert.That(_ports.highlightsSubscriber, Is.EqualTo(_highlightsSubscriber));
        }

        [Test]
        public async Task PublishHighlightSquaresAsync_CallsPublisherOnce()
        {
            var highlights = new[]
            {
                new HighlightSquareVO(new SquareVO(1, 1), HighlightSquareType.Movable),
            };

            await _ports.PublishHighlightSquaresAsync(highlights, CancellationToken.None).AsTask();

            Assert.That(_highlightsPublisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public async Task PublishHighlightSquaresAsync_PublishesExactArray()
        {
            var highlights = new[]
            {
                new HighlightSquareVO(new SquareVO(2, 3), HighlightSquareType.Movable),
            };

            await _ports.PublishHighlightSquaresAsync(highlights, CancellationToken.None).AsTask();

            Assert.That(_highlightsPublisher.LastPublished, Is.EqualTo(highlights));
        }

        [Test]
        public async Task PublishHighlightSquaresAsync_WithEmptyArray_CallsPublisherOnce()
        {
            var highlights = System.Array.Empty<HighlightSquareVO>();

            await _ports.PublishHighlightSquaresAsync(highlights, CancellationToken.None).AsTask();

            Assert.That(_highlightsPublisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public async Task PublishHighlightSquaresAsync_CalledMultipleTimes_IncrementsCount()
        {
            var highlights = System.Array.Empty<HighlightSquareVO>();

            await _ports.PublishHighlightSquaresAsync(highlights, CancellationToken.None).AsTask();
            await _ports.PublishHighlightSquaresAsync(highlights, CancellationToken.None).AsTask();

            Assert.That(_highlightsPublisher.PublishCount, Is.EqualTo(2));
        }

        [Test]
        public async Task PublishChessmenMovementAsync_CallsPublisherOnce()
        {
            var movement = new ChessmenMovementVO(new SquareVO(4, 5));

            await _ports.PublishChessmenMovementAsync(movement, CancellationToken.None).AsTask();

            Assert.That(_chessmenMovementPublisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public async Task PublishChessmenMovementAsync_PublishesExactVO()
        {
            var movement = new ChessmenMovementVO(new SquareVO(4, 5));

            await _ports.PublishChessmenMovementAsync(movement, CancellationToken.None).AsTask();

            Assert.That(_chessmenMovementPublisher.LastPublished, Is.EqualTo(movement));
        }

        [Test]
        public async Task PublishChessmenMovementAsync_CalledMultipleTimes_IncrementsCount()
        {
            var movement = new ChessmenMovementVO(new SquareVO(3, 3));

            await _ports.PublishChessmenMovementAsync(movement, CancellationToken.None).AsTask();
            await _ports.PublishChessmenMovementAsync(movement, CancellationToken.None).AsTask();

            Assert.That(_chessmenMovementPublisher.PublishCount, Is.EqualTo(2));
        }
    }
}
