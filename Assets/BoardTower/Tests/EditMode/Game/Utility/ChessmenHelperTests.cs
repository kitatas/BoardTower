using System;
using BoardTower.Game.Application;
using BoardTower.Game.Utility;
using FastEnumUtility;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Utility
{
    [TestFixture]
    public sealed class ChessmenHelperTests
    {
        // -----------------------------------------------------------------------
        // GetMovableSquares
        // -----------------------------------------------------------------------

        [Test]
        public void GetMovableSquares_WithLeaperMovement_ShouldReturnExactlyOneStepPerOffset()
        {
            // Arrange
            var origin = new SquareVO(4, 4);
            var rule = new ChessmenMovementRuleVO(
                ChessmenType.Knight.ToInt32(),
                ChessmenMovementType.Leaper.ToInt32(),
                new[] { (2, 1) }
            );

            // Act
            var result = ChessmenHelper.GetMovableSquares(origin, rule);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1), "Leaper は1オフセットにつき1マスのみ返すべき");
            Assert.That(result[0].file, Is.EqualTo(6));
            Assert.That(result[0].rank, Is.EqualTo(5));
        }

        [Test]
        public void GetMovableSquares_WithSliderMovement_ShouldReturnAllSquaresUntilBoardEdge()
        {
            // Arrange: (4,4) から右方向にスライド → file 5,6,7,8 の4マス
            var origin = new SquareVO(4, 4);
            var rule = new ChessmenMovementRuleVO(
                ChessmenType.Rook.ToInt32(),
                ChessmenMovementType.Slider.ToInt32(),
                new[] { (1, 0) }
            );

            // Act
            var result = ChessmenHelper.GetMovableSquares(origin, rule);

            // Assert
            Assert.That(result.Count, Is.EqualTo(4), "Slider は盤端まですべてのマスを返すべき");
        }

        [Test]
        public void GetMovableSquares_WithSliderFromMinBoundary_ShouldReturn7Squares()
        {
            // Arrange: file=1 から右方向にスライド → file 2..8 の7マス
            var origin = new SquareVO(BoardConfig.MIN_FILE, 4);
            var rule = new ChessmenMovementRuleVO(
                ChessmenType.Rook.ToInt32(),
                ChessmenMovementType.Slider.ToInt32(),
                new[] { (1, 0) }
            );

            // Act
            var result = ChessmenHelper.GetMovableSquares(origin, rule);

            // Assert
            Assert.That(result.Count, Is.EqualTo(7));
        }

        [Test]
        public void GetMovableSquares_WithOffsetLeadingImmediatelyOffBoard_ShouldReturnEmptyList()
        {
            // Arrange: (1,1) から左方向 → 即座に盤外
            var origin = new SquareVO(1, 1);
            var rule = new ChessmenMovementRuleVO(
                ChessmenType.Knight.ToInt32(),
                ChessmenMovementType.Leaper.ToInt32(),
                new[] { (-1, 0) }
            );

            // Act
            var result = ChessmenHelper.GetMovableSquares(origin, rule);

            // Assert
            Assert.That(result.Count, Is.EqualTo(0), "盤外に出るオフセットのマスは含まれるべきでない");
        }

        [Test]
        public void GetMovableSquares_WithOriginAtMaxBoundaryAndOutwardOffset_ShouldReturnEmptyList()
        {
            // Arrange: (8,8) から右・上方向 → 両方とも即座に盤外
            var origin = new SquareVO(BoardConfig.MAX_FILE, BoardConfig.MAX_RANK);
            var rule = new ChessmenMovementRuleVO(
                ChessmenType.Rook.ToInt32(),
                ChessmenMovementType.Slider.ToInt32(),
                new[] { (1, 0), (0, 1) }
            );

            // Act
            var result = ChessmenHelper.GetMovableSquares(origin, rule);

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetMovableSquares_WithMultipleLeaperOffsets_ShouldReturnOneSquarePerValidOffset()
        {
            // Arrange: (4,4) から上下左右 4方向すべて盤内
            var origin = new SquareVO(4, 4);
            var rule = new ChessmenMovementRuleVO(
                ChessmenType.Knight.ToInt32(),
                ChessmenMovementType.Leaper.ToInt32(),
                new[] { (1, 0), (-1, 0), (0, 1), (0, -1) }
            );

            // Act
            var result = ChessmenHelper.GetMovableSquares(origin, rule);

            // Assert
            Assert.That(result.Count, Is.EqualTo(4), "有効なオフセットの数だけ1マスずつ返すべき");
        }

        [Test]
        public void GetMovableSquares_WithEmptyOffsets_ShouldReturnEmptyList()
        {
            // Arrange
            var origin = new SquareVO(4, 4);
            var rule = new ChessmenMovementRuleVO(
                ChessmenType.Knight.ToInt32(),
                ChessmenMovementType.Leaper.ToInt32(), 
                Array.Empty<(int, int)>()
            );

            // Act
            var result = ChessmenHelper.GetMovableSquares(origin, rule);

            // Assert
            Assert.That(result.Count, Is.EqualTo(0), "オフセットが空の場合は空リストを返すべき");
        }

        [Test]
        public void GetMovableSquares_WithLeaperAtCorner_ShouldExcludeOutOfBoardMoves()
        {
            // Arrange: (1,1) からナイト8方向 → 盤内は (3,2) と (2,3) の2マスのみ
            var origin = new SquareVO(1, 1);
            var rule = new ChessmenMovementRuleVO(
                ChessmenType.Knight.ToInt32(),
                ChessmenMovementType.Leaper.ToInt32(),
                new[] { (2, 1), (-2, 1), (2, -1), (-2, -1), (1, 2), (-1, 2), (1, -2), (-1, -2) }
            );

            // Act
            var result = ChessmenHelper.GetMovableSquares(origin, rule);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2), "角のナイトは2マスのみ移動できるべき");
        }

        [Test]
        public void GetMovableSquares_WithSliderMovement_ShouldReturnSquaresInOrder()
        {
            // Arrange: (4,4) から右方向スライド → file 5,6,7,8 の順で返すべき
            var origin = new SquareVO(4, 4);
            var rule = new ChessmenMovementRuleVO(
                ChessmenType.Rook.ToInt32(),
                ChessmenMovementType.Slider.ToInt32(),
                new[] { (1, 0) }
            );

            // Act
            var result = ChessmenHelper.GetMovableSquares(origin, rule);

            // Assert
            Assert.That(result[0].file, Is.EqualTo(5));
            Assert.That(result[1].file, Is.EqualTo(6));
            Assert.That(result[2].file, Is.EqualTo(7));
            Assert.That(result[3].file, Is.EqualTo(8));
        }

        // -----------------------------------------------------------------------
        // CalcSquare
        // -----------------------------------------------------------------------

        [Test]
        public void CalcSquare_WithPositiveOffset_ShouldReturnCorrectSquare()
        {
            // Arrange
            var square = new SquareVO(3, 3);
            var offset = new ChessmenMovementOffsetVO(2, 1);

            // Act
            var result = ChessmenHelper.CalcSquare(square, offset);

            // Assert
            Assert.That(result.file, Is.EqualTo(5));
            Assert.That(result.rank, Is.EqualTo(4));
        }

        [Test]
        public void CalcSquare_WithZeroOffset_ShouldReturnSamePosition()
        {
            // Arrange
            var square = new SquareVO(4, 5);
            var offset = new ChessmenMovementOffsetVO(0, 0);

            // Act
            var result = ChessmenHelper.CalcSquare(square, offset);

            // Assert
            Assert.That(result.file, Is.EqualTo(4));
            Assert.That(result.rank, Is.EqualTo(5));
        }

        [Test]
        public void CalcSquare_WithNegativeOffset_ShouldReturnCorrectSquare()
        {
            // Arrange
            var square = new SquareVO(5, 5);
            var offset = new ChessmenMovementOffsetVO(-2, -3);

            // Act
            var result = ChessmenHelper.CalcSquare(square, offset);

            // Assert
            Assert.That(result.file, Is.EqualTo(3));
            Assert.That(result.rank, Is.EqualTo(2));
        }

        [Test]
        public void CalcSquare_WithMaxBoundarySquare_ShouldReturnSquareWithAddedOffset()
        {
            // Arrange
            var square = new SquareVO(BoardConfig.MAX_FILE, BoardConfig.MAX_RANK);
            var offset = new ChessmenMovementOffsetVO(-1, -1);

            // Act
            var result = ChessmenHelper.CalcSquare(square, offset);

            // Assert
            Assert.That(result.file, Is.EqualTo(BoardConfig.MAX_FILE - 1));
            Assert.That(result.rank, Is.EqualTo(BoardConfig.MAX_RANK - 1));
        }
    }
}