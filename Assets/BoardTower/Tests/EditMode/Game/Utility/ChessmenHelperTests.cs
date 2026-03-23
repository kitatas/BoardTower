using BoardTower.Game.Application;
using BoardTower.Game.Utility;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Utility
{
    public class ChessmenHelperTests
    {
        [Test]
        public void CalcSquare_正の値のオフセット_正しく計算される()
        {
            // Arrange
            var square = new SquareVO(3, 4);
            var offset = new ChessmenMovementOffsetVO(2, 1);

            // Act
            var result = ChessmenHelper.CalcSquare(square, offset);

            // Assert
            Assert.That(result.file, Is.EqualTo(5));
            Assert.That(result.rank, Is.EqualTo(5));
        }

        [Test]
        public void CalcSquare_負の値のオフセット_正しく計算される()
        {
            // Arrange
            var square = new SquareVO(5, 6);
            var offset = new ChessmenMovementOffsetVO(-2, -3);

            // Act
            var result = ChessmenHelper.CalcSquare(square, offset);

            // Assert
            Assert.That(result.file, Is.EqualTo(3));
            Assert.That(result.rank, Is.EqualTo(3));
        }

        [Test]
        public void CalcSquare_ゼロのオフセット_元の座標と同じ()
        {
            // Arrange
            var square = new SquareVO(4, 7);
            var offset = new ChessmenMovementOffsetVO(0, 0);

            // Act
            var result = ChessmenHelper.CalcSquare(square, offset);

            // Assert
            Assert.That(result.file, Is.EqualTo(4));
            Assert.That(result.rank, Is.EqualTo(7));
        }

        [Test]
        public void CalcSquare_混合オフセット_正しく計算される()
        {
            // Arrange
            var square = new SquareVO(2, 8);
            var offset = new ChessmenMovementOffsetVO(3, -5);

            // Act
            var result = ChessmenHelper.CalcSquare(square, offset);

            // Assert
            Assert.That(result.file, Is.EqualTo(5));
            Assert.That(result.rank, Is.EqualTo(3));
        }

        [Test]
        public void CalcSquare_境界値での計算_正しく処理される()
        {
            // Arrange
            var square = new SquareVO(1, 1);
            var offset = new ChessmenMovementOffsetVO(7, 7);

            // Act
            var result = ChessmenHelper.CalcSquare(square, offset);

            // Assert
            Assert.That(result.file, Is.EqualTo(8));
            Assert.That(result.rank, Is.EqualTo(8));
        }

        [Test]
        public void CalcSquare_大きなオフセット値_正しく計算される()
        {
            // Arrange
            var square = new SquareVO(10, 15);
            var offset = new ChessmenMovementOffsetVO(-8, -12);

            // Act
            var result = ChessmenHelper.CalcSquare(square, offset);

            // Assert
            Assert.That(result.file, Is.EqualTo(2));
            Assert.That(result.rank, Is.EqualTo(3));
        }

        [TestCase(1, 1, 1, 1, 2, 2)]
        [TestCase(5, 5, -2, -2, 3, 3)]
        [TestCase(4, 3, 0, 4, 4, 7)]
        [TestCase(8, 8, -7, -7, 1, 1)]
        public void CalcSquare_パラメータ化テスト_正しく計算される(
            int originalFile, int originalRank,
            int dx, int dy,
            int expectedFile, int expectedRank)
        {
            // Arrange
            var square = new SquareVO(originalFile, originalRank);
            var offset = new ChessmenMovementOffsetVO(dx, dy);

            // Act
            var result = ChessmenHelper.CalcSquare(square, offset);

            // Assert
            Assert.That(result.file, Is.EqualTo(expectedFile));
            Assert.That(result.rank, Is.EqualTo(expectedRank));
        }
    }
}