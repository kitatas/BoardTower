using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Data.Entity
{
    [TestFixture]
    public sealed class GameModalEntityTests
    {
        private GameModalEntity _entity;

        [SetUp]
        public void SetUp()
        {
            _entity = new GameModalEntity();
        }

        [Test]
        public void Constructor_InitialValue_IsNull()
        {
            Assert.That(_entity.value, Is.Null);
        }

        [Test]
        public void Set_WithTransition_StoresSameInstance()
        {
            var transition = GameModalTransitionVO.Create(GameModalType.Menu, Fade.In, 0.25f);

            _entity.Set(transition);

            Assert.That(_entity.value, Is.SameAs(transition));
        }

        [Test]
        public void Set_CalledTwice_KeepsLatestValue()
        {
            var first = GameModalTransitionVO.Create(GameModalType.Menu, Fade.In, 0.25f);
            var second = GameModalTransitionVO.Create(GameModalType.Sound, Fade.Out, 0.5f);

            _entity.Set(first);
            _entity.Set(second);

            Assert.That(_entity.value, Is.SameAs(second));
        }

        [Test]
        public void IsEqual_WithStoredTransition_ReturnsTrue()
        {
            var transition = GameModalTransitionVO.Create(GameModalType.Menu, Fade.In, 0.25f);
            _entity.Set(transition);

            Assert.That(_entity.IsEqual(transition), Is.True);
        }

        [Test]
        public void IsEqual_WithOtherTransition_ReturnsFalse()
        {
            _entity.Set(GameModalTransitionVO.Create(GameModalType.Menu, Fade.In, 0.25f));
            var other = GameModalTransitionVO.Create(GameModalType.Policy, Fade.In, 0.25f);

            Assert.That(_entity.IsEqual(other), Is.False);
        }
    }
}
