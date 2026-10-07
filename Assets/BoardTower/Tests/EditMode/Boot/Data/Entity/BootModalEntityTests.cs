using BoardTower.Boot.Application;
using BoardTower.Boot.Data.Entity;
using BoardTower.Common.Application;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Boot.Data.Entity
{
    [TestFixture]
    public sealed class BootModalEntityTests
    {
        private BootModalEntity _entity;

        [SetUp]
        public void SetUp()
        {
            _entity = new BootModalEntity();
        }

        [Test]
        public void Constructor_InitialValue_IsNull()
        {
            Assert.That(_entity.value, Is.Null);
        }

        [Test]
        public void Set_WithTransition_UpdatesValue()
        {
            var transition = BootModalTransitionVO.Create(BootModalType.Update, Fade.In, 0.25f);

            _entity.Set(transition);

            Assert.That(_entity.value, Is.SameAs(transition));
        }

        [Test]
        public void Set_CalledMultipleTimes_ReflectsLastValue()
        {
            var first = BootModalTransitionVO.Create(BootModalType.Update, Fade.In, 0.25f);
            var second = BootModalTransitionVO.Create(BootModalType.Name, Fade.Out, 0.25f);

            _entity.Set(first);
            _entity.Set(second);

            Assert.That(_entity.value, Is.SameAs(second));
        }

        [Test]
        public void IsEqual_WithSameInstance_ReturnsTrue()
        {
            var transition = BootModalTransitionVO.Create(BootModalType.Update, Fade.In, 0.25f);
            _entity.Set(transition);

            Assert.That(_entity.IsEqual(transition), Is.True);
        }

        [Test]
        public void IsEqual_WithDifferentInstance_ReturnsFalse()
        {
            var current = BootModalTransitionVO.Create(BootModalType.Update, Fade.In, 0.25f);
            var other = BootModalTransitionVO.Create(BootModalType.Update, Fade.In, 0.25f);
            _entity.Set(current);

            Assert.That(_entity.IsEqual(other), Is.False);
        }
    }
}
