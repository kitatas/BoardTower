using BoardTower.Game.Data.Entity;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Data.Entity
{
    [TestFixture]
    public sealed class RoundEntityTests
    {
        private RoundEntity _entity;

        [SetUp]
        public void SetUp()
        {
            _entity = new RoundEntity();
        }

        [Test]
        public void Constructor_InitialValue_IsZero()
        {
            Assert.That(_entity.value, Is.EqualTo(0));
        }

        [Test]
        public void Add_IncreasesValueByAmount()
        {
            _entity.Add(1);

            Assert.That(_entity.value, Is.EqualTo(1));
        }

        [Test]
        public void Add_CalledMultipleTimes_AccumulatesCorrectly()
        {
            _entity.Add(1);
            _entity.Add(1);
            _entity.Add(1);

            Assert.That(_entity.value, Is.EqualTo(3));
        }

        [Test]
        public void Add_WithLargerValue_IncreasesCorrectly()
        {
            _entity.Set(3);

            _entity.Add(4);

            Assert.That(_entity.value, Is.EqualTo(7));
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

        [TestCase(1)]
        [TestCase(3)]
        [TestCase(7)]
        public void Set_WithValidValue_SetsCorrectly(int value)
        {
            _entity.Set(value);

            Assert.That(_entity.value, Is.EqualTo(value));
        }

        [Test]
        public void IsEqual_WithSameValue_ReturnsTrue()
        {
            _entity.Set(5);

            Assert.That(_entity.IsEqual(5), Is.True);
        }

        [Test]
        public void IsEqual_WithDifferentValue_ReturnsFalse()
        {
            _entity.Set(3);

            Assert.That(_entity.IsEqual(7), Is.False);
        }
    }
}
