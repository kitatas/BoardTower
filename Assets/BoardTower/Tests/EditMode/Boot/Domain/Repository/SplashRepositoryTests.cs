using System.Collections.Generic;
using System.Reflection;
using BoardTower.Boot.Application;
using BoardTower.Boot.Data.DataStore;
using BoardTower.Boot.Domain.Repository;
using BoardTower.Common.Application;
using BoardTower.Common.Data.DataStore;
using NUnit.Framework;
using UnityEngine;

namespace BoardTower.Tests.EditMode.Boot.Domain.Repository
{
    [TestFixture]
    public sealed class SplashRepositoryTests
    {
        private SplashTable _splashTable;

        [SetUp]
        public void SetUp()
        {
            _splashTable = ScriptableObject.CreateInstance<SplashTable>();
            SetTableList(_splashTable, new List<SplashData>());
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_splashTable);
        }

        [Test]
        public void Constructor_WithEmptyTable_InitializesSuccessfully()
        {
            var repository = new SplashRepository(_splashTable);

            Assert.That(repository, Is.Not.Null);
        }

        [TestCase(SplashType.Developer)]
        [TestCase(SplashType.PlayFab)]
        public void Find_WithMissingType_ThrowsQuitExceptionVO(SplashType type)
        {
            var repository = new SplashRepository(_splashTable);

            Assert.That(() => repository.Find(type), Throws.TypeOf<QuitExceptionVO>());
        }

        [Test]
        public void Find_WithExistingType_ReturnsSplashVO()
        {
            var splashData = ScriptableObject.CreateInstance<SplashData>();
            var texture = new Texture2D(1, 1);
            var sprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), Vector2.zero);
            SetSplashDataFields(splashData, SplashType.Developer, sprite);
            SetTableList(_splashTable, new List<SplashData> { splashData });

            var repository = new SplashRepository(_splashTable);
            var result = repository.Find(SplashType.Developer);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.type, Is.EqualTo(SplashType.Developer));
            Assert.That(result.sprite, Is.EqualTo(sprite));

            Object.DestroyImmediate(splashData);
        }

        [Test]
        public void Find_WithMultipleTypes_ReturnCorrectSplashVO()
        {
            var developerData = ScriptableObject.CreateInstance<SplashData>();
            var playFabData = ScriptableObject.CreateInstance<SplashData>();
            var texture = new Texture2D(1, 1);
            var developerSprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), Vector2.zero);
            var playFabSprite = Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1), Vector2.zero);
            SetSplashDataFields(developerData, SplashType.Developer, developerSprite);
            SetSplashDataFields(playFabData, SplashType.PlayFab, playFabSprite);
            SetTableList(_splashTable, new List<SplashData> { developerData, playFabData });

            var repository = new SplashRepository(_splashTable);

            Assert.That(repository.Find(SplashType.Developer).type, Is.EqualTo(SplashType.Developer));
            Assert.That(repository.Find(SplashType.PlayFab).type, Is.EqualTo(SplashType.PlayFab));

            Object.DestroyImmediate(developerData);
            Object.DestroyImmediate(playFabData);
        }

        private static void SetTableList(SplashTable table, List<SplashData> items)
        {
            var field = typeof(BaseTable<SplashData>)
                .GetField("list", BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(table, items);
        }

        private static void SetSplashDataFields(SplashData data, SplashType type, Sprite sprite)
        {
            var typeField = typeof(SplashData)
                .GetField("splashType", BindingFlags.NonPublic | BindingFlags.Instance);
            typeField.SetValue(data, type);

            var spriteField = typeof(SplashData)
                .GetField("splashSprite", BindingFlags.NonPublic | BindingFlags.Instance);
            spriteField.SetValue(data, sprite);
        }
    }
}
