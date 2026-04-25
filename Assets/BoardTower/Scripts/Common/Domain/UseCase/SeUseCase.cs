using BoardTower.Common.Application;
using BoardTower.Common.Domain.Repository;

namespace BoardTower.Common.Domain.UseCase
{
    public sealed class SeUseCase : BaseSoundUseCase<SeType, SeSoundVO>
    {
        public SeUseCase(SoundRepository soundRepository) : base(soundRepository)
        {
        }

        protected override SeSoundVO CreateSound(AudioVO<SeType> audio, float delay)
        {
            return new SeSoundVO(audio as SeAudioVO, delay);
        }
    }
}