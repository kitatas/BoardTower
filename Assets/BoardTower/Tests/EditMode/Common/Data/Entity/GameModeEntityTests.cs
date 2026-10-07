using BoardTower.Common.Application;
using BoardTower.Common.Data.Entity;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Common.Data.Entity
{
    /// <summary>
    /// GameModeEntity のテスト。基底の BaseEntity&lt;T&gt; (Set / IsEqual) の検証も兼ねる。
    /// </summary>
    [TestFixture]
    public sealed class GameModeEntityTests
    {
        private GameModeEntity _entity;

        [SetUp]
        public void SetUp()
        {
            _entity = new GameModeEntity();
        }

        [Test]
        public void Constructor_InitialValue_IsNull()
        {
            Assert.That(_entity.value, Is.Null);
        }

        [Test]
        public void Set_WithGameModeVO_UpdatesValue()
        {
            var vo = new GameModeVO(GameMode.Online);

            _entity.Set(vo);

            Assert.That(_entity.value, Is.SameAs(vo));
        }

        [Test]
        public void Set_CalledTwice_ReplacesWithLatestValue()
        {
            var latest = new GameModeVO(GameMode.Offline);
            _entity.Set(new GameModeVO(GameMode.Online));

            _entity.Set(latest);

            Assert.That(_entity.value.mode, Is.EqualTo(GameMode.Offline));
        }

        [Test]
        public void IsEqual_WithCurrentValue_ReturnsTrue()
        {
            var vo = new GameModeVO(GameMode.Online);
            _entity.Set(vo);

            Assert.That(_entity.IsEqual(vo), Is.True);
        }

        [Test]
        public void IsEqual_WithDifferentValue_ReturnsFalse()
        {
            _entity.Set(new GameModeVO(GameMode.Online));

            Assert.That(_entity.IsEqual(new GameModeVO(GameMode.Offline)), Is.False);
        }
    }
}
