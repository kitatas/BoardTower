using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Data.Entity
{
    [TestFixture]
    public sealed class GameStateEntityTests
    {
        private GameStateEntity _entity;

        [SetUp]
        public void SetUp()
        {
            _entity = new GameStateEntity();
        }

        [Test]
        public void Constructor_InitialValue_IsNone()
        {
            Assert.That(_entity.value, Is.EqualTo(GameState.None));
        }

        [TestCase(GameState.Init)]
        [TestCase(GameState.Input)]
        [TestCase(GameState.Send)]
        public void Set_WithState_StoresState(GameState state)
        {
            _entity.Set(state);

            Assert.That(_entity.value, Is.EqualTo(state));
        }

        [Test]
        public void Set_CalledTwice_KeepsLatestState()
        {
            _entity.Set(GameState.Input);
            _entity.Set(GameState.Event);

            Assert.That(_entity.value, Is.EqualTo(GameState.Event));
        }

        [Test]
        public void IsEqual_WithStoredState_ReturnsTrue()
        {
            _entity.Set(GameState.Judge);

            Assert.That(_entity.IsEqual(GameState.Judge), Is.True);
        }

        [Test]
        public void IsEqual_WithOtherState_ReturnsFalse()
        {
            _entity.Set(GameState.Judge);

            Assert.That(_entity.IsEqual(GameState.Clear), Is.False);
        }

        [Test]
        public void IsEqual_BeforeAnySet_MatchesNone()
        {
            Assert.That(_entity.IsEqual(GameState.None), Is.True);
        }
    }
}
