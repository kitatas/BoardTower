using System;
using System.Collections.Generic;
using System.Linq;
using BoardTower.Game.Application;
using BoardTower.Game.Utility;
using FastEnumUtility;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Utility
{
    public class ChessmenHelperTests
    {

        [Test, TestCaseSource(nameof(GetChessmenMovementRules))]
        public void GetMovableSquares_FromCenter_SatisfiesRuleProperties(ChessmenMovementRuleVO rule)
        {
            var origin = new SquareVO(
                (BoardConfig.MIN_FILE + BoardConfig.MAX_FILE) / 2,
                (BoardConfig.MIN_RANK + BoardConfig.MAX_RANK) / 2);

            var squares = ChessmenHelper.GetMovableSquares(origin, rule);

            AssertSquaresSatisfyRule(origin, rule, squares);
        }

        [Test, TestCaseSource(nameof(GetChessmenMovementRules))]
        public void GetMovableSquares_FromBottomLeftCorner_SatisfiesRuleProperties(ChessmenMovementRuleVO rule)
        {
            var origin = new SquareVO(BoardConfig.MIN_FILE, BoardConfig.MIN_RANK);

            var squares = ChessmenHelper.GetMovableSquares(origin, rule);

            AssertSquaresSatisfyRule(origin, rule, squares);
        }

        private static IEnumerable<ChessmenMovementRuleVO> GetChessmenMovementRules()
        {
            return new[]
            {
                new ChessmenMovementRuleVO(ChessmenType.King.ToInt32(), ChessmenMovementType.Leaper.ToInt32(),
                    new[]
                    {
                        (dx: -1, dy: -1),
                        (dx: -1, dy: 0),
                        (dx: -1, dy: 1),
                        (dx: 0, dy: -1),
                        (dx: 0, dy: 1),
                        (dx: 1, dy: -1),
                        (dx: 1, dy: 0),
                        (dx: 1, dy: 1),
                    }),
                new ChessmenMovementRuleVO(ChessmenType.Queen.ToInt32(), ChessmenMovementType.Slider.ToInt32(),
                    new[]
                    {
                        (dx: -1, dy: -1),
                        (dx: -1, dy: 0),
                        (dx: -1, dy: 1),
                        (dx: 0, dy: -1),
                        (dx: 0, dy: 1),
                        (dx: 1, dy: -1),
                        (dx: 1, dy: 0),
                        (dx: 1, dy: 1),
                    }),
                new ChessmenMovementRuleVO(ChessmenType.Rook.ToInt32(), ChessmenMovementType.Slider.ToInt32(),
                    new[]
                    {
                        (dx: -1, dy: 0),
                        (dx: 1, dy: 0),
                        (dx: 0, dy: -1),
                        (dx: 0, dy: 1),
                    }),
                new ChessmenMovementRuleVO(ChessmenType.Bishop.ToInt32(), ChessmenMovementType.Slider.ToInt32(),
                    new[]
                    {
                        (dx: -1, dy: -1),
                        (dx: -1, dy: 1),
                        (dx: 1, dy: -1),
                        (dx: 1, dy: 1),
                    }),
                new ChessmenMovementRuleVO(ChessmenType.Knight.ToInt32(), ChessmenMovementType.Leaper.ToInt32(),
                    new[]
                    {
                        (dx: -2, dy: -1),
                        (dx: -2, dy: 1),
                        (dx: -1, dy: -2),
                        (dx: -1, dy: 2),
                        (dx: 1, dy: -2),
                        (dx: 1, dy: 2),
                        (dx: 2, dy: -1),
                        (dx: 2, dy: 1),
                    }),
            };
        }

        // 期待性質の検証
        private static void AssertSquaresSatisfyRule(SquareVO origin, ChessmenMovementRuleVO rule,
            IList<SquareVO> squares)
        {
            // 1) 盤外なし・重複なし
            Assert.That(squares.All(s => !BoardHelper.IsOutOfBoard(s.file, s.rank)), Is.True,
                "contains out-of-board square");
            var uniq = squares
                .Select(s => (s.file, s.rank))
                .ToHashSet();
            Assert.That(uniq.Count, Is.EqualTo(squares.Count), "contains duplicate squares");

            if (rule.movement == ChessmenMovementType.Leaper)
            {
                // 2-L) 1手先のみ＆オフセット一致、件数は盤内に収まるオフセット数
                var expected = rule.offsets
                    .Select(o => (origin.file + o.dx, origin.rank + o.dy))
                    .Where(p => !BoardHelper.IsOutOfBoard(p.Item1, p.Item2))
                    .ToHashSet();

                Assert.That(uniq, Is.EquivalentTo(expected), "Leaper destinations mismatch");
            }
            else
            {
                // 2-S) 件数は各方向の端までの歩数の総和
                var totalSteps = rule.offsets.Sum(o => StepsToEdge(origin.file, origin.rank, o.dx, o.dy));
                Assert.That(squares.Count, Is.EqualTo(totalSteps), "Slider total steps mismatch");

                // 3-S) 返却マスは必ずいずれかのレイ上にあり、各レイは 1..N の連続ステップが揃う
                var matched = new HashSet<(int, int)>();
                foreach (var o in rule.offsets)
                {
                    var limit = StepsToEdge(origin.file, origin.rank, o.dx, o.dy);
                    var steps = new HashSet<int>();

                    foreach (var s in squares)
                    {
                        if (TryGetStepOnRay(origin, s, o.dx, o.dy, out var step))
                        {
                            steps.Add(step);
                            matched.Add((s.file, s.rank));
                        }
                    }

                    // この方向のマスは 1..limit が全て揃い、件数も一致
                    Assert.That(steps.Count, Is.EqualTo(limit), $"ray ({o.dx},{o.dy}) count mismatch");
                    for (int t = 1; t <= limit; t++)
                        Assert.That(steps.Contains(t), Is.True, $"ray ({o.dx},{o.dy}) missing step {t}");
                }

                // 4-S) すべての返却マスはどれかのレイに該当している
                Assert.That(matched.Count, Is.EqualTo(squares.Count), "contains squares not on any allowed ray");
            }
        }

        private static bool TryGetStepOnRay(SquareVO origin, SquareVO s, int dx, int dy, out int step)
        {
            step = 0;
            var df = s.file - origin.file;
            var dr = s.rank - origin.rank;

            if (dx == 0 && dy == 0) return false;

            if (dx == 0)
            {
                if (df != 0) return false;
                if (dy == 0) return false;
                if (dr % dy != 0) return false;
                step = dr / dy;
                return step > 0;
            }

            if (dy == 0)
            {
                if (dr != 0) return false;
                if (df % dx != 0) return false;
                step = df / dx;
                return step > 0;
            }

            // 斜め・任意方向
            if (df % dx != 0) return false;
            step = df / dx;
            if (step <= 0) return false;
            return dr == dy * step;
        }

        private static int StepsToEdge(int f, int r, int dx, int dy)
        {
            int stepsX = dx > 0 ? BoardConfig.MAX_FILE - f
                : dx < 0 ? f - BoardConfig.MIN_FILE
                : int.MaxValue;

            int stepsY = dy > 0 ? BoardConfig.MAX_RANK - r
                : dy < 0 ? r - BoardConfig.MIN_RANK
                : int.MaxValue;

            return Math.Min(stepsX, stepsY);
        }

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