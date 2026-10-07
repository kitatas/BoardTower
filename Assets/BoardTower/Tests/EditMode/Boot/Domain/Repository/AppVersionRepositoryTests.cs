using System.Collections.Generic;
using BoardTower.Boot.Domain.Repository;
using BoardTower.Common.Application;
using BoardTower.Common.Data.DataStore;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Boot.Domain.Repository
{
    [TestFixture]
    public sealed class AppVersionRepositoryTests
    {
        private PlayFabTitleData _titleData;
        private AppVersionRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _titleData = new PlayFabTitleData();
            _repository = new AppVersionRepository(_titleData);
        }

        [TestCase(1, 2)]
        [TestCase(0, 1)]
        [TestCase(10, 0)]
        public void Fetch_WithValidJson_ReturnsMajor(int major, int minor)
        {
            CacheJson($"[{{\"major\":{major},\"minor\":{minor}}}]");

            var result = _repository.Fetch();

            Assert.That(result.major, Is.EqualTo(major));
        }

        [TestCase(1, 2)]
        [TestCase(0, 1)]
        [TestCase(10, 0)]
        public void Fetch_WithValidJson_ReturnsMinor(int major, int minor)
        {
            CacheJson($"[{{\"major\":{major},\"minor\":{minor}}}]");

            var result = _repository.Fetch();

            Assert.That(result.minor, Is.EqualTo(minor));
        }

        [Test]
        public void Fetch_WithMultipleEntries_ReturnsFirstEntry()
        {
            CacheJson("[{\"major\":3,\"minor\":4},{\"major\":9,\"minor\":9}]");

            var result = _repository.Fetch();

            Assert.That(result.major, Is.EqualTo(3));
        }

        [Test]
        public void Fetch_WhenKeyIsMissing_ThrowsQuitExceptionVO()
        {
            _titleData.CacheTitleData(new Dictionary<string, string>
            {
                { "other_key", "[]" },
            });

            Assert.That(() => _repository.Fetch(), Throws.TypeOf<QuitExceptionVO>());
        }

        [Test]
        public void Fetch_WhenFirstEntryIsNull_ThrowsQuitExceptionVOWithNotFoundAppVersion()
        {
            CacheJson("[null]");

            Assert.That(() => _repository.Fetch(),
                Throws.TypeOf<QuitExceptionVO>().And.Message.EqualTo(ExceptionConfig.NOT_FOUND_APP_VERSION));
        }

        private void CacheJson(string json)
        {
            _titleData.CacheTitleData(new Dictionary<string, string>
            {
                { PlayFabConfig.APP_VERSION_KEY, json },
            });
        }
    }
}
