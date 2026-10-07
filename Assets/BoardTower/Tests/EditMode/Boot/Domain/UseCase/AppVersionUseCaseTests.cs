using System.Collections.Generic;
using BoardTower.Boot.Domain.Repository;
using BoardTower.Boot.Domain.UseCase;
using BoardTower.Common.Application;
using BoardTower.Common.Data.DataStore;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Boot.Domain.UseCase
{
    [TestFixture]
    public sealed class AppVersionUseCaseTests
    {
        private AppVersionUseCase _useCase;
        private PlayFabTitleData _titleData;
        private int _localMajor;
        private int _localMinor;

        [SetUp]
        public void SetUp()
        {
            _titleData = new PlayFabTitleData();
            var repository = new AppVersionRepository(_titleData);
            _useCase = new AppVersionUseCase(repository);

            // NOTE: ローカルバージョンは PlayerSettings の値に依存するため、テスト側でも同じ値を取得して相対的に設定する
            var versions = UnityEngine.Application.version.Split('.');
            _localMajor = int.Parse(versions[0]);
            _localMinor = int.Parse(versions[1]);
        }

        [Test]
        public void IsForceUpdate_WhenRemoteMajorIsGreater_ReturnsTrue()
        {
            // minor が小さくても major が大きければ強制アップデート
            CacheRemoteVersion(_localMajor + 1, 0);

            Assert.That(_useCase.IsForceUpdate(), Is.True);
        }

        [Test]
        public void IsForceUpdate_WhenMajorIsSameAndRemoteMinorIsGreater_ReturnsTrue()
        {
            CacheRemoteVersion(_localMajor, _localMinor + 1);

            Assert.That(_useCase.IsForceUpdate(), Is.True);
        }

        [Test]
        public void IsForceUpdate_WhenRemoteVersionIsSame_ReturnsFalse()
        {
            CacheRemoteVersion(_localMajor, _localMinor);

            Assert.That(_useCase.IsForceUpdate(), Is.False);
        }

        [Test]
        public void IsForceUpdate_WhenMajorIsSameAndRemoteMinorIsLower_ReturnsFalse()
        {
            Assume.That(_localMinor, Is.GreaterThan(0), "ローカル minor が 0 の場合は下げられないためスキップ");
            CacheRemoteVersion(_localMajor, _localMinor - 1);

            Assert.That(_useCase.IsForceUpdate(), Is.False);
        }

        private void CacheRemoteVersion(int major, int minor)
        {
            var json = $"[{{\"major\":{major},\"minor\":{minor}}}]";
            _titleData.CacheTitleData(new Dictionary<string, string>
            {
                { PlayFabConfig.APP_VERSION_KEY, json },
            });
        }
    }
}
