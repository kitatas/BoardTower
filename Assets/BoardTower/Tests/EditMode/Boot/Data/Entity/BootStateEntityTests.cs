using BoardTower.Boot.Application;
using BoardTower.Boot.Data.Entity;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Boot.Data.Entity
{
    [TestFixture]
    public sealed class BootStateEntityTests
    {
        private BootStateEntity _entity;

        [SetUp]
        public void SetUp()
        {
            _entity = new BootStateEntity();
        }

        [Test]
        public void Constructor_InitialValue_IsNone()
        {
            Assert.That(_entity.value, Is.EqualTo(BootState.None));
        }

        [TestCase(BootState.None)]
        [TestCase(BootState.Init)]
        [TestCase(BootState.Load)]
        [TestCase(BootState.Splash)]
        [TestCase(BootState.Login)]
        public void Set_WithAnyState_UpdatesValue(BootState state)
        {
            _entity.Set(state);

            Assert.That(_entity.value, Is.EqualTo(state));
        }

        [TestCase(BootState.None)]
        [TestCase(BootState.Init)]
        [TestCase(BootState.Load)]
        [TestCase(BootState.Splash)]
        [TestCase(BootState.Login)]
        public void IsEqual_WithSameState_ReturnsTrue(BootState state)
        {
            _entity.Set(state);

            Assert.That(_entity.IsEqual(state), Is.True);
        }

        [Test]
        public void IsEqual_WithDifferentState_ReturnsFalse()
        {
            _entity.Set(BootState.Init);

            Assert.That(_entity.IsEqual(BootState.Load), Is.False);
        }

        [Test]
        public void IsEqual_BeforeSet_WithNone_ReturnsTrue()
        {
            Assert.That(_entity.IsEqual(BootState.None), Is.True);
        }

        [Test]
        public void IsEqual_BeforeSet_WithNonDefault_ReturnsFalse()
        {
            Assert.That(_entity.IsEqual(BootState.Init), Is.False);
        }

        [Test]
        public void Set_CalledMultipleTimes_ReflectsLastValue()
        {
            _entity.Set(BootState.Init);
            _entity.Set(BootState.Load);
            _entity.Set(BootState.Login);

            Assert.That(_entity.value, Is.EqualTo(BootState.Login));
        }
    }
}
