using System;
using System.Collections.Generic;
using System.Linq;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Utility;
using FastEnumUtility;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Utility
{
    [TestFixture]
    public sealed class BoardHelperTests
    {
        private static int files => BoardConfig.MAX_FILE - BoardConfig.MIN_FILE + 1;
        private static int ranks => BoardConfig.MAX_RANK - BoardConfig.MIN_RANK + 1;
        private static int maxIndex => files * ranks;

        [Test]
        public void ToFileRank_Index0_Returns_MinMin()
        {
            var (file, rank) = BoardHelper.ToFileRank(0);
            Assert.That(file, Is.EqualTo(BoardConfig.MIN_FILE));
            Assert.That(rank, Is.EqualTo(BoardConfig.MIN_RANK));
        }

        [Test]
        public void ToFileRank_LastIndex_Returns_MaxMax()
        {
            var last = maxIndex - 1;
            var (file, rank) = BoardHelper.ToFileRank(last);
            Assert.That(file, Is.EqualTo(BoardConfig.MAX_FILE));
            Assert.That(rank, Is.EqualTo(BoardConfig.MAX_RANK));
        }

        [Test]
        public void ToFileRank_FirstFile_AllRanks_MapCorrectly()
        {
            // 0..(Ranks-1) は常に MIN_FILE、rank は MIN_RANK..MAX_RANK
            for (int i = 0; i < ranks; i++)
            {
                var (file, rank) = BoardHelper.ToFileRank(i);
                Assert.That(file, Is.EqualTo(BoardConfig.MIN_FILE), $"file mismatch at index={i}");
                Assert.That(rank, Is.EqualTo(BoardConfig.MIN_RANK + i), $"rank mismatch at index={i}");
            }
        }

        [Test]
        public void ToFileRank_SecondFile_FirstIndex_IsMinRank()
        {
            var (file, rank) = BoardHelper.ToFileRank(ranks);
            Assert.That(file, Is.EqualTo(BoardConfig.MIN_FILE + 1));
            Assert.That(rank, Is.EqualTo(BoardConfig.MIN_RANK));
        }

        [Test]
        public void ToFileRank_OutOfRange_ThrowsQuitExceptionVO()
        {
            Assert.That(() => BoardHelper.ToFileRank(-1), Throws.TypeOf<QuitExceptionVO>());
            Assert.That(() => BoardHelper.ToFileRank(maxIndex), Throws.TypeOf<QuitExceptionVO>());
        }

        [Test]
        public void ToIndex_ZeroBased_MinAndMax()
        {
            var idxMin = BoardHelper.ToIndex(BoardConfig.MIN_FILE, BoardConfig.MIN_RANK);
            Assert.That(idxMin, Is.EqualTo(0), "ToIndex should be zero-based at minimum square");

            var idxMax = BoardHelper.ToIndex(BoardConfig.MAX_FILE, BoardConfig.MAX_RANK);
            Assert.That(idxMax, Is.EqualTo(maxIndex - 1), "ToIndex should be zero-based at maximum square");
        }

        [Test]
        public void RoundTrip_ToIndex_And_ToFileRank_ZeroBased()
        {
            for (int file = BoardConfig.MIN_FILE; file <= BoardConfig.MAX_FILE; file++)
            {
                for (int rank = BoardConfig.MIN_RANK; rank <= BoardConfig.MAX_RANK; rank++)
                {
                    var index = BoardHelper.ToIndex(file, rank); // 0-based
                    Assert.That(index, Is.InRange(0, maxIndex - 1), $"index out of range at ({file},{rank})");

                    var (f2, r2) = BoardHelper.ToFileRank(index); // 0-based -> (file,rank)
                    Assert.That(f2, Is.EqualTo(file), $"file mismatch at ({file},{rank})");
                    Assert.That(r2, Is.EqualTo(rank), $"rank mismatch at ({file},{rank})");
                }
            }
        }

        [Test]
        public void IsOutOfBoard_BoundaryAndOverflow()
        {
            // 境界内
            Assert.That(BoardHelper.IsOutOfBoard(BoardConfig.MIN_FILE, BoardConfig.MIN_RANK), Is.False);
            Assert.That(BoardHelper.IsOutOfBoard(BoardConfig.MAX_FILE, BoardConfig.MAX_RANK), Is.False);

            // file 側はみ出し
            Assert.That(BoardHelper.IsOutOfBoard(BoardConfig.MIN_FILE - 1, BoardConfig.MIN_RANK), Is.True);
            Assert.That(BoardHelper.IsOutOfBoard(BoardConfig.MAX_FILE + 1, BoardConfig.MIN_RANK), Is.True);

            // rank 側はみ出し
            Assert.That(BoardHelper.IsOutOfBoard(BoardConfig.MIN_FILE, BoardConfig.MIN_RANK - 1), Is.True);
            Assert.That(BoardHelper.IsOutOfBoard(BoardConfig.MIN_FILE, BoardConfig.MAX_RANK + 1), Is.True);
        }

        [Test, TestCaseSource(nameof(GetChessmenMovementRules))]
        public void GetMovableSquares_FromCenter_SatisfiesRuleProperties(ChessmenMovementRuleVO rule)
        {
            var origin = new SquareVO(
                (BoardConfig.MIN_FILE + BoardConfig.MAX_FILE) / 2,
                (BoardConfig.MIN_RANK + BoardConfig.MAX_RANK) / 2);

            var squares = BoardHelper.GetMovableSquares(origin, rule);

            AssertSquaresSatisfyRule(origin, rule, squares);
        }

        [Test, TestCaseSource(nameof(GetChessmenMovementRules))]
        public void GetMovableSquares_FromBottomLeftCorner_SatisfiesRuleProperties(ChessmenMovementRuleVO rule)
        {
            var origin = new SquareVO(BoardConfig.MIN_FILE, BoardConfig.MIN_RANK);

            var squares = BoardHelper.GetMovableSquares(origin, rule);

            AssertSquaresSatisfyRule(origin, rule, squares);
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
    }
}