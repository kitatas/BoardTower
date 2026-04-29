using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Domain.Repository;
using Cysharp.Threading.Tasks;

namespace BoardTower.Common.Domain.UseCase
{
    public sealed class BgmUseCase : BaseSoundUseCase<BgmType, BgmSoundVO>
    {
        public BgmUseCase(SaveRepository saveRepository, SoundRepository soundRepository) : base(saveRepository,
            soundRepository)
        {
        }

        protected override async UniTask<(VolumeVO thisVolume, VolumeVO masterVolume)> LoadVolumeAsync(
            CancellationToken token)
        {
            var saveData = await _saveRepository.LoadAsync(token);
            return (saveData.bgmVolume, saveData.masterVolume);
        }

        public override void SaveVolume()
        {
            var vo = new VolumeVO(thisVolumeValue, isThisMute.CurrentValue);
            _saveRepository.SaveBgmVolume(vo);
        }

        protected override BgmSoundVO CreateSound(AudioVO<BgmType> audio, float delay)
        {
            return new BgmSoundVO(audio as BgmAudioVO, delay, isMute.CurrentValue);
        }
    }
}