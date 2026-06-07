using BoardTower.Common.Application;
using BoardTower.Common.Data.DataStore;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Common.Data.DataStore
{
    [TestFixture]
    public sealed class VolumeDTOTests
    {
        [Test]
        public void Constructor_Default_SetsInitVolume()
        {
            var sut = new VolumeDTO();

            Assert.That(sut.value, Is.EqualTo(SoundConfig.INIT_VOLUME));
        }

        [Test]
        public void Constructor_Default_SetsMuteToFalse()
        {
            var sut = new VolumeDTO();

            Assert.That(sut.isMute, Is.False);
        }

        [TestCase(0.0f, false)]
        [TestCase(0.5f, true)]
        [TestCase(1.0f, false)]
        public void Constructor_WithVolumeVO_AssignsValueAndMute(float value, bool isMute)
        {
            var vo = new VolumeVO(value, isMute);

            var sut = new VolumeDTO(vo);

            Assert.That(sut.value, Is.EqualTo(value));
            Assert.That(sut.isMute, Is.EqualTo(isMute));
        }

        [Test]
        public void ToVO_ReturnsVolumeVOWithSameValues()
        {
            var sut = new VolumeDTO { value = 0.8f, isMute = true };

            var result = sut.ToVO();

            Assert.That(result.value, Is.EqualTo(0.8f));
            Assert.That(result.isMute, Is.True);
        }

        [Test]
        public void ToVO_FromDefaultConstructor_ReturnsInitVolume()
        {
            var sut = new VolumeDTO();

            var result = sut.ToVO();

            Assert.That(result.value, Is.EqualTo(SoundConfig.INIT_VOLUME));
            Assert.That(result.isMute, Is.False);
        }

        [Test]
        public void ToVO_FromVolumeVO_RoundTripsCorrectly()
        {
            var original = new VolumeVO(0.3f, true);
            var dto = new VolumeDTO(original);

            var result = dto.ToVO();

            Assert.That(result.value, Is.EqualTo(original.value));
            Assert.That(result.isMute, Is.EqualTo(original.isMute));
        }
    }
}
