
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Utility;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Utility
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
    }
}