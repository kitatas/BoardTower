using BoardTower.Common.Application;
using BoardTower.Common.Domain.Repository;

namespace BoardTower.Common.Domain.UseCase
{
    public sealed class BgmUseCase : BaseSoundUseCase<BgmType, BgmSoundVO>
    {
        public BgmUseCase(SoundRepository soundRepository) : base(soundRepository)
        {
        }

        protected override BgmSoundVO CreateSound(AudioVO<BgmType> audio, float delay)
        {
            return new BgmSoundVO(audio as BgmAudioVO, delay);
        }
    }
}