using BoardTower.Common.Application;

namespace BoardTower.Common.Data.DataStore
{
    public sealed class SaveDTO
    {
        public VolumeDTO bgmVolume;
        public VolumeDTO seVolume;

        public SaveDTO()
        {
            bgmVolume = new VolumeDTO();
            seVolume = new VolumeDTO();
        }

        public SaveVO ToVO() => new(bgmVolume.ToVO(), seVolume.ToVO());
    }
}