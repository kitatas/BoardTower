using BoardTower.Game.Data.Entity;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Data.Entity
{
    [TestFixture]
    public sealed class ScoreEntityTests
    {
        private ScoreEntity _entity;

        [SetUp]
        public void SetUp()
        {
            _entity = new ScoreEntity();
        }

        [Test]
        public void Constructor_InitialValue_IsZero()
        {
            Assert.That(_entity.value, Is.EqualTo(0));
        }

        [Test]
        public void Reset_SetsValueToZero()
        {
            _entity.Set(500);

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
            _entity.Add(100);

            Assert.That(_entity.value, Is.EqualTo(100));
        }

        [Test]
        public void Add_CalledMultipleTimes_AccumulatesCorrectly()
        {
            _entity.Add(100);
            _entity.Add(200);

            Assert.That(_entity.value, Is.EqualTo(300));
        }

        [Test]
        public void Set_WithNegativeValue_ClampsToZero()
        {
            _entity.Set(-100);

            Assert.That(_entity.value, Is.EqualTo(0));
        }

        [TestCase(0)]
        [TestCase(100)]
        [TestCase(9999)]
        public void Set_WithValidValue_SetsCorrectly(int value)
        {
            _entity.Set(value);

            Assert.That(_entity.value, Is.EqualTo(value));
        }

        [Test]
        public void IsEqual_WithSameValue_ReturnsTrue()
        {
            _entity.Set(500);

            Assert.That(_entity.IsEqual(500), Is.True);
        }

        [Test]
        public void IsEqual_WithDifferentValue_ReturnsFalse()
        {
            _entity.Set(500);

            Assert.That(_entity.IsEqual(999), Is.False);
        }
    }
}
