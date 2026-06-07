using BoardTower.Common.Application;
using BoardTower.Common.Data.DataStore;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Common.Data.DataStore
{
    [TestFixture]
    public sealed class LocalUserDTOTests
    {
        [Test]
        public void Constructor_Default_SetsEmptyId()
        {
            var sut = new LocalUserDTO();

            Assert.That(sut.id, Is.EqualTo(""));
        }

        [TestCase("user-123")]
        [TestCase("abc")]
        [TestCase("")]
        public void Constructor_WithLocalUserVO_AssignsId(string id)
        {
            var vo = new LocalUserVO(id);

            var sut = new LocalUserDTO(vo);

            Assert.That(sut.id, Is.EqualTo(id));
        }

        [Test]
        public void ToVO_ReturnsLocalUserVOWithSameId()
        {
            var sut = new LocalUserDTO { id = "test-id" };

            var result = sut.ToVO();

            Assert.That(result.id, Is.EqualTo("test-id"));
        }

        [Test]
        public void ToVO_FromDefaultConstructor_ReturnsEmptyId()
        {
            var sut = new LocalUserDTO();

            var result = sut.ToVO();

            Assert.That(result.id, Is.EqualTo(""));
        }

        [Test]
        public void ToVO_FromLocalUserVO_RoundTripsCorrectly()
        {
            var original = new LocalUserVO("round-trip-id");
            var dto = new LocalUserDTO(original);

            var result = dto.ToVO();

            Assert.That(result.id, Is.EqualTo(original.id));
        }
    }
}
