using BoardTower.Common.Application;

namespace BoardTower.Common.Data.DataStore
{
    public sealed class SaveDTO
    {
        public VolumeDTO masterVolume;
        public VolumeDTO bgmVolume;
        public VolumeDTO seVolume;

        public SaveDTO()
        {
            masterVolume = new VolumeDTO();
            bgmVolume = new VolumeDTO();
            seVolume = new VolumeDTO();
        }

        public SaveVO ToVO() => new(masterVolume.ToVO(), bgmVolume.ToVO(), seVolume.ToVO());
    }
}