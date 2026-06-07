using BoardTower.Game.Data.Entity;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Data.Entity
{
    [TestFixture]
    public sealed class GemEntityTests
    {
        private GemEntity _entity;

        [SetUp]
        public void SetUp()
        {
            _entity = new GemEntity();
        }

        [Test]
        public void Constructor_InitialValue_IsZero()
        {
            Assert.That(_entity.value, Is.EqualTo(0));
        }

        [Test]
        public void Reset_SetsValueToZero()
        {
            _entity.Set(5);

            _entity.Reset();

            Assert.That(_entity.value, Is.EqualTo(0));
        }

        [Test]
        public void Reset_FromInitialState_ValueRemainsZero()
        {
            _entity.Reset();

            Assert.That(_entity.value, Is.EqualTo(0));
        }

        [Test]
        public void Add_IncreasesValueByAmount()
        {
            _entity.Add(3);

            Assert.That(_entity.value, Is.EqualTo(3));
        }

        [Test]
        public void Add_CalledMultipleTimes_AccumulatesCorrectly()
        {
            _entity.Add(3);
            _entity.Add(2);

            Assert.That(_entity.value, Is.EqualTo(5));
        }

        [Test]
        public void Subtract_DecreasesValueByAmount()
        {
            _entity.Set(5);

            _entity.Subtract(2);

            Assert.That(_entity.value, Is.EqualTo(3));
        }

        [Test]
        public void Subtract_BelowZero_ClampsToZero()
        {
            _entity.Set(1);

            _entity.Subtract(5);

            Assert.That(_entity.value, Is.EqualTo(0));
        }

        [Test]
        public void Set_WithNegativeValue_ClampsToZero()
        {
            _entity.Set(-3);

            Assert.That(_entity.value, Is.EqualTo(0));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(100)]
        public void Set_WithValidValue_SetsCorrectly(int value)
        {
            _entity.Set(value);

            Assert.That(_entity.value, Is.EqualTo(value));
        }

        [Test]
        public void IsEqual_WithSameValue_ReturnsTrue()
        {
            _entity.Set(3);

            Assert.That(_entity.IsEqual(3), Is.True);
        }

        [Test]
        public void IsEqual_WithDifferentValue_ReturnsFalse()
        {
            _entity.Set(3);

            Assert.That(_entity.IsEqual(5), Is.False);
        }
    }
}
