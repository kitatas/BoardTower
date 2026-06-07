using System.Collections.Generic;
using System.Reflection;
using BoardTower.Common.Application;
using BoardTower.Common.Data.DataStore;
using BoardTower.Common.Domain.Repository;
using NUnit.Framework;
using UnityEngine;

namespace BoardTower.Tests.EditMode.Common.Domain.Repository
{
    [TestFixture]
    public sealed class SoundRepositoryTests
    {
        private BgmTable _bgmTable;
        private SeTable _seTable;

        [SetUp]
        public void SetUp()
        {
            _bgmTable = ScriptableObject.CreateInstance<BgmTable>();
            _seTable = ScriptableObject.CreateInstance<SeTable>();
            SetTableList(_bgmTable, new List<BgmData>());
            SetTableList(_seTable, new List<SeData>());
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_bgmTable);
            Object.DestroyImmediate(_seTable);
        }

        [Test]
        public void Constructor_WithEmptyTables_InitializesSuccessfully()
        {
            var repository = new SoundRepository(_bgmTable, _seTable);

            Assert.That(repository, Is.Not.Null);
        }

        [TestCase(BgmType.Top)]
        [TestCase(BgmType.Game)]
        public void FindBgm_WithMissingType_ThrowsQuitExceptionVO(BgmType type)
        {
            var repository = new SoundRepository(_bgmTable, _seTable);

            Assert.That(() => repository.Find(type), Throws.TypeOf<QuitExceptionVO>());
        }

        [TestCase(SeType.Decision)]
        [TestCase(SeType.Cancel)]
        public void FindSe_WithMissingType_ThrowsQuitExceptionVO(SeType type)
        {
            var repository = new SoundRepository(_bgmTable, _seTable);

            Assert.That(() => repository.Find(type), Throws.TypeOf<QuitExceptionVO>());
        }

        [Test]
        public void FindBgm_WithExistingType_ReturnsBgmAudioVO()
        {
            var bgmData = CreateBgmData(BgmType.Top);
            SetTableList(_bgmTable, new List<BgmData> { bgmData });

            var repository = new SoundRepository(_bgmTable, _seTable);
            var result = repository.Find(BgmType.Top);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.type, Is.EqualTo(BgmType.Top));

            Object.DestroyImmediate(bgmData);
        }

        [Test]
        public void FindSe_WithExistingType_ReturnsSeAudioVO()
        {
            var seData = CreateSeData(SeType.Decision);
            SetTableList(_seTable, new List<SeData> { seData });

            var repository = new SoundRepository(_bgmTable, _seTable);
            var result = repository.Find(SeType.Decision);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.type, Is.EqualTo(SeType.Decision));

            Object.DestroyImmediate(seData);
        }

        [Test]
        public void FindBgm_WithMultipleTypes_ReturnsCorrectVO()
        {
            var topData = CreateBgmData(BgmType.Top);
            var gameData = CreateBgmData(BgmType.Game);
            SetTableList(_bgmTable, new List<BgmData> { topData, gameData });

            var repository = new SoundRepository(_bgmTable, _seTable);

            Assert.That(repository.Find(BgmType.Top).type, Is.EqualTo(BgmType.Top));
            Assert.That(repository.Find(BgmType.Game).type, Is.EqualTo(BgmType.Game));

            Object.DestroyImmediate(topData);
            Object.DestroyImmediate(gameData);
        }

        [Test]
        public void FindSe_WithMultipleTypes_ReturnsCorrectVO()
        {
            var decisionData = CreateSeData(SeType.Decision);
            var cancelData = CreateSeData(SeType.Cancel);
            SetTableList(_seTable, new List<SeData> { decisionData, cancelData });

            var repository = new SoundRepository(_bgmTable, _seTable);

            Assert.That(repository.Find(SeType.Decision).type, Is.EqualTo(SeType.Decision));
            Assert.That(repository.Find(SeType.Cancel).type, Is.EqualTo(SeType.Cancel));

            Object.DestroyImmediate(decisionData);
            Object.DestroyImmediate(cancelData);
        }

        [Test]
        public void FindGeneric_WithBgmType_ReturnsBgmAudioVO()
        {
            var bgmData = CreateBgmData(BgmType.Top);
            SetTableList(_bgmTable, new List<BgmData> { bgmData });

            var repository = new SoundRepository(_bgmTable, _seTable);
            var result = repository.Find<BgmType>(BgmType.Top);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.type, Is.EqualTo(BgmType.Top));

            Object.DestroyImmediate(bgmData);
        }

        [Test]
        public void FindGeneric_WithSeType_ReturnsSeAudioVO()
        {
            var seData = CreateSeData(SeType.Decision);
            SetTableList(_seTable, new List<SeData> { seData });

            var repository = new SoundRepository(_bgmTable, _seTable);
            var result = repository.Find<SeType>(SeType.Decision);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.type, Is.EqualTo(SeType.Decision));

            Object.DestroyImmediate(seData);
        }

        private static BgmData CreateBgmData(BgmType type)
        {
            var data = ScriptableObject.CreateInstance<BgmData>();
            var field = typeof(BgmData).GetField("bgmType", BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(data, type);
            return data;
        }

        private static SeData CreateSeData(SeType type)
        {
            var data = ScriptableObject.CreateInstance<SeData>();
            var field = typeof(SeData).GetField("seType", BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(data, type);
            return data;
        }

        private static void SetTableList<T>(BaseTable<T> table, List<T> items) where T : ScriptableObject
        {
            var field = typeof(BaseTable<T>).GetField("list", BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(table, items);
        }
    }
}
