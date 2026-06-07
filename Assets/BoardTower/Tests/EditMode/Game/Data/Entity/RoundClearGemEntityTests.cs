using BoardTower.Game.Data.Entity;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Data.Entity
{
    [TestFixture]
    public sealed class RoundClearGemEntityTests
    {
        private RoundClearGemEntity _entity;

        [SetUp]
        public void SetUp()
        {
            _entity = new RoundClearGemEntity();
        }

        [Test]
        public void Constructor_InitialValue_IsZero()
        {
            Assert.That(_entity.value, Is.EqualTo(0));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(100)]
        public void Set_WithPositiveOrZeroValue_SetsCorrectly(int value)
        {
            _entity.Set(value);

            Assert.That(_entity.value, Is.EqualTo(value));
        }

        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(-100)]
        public void Set_WithNegativeValue_ClampsToZero(int value)
        {
            _entity.Set(value);

            Assert.That(_entity.value, Is.EqualTo(0));
        }

        [Test]
        public void Set_OverwritesPreviousValue()
        {
            _entity.Set(10);
            _entity.Set(5);

            Assert.That(_entity.value, Is.EqualTo(5));
        }

        [Test]
        public void IsEqual_WithSameValue_ReturnsTrue()
        {
            _entity.Set(7);

            Assert.That(_entity.IsEqual(7), Is.True);
        }

        [Test]
        public void IsEqual_WithDifferentValue_ReturnsFalse()
        {
            _entity.Set(7);

            Assert.That(_entity.IsEqual(3), Is.False);
        }
    }
}
