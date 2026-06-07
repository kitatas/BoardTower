using BoardTower.Common.Application;
using BoardTower.Common.Data.DataStore;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Common.Data.DataStore
{
    [TestFixture]
    public sealed class SaveDTOTests
    {
        [Test]
        public void Constructor_Default_UserIsNotNull()
        {
            var sut = new SaveDTO();

            Assert.That(sut.user, Is.Not.Null);
        }

        [Test]
        public void Constructor_Default_UserIdIsEmpty()
        {
            var sut = new SaveDTO();

            Assert.That(sut.user.id, Is.EqualTo(""));
        }

        [Test]
        public void Constructor_Default_MasterVolumeIsNotNull()
        {
            var sut = new SaveDTO();

            Assert.That(sut.masterVolume, Is.Not.Null);
        }

        [Test]
        public void Constructor_Default_BgmVolumeIsNotNull()
        {
            var sut = new SaveDTO();

            Assert.That(sut.bgmVolume, Is.Not.Null);
        }

        [Test]
        public void Constructor_Default_SeVolumeIsNotNull()
        {
            var sut = new SaveDTO();

            Assert.That(sut.seVolume, Is.Not.Null);
        }

        [Test]
        public void Constructor_Default_VolumeValuesAreInitVolume()
        {
            var sut = new SaveDTO();

            Assert.That(sut.masterVolume.value, Is.EqualTo(SoundConfig.INIT_VOLUME));
            Assert.That(sut.bgmVolume.value, Is.EqualTo(SoundConfig.INIT_VOLUME));
            Assert.That(sut.seVolume.value, Is.EqualTo(SoundConfig.INIT_VOLUME));
        }

        [Test]
        public void Recreate_ResetsUserToEmpty()
        {
            var original = new SaveDTO { user = new LocalUserDTO { id = "original-id" } };

            var result = SaveDTO.Recreate(original);

            Assert.That(result.user.id, Is.EqualTo(""));
        }

        [Test]
        public void Recreate_PreservesMasterVolume()
        {
            var original = new SaveDTO();
            original.masterVolume.value = 0.6f;
            original.masterVolume.isMute = true;

            var result = SaveDTO.Recreate(original);

            Assert.That(result.masterVolume, Is.EqualTo(original.masterVolume));
        }

        [Test]
        public void Recreate_PreservesBgmVolume()
        {
            var original = new SaveDTO();
            original.bgmVolume.value = 0.4f;

            var result = SaveDTO.Recreate(original);

            Assert.That(result.bgmVolume, Is.EqualTo(original.bgmVolume));
        }

        [Test]
        public void Recreate_PreservesSeVolume()
        {
            var original = new SaveDTO();
            original.seVolume.value = 0.3f;

            var result = SaveDTO.Recreate(original);

            Assert.That(result.seVolume, Is.EqualTo(original.seVolume));
        }

        [Test]
        public void ToVO_ReturnsNonNull()
        {
            var sut = new SaveDTO();

            var result = sut.ToVO();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void ToVO_UserIdMatchesDTO()
        {
            var sut = new SaveDTO { user = new LocalUserDTO { id = "test-id" } };

            var result = sut.ToVO();

            Assert.That(result.user.id, Is.EqualTo("test-id"));
        }

        [Test]
        public void ToVO_VolumeValuesMatchDTO()
        {
            var sut = new SaveDTO();
            sut.masterVolume.value = 0.8f;
            sut.bgmVolume.value = 0.6f;
            sut.seVolume.value = 0.4f;

            var result = sut.ToVO();

            Assert.That(result.masterVolume.value, Is.EqualTo(0.8f));
            Assert.That(result.bgmVolume.value, Is.EqualTo(0.6f));
            Assert.That(result.seVolume.value, Is.EqualTo(0.4f));
        }
    }
}
