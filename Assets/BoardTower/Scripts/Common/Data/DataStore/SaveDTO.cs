using BoardTower.Common.Application;

namespace BoardTower.Common.Data.DataStore
{
    public sealed class SaveDTO
    {
        public UserDTO user;
        public VolumeDTO masterVolume;
        public VolumeDTO bgmVolume;
        public VolumeDTO seVolume;

        public SaveDTO()
        {
            user = new UserDTO();
            masterVolume = new VolumeDTO();
            bgmVolume = new VolumeDTO();
            seVolume = new VolumeDTO();
        }

        public static SaveDTO Recreate(SaveDTO dto)
        {
            return new SaveDTO
            {
                user = new UserDTO(),
                masterVolume = dto.masterVolume,
                bgmVolume = dto.bgmVolume,
                seVolume = dto.seVolume,
            };
        }

        public SaveVO ToVO() => new(user.ToVO(), masterVolume.ToVO(), bgmVolume.ToVO(), seVolume.ToVO());
    }
}