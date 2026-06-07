using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Data.Entity
{
    [TestFixture]
    public sealed class ChessmenEntityTests
    {
        private ChessmenEntity _entity;

        [SetUp]
        public void SetUp()
        {
            _entity = new ChessmenEntity();
        }

        [Test]
        public void Constructor_ChessmenType_IsDefaultType()
        {
            Assert.That(_entity.chessmenType, Is.EqualTo(ChessmenConfig.DEFAULT_TYPE));
        }

        [Test]
        public void Constructor_Square_IsMinFileAndRank()
        {
            Assert.That(_entity.square.file, Is.EqualTo(BoardConfig.MIN_FILE));
            Assert.That(_entity.square.rank, Is.EqualTo(BoardConfig.MIN_RANK));
        }

        [Test]
        public void Reset_RestoresDefaultTypeAndPosition()
        {
            _entity.Set(new SquareVO(5, 5));

            _entity.Reset();

            Assert.That(_entity.chessmenType, Is.EqualTo(ChessmenConfig.DEFAULT_TYPE));
            Assert.That(_entity.square.file, Is.EqualTo(BoardConfig.MIN_FILE));
            Assert.That(_entity.square.rank, Is.EqualTo(BoardConfig.MIN_RANK));
        }

        [Test]
        public void Set_UpdatesSquarePosition()
        {
            var newSquare = new SquareVO(4, 6);

            _entity.Set(newSquare);

            Assert.That(_entity.square.file, Is.EqualTo(4));
            Assert.That(_entity.square.rank, Is.EqualTo(6));
        }

        [Test]
        public void Set_PreservesChessmenType()
        {
            var newSquare = new SquareVO(4, 6);

            _entity.Set(newSquare);

            Assert.That(_entity.chessmenType, Is.EqualTo(ChessmenConfig.DEFAULT_TYPE));
        }

        [Test]
        public void MoveBy_UpdatesPositionByOffset()
        {
            _entity.Set(new SquareVO(3, 3));
            var offset = new ChessmenMovementOffsetVO(1, 2);

            _entity.MoveBy(offset);

            Assert.That(_entity.square.file, Is.EqualTo(4));
            Assert.That(_entity.square.rank, Is.EqualTo(5));
        }

        [Test]
        public void CalcMovementOffset_ReturnsCorrectSquare()
        {
            _entity.Set(new SquareVO(3, 3));
            var offset = new ChessmenMovementOffsetVO(2, -1);

            var result = _entity.CalcMovementOffset(offset);

            Assert.That(result.file, Is.EqualTo(5));
            Assert.That(result.rank, Is.EqualTo(2));
        }

        [Test]
        public void CalcMovementOffset_DoesNotMoveChessmen()
        {
            _entity.Set(new SquareVO(3, 3));
            var offset = new ChessmenMovementOffsetVO(2, -1);

            _entity.CalcMovementOffset(offset);

            Assert.That(_entity.square.file, Is.EqualTo(3));
            Assert.That(_entity.square.rank, Is.EqualTo(3));
        }

        [Test]
        public void Movement_ReturnsCurrentSquare()
        {
            _entity.Set(new SquareVO(5, 7));

            Assert.That(_entity.movement.square.file, Is.EqualTo(5));
            Assert.That(_entity.movement.square.rank, Is.EqualTo(7));
        }
    }
}
