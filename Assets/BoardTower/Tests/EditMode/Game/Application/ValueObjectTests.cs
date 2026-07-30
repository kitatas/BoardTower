using BoardTower.Common.Application;
using BoardTower.Game.Application;
using FastEnumUtility;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Application
{
    [TestFixture]
    public sealed class ValueObjectTests
    {

        // ---- AchievementVO ----

        [Test]
        public void AchievementVO_Constructor_AssignsTypeRankAndValue()
        {
            var sut = new AchievementVO(AchievementType.Play.ToInt32(), AchievementRankType.Normal.ToInt32(), 5);

            Assert.That(sut.type, Is.EqualTo(AchievementType.Play));
            Assert.That(sut.rank, Is.EqualTo(AchievementRankType.Normal));
            Assert.That(sut.value, Is.EqualTo(5));
        }

        // ---- ProgressVO ----

        [Test]
        public void ProgressVO_Constructor_AssignsTypeAndValue()
        {
            var sut = new ProgressVO(AchievementType.Score.ToInt32(), 10);

            Assert.That(sut.type, Is.EqualTo(AchievementType.Score.ToInt32()));
            Assert.That(sut.value, Is.EqualTo(10));
        }

        [TestCase(AchievementType.None, 0)]
        [TestCase(AchievementType.Play, 1)]
        [TestCase(AchievementType.Clear, 999)]
        public void ProgressVO_Constructor_WithVariousValues_AssignsCorrectly(AchievementType type, int value)
        {
            var sut = new ProgressVO(type.ToInt32(), value);

            Assert.That(sut.type, Is.EqualTo(type.ToInt32()));
            Assert.That(sut.value, Is.EqualTo(value));
        }

        // ---- AchievementContentVO ----

        [Test]
        public void AchievementContentVO_Constructor_AssignsAchievementIsAchieveAndContent()
        {
            var achievement = new AchievementVO(AchievementType.Play.ToInt32(), AchievementRankType.Normal.ToInt32(), 5);

            var sut = new AchievementContentVO(achievement, true, "5回プレイする");

            Assert.That(sut.achievement, Is.EqualTo(achievement));
            Assert.That(sut.isAchieve, Is.True);
            Assert.That(sut.content, Is.EqualTo("5回プレイする"));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void AchievementContentVO_Constructor_WithIsAchieveVariants_SetsCorrectly(bool isAchieve)
        {
            var achievement = new AchievementVO(AchievementType.Score.ToInt32(), AchievementRankType.Gold.ToInt32(), 100);

            var sut = new AchievementContentVO(achievement, isAchieve, "content");

            Assert.That(sut.isAchieve, Is.EqualTo(isAchieve));
        }

        // AchievementVO は参照型フィールドのため、同一インスタンスが保持されることを検証する
        [Test]
        public void AchievementContentVO_Constructor_KeepsSameAchievementReference()
        {
            var achievement = new AchievementVO(AchievementType.Clear.ToInt32(), AchievementRankType.Platinum.ToInt32(),
                999);

            var sut = new AchievementContentVO(achievement, false, "content");

            Assert.That(sut.achievement, Is.SameAs(achievement));
        }

        // バリデーションを持たない VO のため、空文字・null もそのまま保持される
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void AchievementContentVO_Constructor_WithEmptyOrNullContent_AssignsAsIs(string content)
        {
            var achievement = new AchievementVO(AchievementType.None.ToInt32(), AchievementRankType.None.ToInt32(), 0);

            var sut = new AchievementContentVO(achievement, false, content);

            Assert.That(sut.content, Is.EqualTo(content));
        }

        [Test]
        public void AchievementContentVO_Constructor_WithNullAchievement_AssignsNull()
        {
            var sut = new AchievementContentVO(null, false, "content");

            Assert.That(sut.achievement, Is.Null);
        }

        [TestCase(AchievementType.None, AchievementRankType.None, 0)]
        [TestCase(AchievementType.Play, AchievementRankType.Bronze, 1)]
        [TestCase(AchievementType.Score, AchievementRankType.Silver, 100)]
        [TestCase(AchievementType.Clear, AchievementRankType.Platinum, 999)]
        public void AchievementContentVO_Constructor_WithVariousAchievements_AssignsCorrectly(AchievementType type,
            AchievementRankType rank, int value)
        {
            var achievement = new AchievementVO(type.ToInt32(), rank.ToInt32(), value);

            var sut = new AchievementContentVO(achievement, true, "content");

            Assert.That(sut.achievement.type, Is.EqualTo(type));
            Assert.That(sut.achievement.rank, Is.EqualTo(rank));
            Assert.That(sut.achievement.value, Is.EqualTo(value));
        }
    }
}