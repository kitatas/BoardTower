using BoardTower.Common.Application;

namespace BoardTower.Common.Data.DataStore
{
    public sealed class VolumeDTO
    {
        public float value;

        public VolumeDTO()
        {
            value = SoundConfig.INIT_VOLUME;
        }

        public VolumeDTO(VolumeVO volume)
        {
            value = volume.value;
        }

        public VolumeVO ToVO() => new(value);
    }
}