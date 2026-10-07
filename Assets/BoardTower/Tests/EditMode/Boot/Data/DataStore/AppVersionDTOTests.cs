using BoardTower.Boot.Data.DataStore;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Boot.Data.DataStore
{
    [TestFixture]
    public sealed class AppVersionDTOTests
    {
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(12, 34)]
        public void ToVO_ReturnsMajor(int major, int minor)
        {
            var dto = new AppVersionDTO { major = major, minor = minor };

            Assert.That(dto.ToVO().major, Is.EqualTo(major));
        }

        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(12, 34)]
        public void ToVO_ReturnsMinor(int major, int minor)
        {
            var dto = new AppVersionDTO { major = major, minor = minor };

            Assert.That(dto.ToVO().minor, Is.EqualTo(minor));
        }
    }
}
