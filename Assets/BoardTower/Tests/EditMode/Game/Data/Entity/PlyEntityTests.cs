using BoardTower.Game.Data.Entity;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Data.Entity
{
    [TestFixture]
    public sealed class PlyEntityTests
    {
        private PlyEntity _entity;

        [SetUp]
        public void SetUp()
        {
            _entity = new PlyEntity();
        }

        [Test]
        public void Constructor_InitialValue_IsZero()
        {
            Assert.That(_entity.value, Is.EqualTo(0));
        }

        [Test]
        public void Constructor_InitialMaxValue_IsZero()
        {
            Assert.That(_entity.maxValue, Is.EqualTo(0));
        }

        [Test]
        public void SetUp_SetsValueAndMaxValue()
        {
            _entity.SetUp(5);

            Assert.That(_entity.value, Is.EqualTo(5));
            Assert.That(_entity.maxValue, Is.EqualTo(5));
        }

        [Test]
        public void SetUp_WithZero_SetsValueAndMaxValueToZero()
        {
            _entity.SetUp(0);

            Assert.That(_entity.value, Is.EqualTo(0));
            Assert.That(_entity.maxValue, Is.EqualTo(0));
        }

        [Test]
        public void SetMax_WithNegativeValue_ClampsToZero()
        {
            _entity.SetMax(-3);

            Assert.That(_entity.maxValue, Is.EqualTo(0));
        }

        [TestCase(0)]
        [TestCase(3)]
        [TestCase(10)]
        public void SetMax_WithValidValue_SetsMaxValue(int maxValue)
        {
            _entity.SetMax(maxValue);

            Assert.That(_entity.maxValue, Is.EqualTo(maxValue));
        }

        [Test]
        public void Set_ValueClampedToMaxPlusOne()
        {
            _entity.SetUp(5);

            _entity.Set(10);

            Assert.That(_entity.value, Is.EqualTo(6));
        }

        [Test]
        public void Set_ValueClampedToZeroAsMin()
        {
            _entity.SetUp(5);

            _entity.Set(-1);

            Assert.That(_entity.value, Is.EqualTo(0));
        }

        [Test]
        public void Add_IncreasesValue()
        {
            _entity.SetUp(5);

            _entity.Add(2);

            Assert.That(_entity.value, Is.EqualTo(5));
        }

        [Test]
        public void Add_BeyondMax_ClampsToMaxPlusOne()
        {
            _entity.SetUp(3);
            _entity.Set(3);

            _entity.Add(5);

            Assert.That(_entity.value, Is.EqualTo(4));
        }

        [Test]
        public void Subtract_DecreasesValue()
        {
            _entity.SetUp(5);

            _entity.Subtract(2);

            Assert.That(_entity.value, Is.EqualTo(3));
        }

        [Test]
        public void Subtract_BelowZero_ClampsToZero()
        {
            _entity.SetUp(5);
            _entity.Set(1);

            _entity.Subtract(5);

            Assert.That(_entity.value, Is.EqualTo(0));
        }

        [Test]
        public void Subtract_One_FromOne_BecomesZero()
        {
            _entity.SetUp(5);
            _entity.Set(1);

            _entity.Subtract(1);

            Assert.That(_entity.value, Is.EqualTo(0));
        }
    }
}
