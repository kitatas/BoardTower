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

        public static SaveDTO Recreate(SaveDTO dto)
        {
            return new SaveDTO
            {
                masterVolume = dto.masterVolume,
                bgmVolume = dto.bgmVolume,
                seVolume = dto.seVolume,
            };
        }

        public SaveVO ToVO() => new(masterVolume.ToVO(), bgmVolume.ToVO(), seVolume.ToVO());
    }
}