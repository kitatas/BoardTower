using System.Threading;
using System.Threading.Tasks;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.Ports;
using BoardTower.Tests.EditMode.TestHelpers;
using Cysharp.Threading.Tasks;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Domain.Ports
{
    [TestFixture]
    public sealed class ChessmenPortsTests
    {
        private ChessmenPorts _ports;
        private FakeAsyncPublisher<ChessmenTransitionVO> _transitionPublisher;
        private FakeAsyncSubscriber<ChessmenTransitionVO> _transitionSubscriber;
        private FakeAsyncSubscriber<ChessmenMovementVO> _movementSubscriber;

        [SetUp]
        public void SetUp()
        {
            _transitionPublisher = new FakeAsyncPublisher<ChessmenTransitionVO>();
            _transitionSubscriber = new FakeAsyncSubscriber<ChessmenTransitionVO>();
            _movementSubscriber = new FakeAsyncSubscriber<ChessmenMovementVO>();

            _ports = new ChessmenPorts(
                _transitionSubscriber,
                _movementSubscriber,
                _transitionPublisher);
        }

        [Test]
        public void Constructor_AssignsChessmenTransitionSubscriber()
        {
            Assert.That(_ports.chessmenTransitionSubscriber, Is.EqualTo(_transitionSubscriber));
        }

        [Test]
        public void Constructor_AssignsChessmenMovementSubscriber()
        {
            Assert.That(_ports.chessmenMovementSubscriber, Is.EqualTo(_movementSubscriber));
        }

        [Test]
        public async Task PublishChessmenTransitionAsync_CallsPublisherOnce()
        {
            var square = new SquareVO(1, 1);
            var vo = ChessmenTransitionVO.Create(Fade.Out, square);

            await _ports.PublishChessmenTransitionAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_transitionPublisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public async Task PublishChessmenTransitionAsync_PublishesExactVO()
        {
            var square = new SquareVO(3, 5);
            var vo = ChessmenTransitionVO.Create(Fade.In, square);

            await _ports.PublishChessmenTransitionAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_transitionPublisher.LastPublished, Is.EqualTo(vo));
        }

        [TestCase(Fade.In)]
        [TestCase(Fade.Out)]
        [TestCase(Fade.InOut)]
        public async Task PublishChessmenTransitionAsync_PublishesCorrectFade(Fade fade)
        {
            var square = new SquareVO(1, 1);
            var vo = ChessmenTransitionVO.Create(fade, square);

            await _ports.PublishChessmenTransitionAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_transitionPublisher.LastPublished.transition.fade, Is.EqualTo(fade));
        }

        [Test]
        public async Task PublishChessmenTransitionAsync_CalledMultipleTimes_IncrementsCount()
        {
            var square = new SquareVO(1, 1);
            var vo = ChessmenTransitionVO.Create(Fade.Out, square);

            await _ports.PublishChessmenTransitionAsync(vo, CancellationToken.None).AsTask();
            await _ports.PublishChessmenTransitionAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_transitionPublisher.PublishCount, Is.EqualTo(2));
        }
    }
}
