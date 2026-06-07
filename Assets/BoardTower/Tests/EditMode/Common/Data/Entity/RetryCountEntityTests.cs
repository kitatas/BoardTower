using BoardTower.Common.Application;
using BoardTower.Common.Data.Entity;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Common.Data.Entity
{
    [TestFixture]
    public sealed class RetryCountEntityTests
    {
        private RetryCountEntity _entity;

        [SetUp]
        public void SetUp()
        {
            _entity = new RetryCountEntity();
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
        public void Reset_CalledFromInitialState_ValueRemainsZero()
        {
            _entity.Reset();

            Assert.That(_entity.value, Is.EqualTo(0));
        }

        [Test]
        public void Increment_IncreasesValueByOne()
        {
            _entity.Increment();

            Assert.That(_entity.value, Is.EqualTo(1));
        }

        [Test]
        public void Increment_CalledMultipleTimes_AccumulatesCorrectly()
        {
            _entity.Increment();
            _entity.Increment();
            _entity.Increment();

            Assert.That(_entity.value, Is.EqualTo(3));
        }

        [Test]
        public void Update_WithIsEqualTrue_IncrementsValue()
        {
            _entity.Set(2);

            _entity.Update(true);

            Assert.That(_entity.value, Is.EqualTo(3));
        }

        [Test]
        public void Update_WithIsEqualFalse_SetsValueToOne()
        {
            _entity.Set(5);

            _entity.Update(false);

            Assert.That(_entity.value, Is.EqualTo(1));
        }

        [Test]
        public void Update_FromZeroWithIsEqualTrue_SetsValueToOne()
        {
            _entity.Update(true);

            Assert.That(_entity.value, Is.EqualTo(1));
        }

        [Test]
        public void IsMaxRetry_WhenBelowMaxRetryCount_ReturnsFalse()
        {
            _entity.Set(ExceptionConfig.MAX_RETRY_COUNT);

            Assert.That(_entity.IsMaxRetry(), Is.False);
        }

        [Test]
        public void IsMaxRetry_WhenAboveMaxRetryCount_ReturnsTrue()
        {
            _entity.Set(ExceptionConfig.MAX_RETRY_COUNT + 1);

            Assert.That(_entity.IsMaxRetry(), Is.True);
        }

        [Test]
        public void IsMaxRetry_WhenAtInitialValue_ReturnsFalse()
        {
            Assert.That(_entity.IsMaxRetry(), Is.False);
        }

        [Test]
        public void Set_UpdatesValue()
        {
            _entity.Set(10);

            Assert.That(_entity.value, Is.EqualTo(10));
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
