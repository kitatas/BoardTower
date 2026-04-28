using System;
using BoardTower.Common.Application;

namespace BoardTower.Common.Data.DataStore
{
    [Serializable]
    public sealed class VolumeDTO
    {
        public float value;
        public bool isMute;

        public VolumeDTO()
        {
            value = SoundConfig.INIT_VOLUME;
            isMute = false;
        }

        public VolumeDTO(VolumeVO volume)
        {
            value = volume.value;
            isMute = volume.isMute;
        }

        public VolumeVO ToVO() => new(value, isMute);
    }
}