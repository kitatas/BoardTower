using BoardTower.Common.Application;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Common.Application
{
    [TestFixture]
    public sealed class EnumExtensionTests
    {
        [Test]
        public void ToURL_WithApps_ReturnsAppUrl()
        {
            Assert.That(UrlType.Apps.ToURL(), Is.EqualTo(UrlConfig.APP_URL));
        }

        [Test]
        public void ToURL_WithDeveloperApps_ReturnsDeveloperAppUrl()
        {
            Assert.That(UrlType.DeveloperApps.ToURL(), Is.EqualTo(UrlConfig.DEVELOPER_APP_URL));
        }

        [Test]
        public void ToURL_WithNone_ThrowsQuitExceptionVO()
        {
            Assert.That(() => UrlType.None.ToURL(), Throws.TypeOf<QuitExceptionVO>());
        }

        [Test]
        public void ToURL_WithUndefinedValue_ThrowsQuitExceptionVO()
        {
            var undefined = (UrlType)99;

            Assert.That(() => undefined.ToURL(), Throws.TypeOf<QuitExceptionVO>());
        }
    }
}
