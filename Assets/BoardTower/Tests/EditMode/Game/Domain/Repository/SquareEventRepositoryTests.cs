using System.Collections.Generic;
using System.Reflection;
using BoardTower.Common.Application;
using BoardTower.Common.Data.DataStore;
using BoardTower.Game.Application;
using BoardTower.Game.Data.DataStore;
using BoardTower.Game.Domain.Repository;
using NUnit.Framework;
using UnityEngine;

namespace BoardTower.Tests.EditMode.Game.Domain.Repository
{
    [TestFixture]
    public sealed class SquareEventRepositoryTests
    {
        private SquareEventTable _table;
        private List<Object> _createdObjects;

        [SetUp]
        public void SetUp()
        {
            _createdObjects = new List<Object>();
            _table = ScriptableObject.CreateInstance<SquareEventTable>();
            SetTableList(_table, new List<SquareEventData>());
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var createdObject in _createdObjects)
            {
                Object.DestroyImmediate(createdObject);
            }

            Object.DestroyImmediate(_table);
        }

        [Test]
        public void Constructor_WithEmptyTable_InitializesSuccessfully()
        {
            Assert.That(() => new SquareEventRepository(_table), Throws.Nothing);
        }

        [Test]
        public void Constructor_WithNoneTypeEntry_ThrowsQuitExceptionVO()
        {
            var data = CreateData(SquareEventType.None);
            SetTableList(_table, new List<SquareEventData> { data });

            Assert.That(() => new SquareEventRepository(_table), Throws.TypeOf<QuitExceptionVO>());
        }

        [TestCase(SquareEventType.Empty)]
        [TestCase(SquareEventType.Gem)]
        [TestCase(SquareEventType.BeltUp)]
        public void Find_WithRegisteredType_ReturnsVOOfThatType(SquareEventType type)
        {
            SetTableList(_table, new List<SquareEventData>
            {
                CreateData(SquareEventType.Empty),
                CreateData(SquareEventType.Gem),
                CreateData(SquareEventType.BeltUp),
            });
            var repository = new SquareEventRepository(_table);

            var result = repository.Find(type);

            Assert.That(result.type, Is.EqualTo(type));
        }

        [Test]
        public void Find_WithRegisteredType_ReturnsVOHoldingEventObjectOfSameEntry()
        {
            var gemObject = new GameObject("Gem");
            var plyObject = new GameObject("Ply");
            _createdObjects.Add(gemObject);
            _createdObjects.Add(plyObject);
            SetTableList(_table, new List<SquareEventData>
            {
                CreateData(SquareEventType.Gem, gemObject),
                CreateData(SquareEventType.Ply, plyObject),
            });
            var repository = new SquareEventRepository(_table);

            var result = repository.Find(SquareEventType.Ply);

            Assert.That(result.eventObject, Is.SameAs(plyObject));
        }

        [Test]
        public void Find_WithUnregisteredType_ThrowsQuitExceptionVOForInvalidSquareEvent()
        {
            SetTableList(_table, new List<SquareEventData> { CreateData(SquareEventType.Gem) });
            var repository = new SquareEventRepository(_table);

            Assert.That(
                () => repository.Find(SquareEventType.Block),
                Throws.TypeOf<QuitExceptionVO>().With.Property("Message").EqualTo(ExceptionConfig.INVALID_SQUARE_EVENT));
        }

        [Test]
        public void Find_WithEmptyTable_ThrowsQuitExceptionVO()
        {
            var repository = new SquareEventRepository(_table);

            Assert.That(() => repository.Find(SquareEventType.Empty), Throws.TypeOf<QuitExceptionVO>());
        }

        private SquareEventData CreateData(SquareEventType type, GameObject eventObject = null)
        {
            var data = ScriptableObject.CreateInstance<SquareEventData>();
            typeof(SquareEventData)
                .GetField("squareEventType", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(data, type);
            typeof(SquareEventData)
                .GetField("eventObject", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(data, eventObject);
            _createdObjects.Add(data);
            return data;
        }

        private static void SetTableList(SquareEventTable table, List<SquareEventData> items)
        {
            var field = typeof(BaseTable<SquareEventData>)
                .GetField("list", BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(table, items);
        }
    }
}
