using System.Linq;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Data.Entity
{
    [TestFixture]
    public sealed class EventLogEntityTests
    {
        private EventLogEntity _entity;

        [SetUp]
        public void SetUp()
        {
            _entity = new EventLogEntity();
        }

        [Test]
        public void Constructor_InitialEventLogs_IsEmpty()
        {
            Assert.That(_entity.eventLogs.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Add_WithValidTypeAndValue_AddsOneEntry()
        {
            _entity.Add(EventLogType.Gem, 3);

            Assert.That(_entity.eventLogs.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Add_WithValidTypeAndValue_StoresCorrectType()
        {
            _entity.Add(EventLogType.Score, 10);

            Assert.That(_entity.eventLogs.First().type, Is.EqualTo(EventLogType.Score));
        }

        [Test]
        public void Add_WithValidTypeAndValue_StoresCorrectValue()
        {
            _entity.Add(EventLogType.Gem, 5);

            Assert.That(_entity.eventLogs.First().value, Is.EqualTo(5));
        }

        [Test]
        public void Add_CalledMultipleTimes_AccumulatesAllEntries()
        {
            _entity.Add(EventLogType.Gem, 1);
            _entity.Add(EventLogType.Score, 100);
            _entity.Add(EventLogType.Heart, 2);

            Assert.That(_entity.eventLogs.Count(), Is.EqualTo(3));
        }

        [TestCase(EventLogType.Ride)]
        [TestCase(EventLogType.Combo)]
        [TestCase(EventLogType.Keep)]
        [TestCase(EventLogType.Gem)]
        [TestCase(EventLogType.Score)]
        [TestCase(EventLogType.Heart)]
        public void Add_WithEachValidType_StoresCorrectType(EventLogType type)
        {
            _entity.Add(type, 1);

            Assert.That(_entity.eventLogs.First().type, Is.EqualTo(type));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(int.MaxValue)]
        public void Add_WithBoundaryValues_StoresCorrectValue(int value)
        {
            _entity.Add(EventLogType.Score, value);

            Assert.That(_entity.eventLogs.First().value, Is.EqualTo(value));
        }

        [Test]
        public void Clear_RemovesAllEntries()
        {
            _entity.Add(EventLogType.Gem, 1);
            _entity.Add(EventLogType.Score, 100);

            _entity.Clear();

            Assert.That(_entity.eventLogs.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Clear_WhenEmpty_RemainsEmpty()
        {
            _entity.Clear();

            Assert.That(_entity.eventLogs.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Add_AfterClear_AddsNewEntry()
        {
            _entity.Add(EventLogType.Gem, 5);
            _entity.Clear();

            _entity.Add(EventLogType.Score, 10);

            Assert.That(_entity.eventLogs.Count(), Is.EqualTo(1));
        }
    }
}
