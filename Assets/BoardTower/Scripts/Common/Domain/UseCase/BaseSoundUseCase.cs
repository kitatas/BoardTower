using System;
using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Domain.Repository;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace BoardTower.Common.Domain.UseCase
{
    public abstract class BaseSoundUseCase<TType, TSoundVO> : IDisposable
        where TType : Enum where TSoundVO : SoundVO<TType>
    {
        protected readonly SaveRepository _saveRepository;
        private readonly SoundRepository _soundRepository;
        private readonly Subject<TSoundVO> _play;
        private readonly ReactiveProperty<float> _volume;
        private readonly ReactiveProperty<bool> _isMute;

        public BaseSoundUseCase(SaveRepository saveRepository, SoundRepository soundRepository)
        {
            _saveRepository = saveRepository;
            _soundRepository = soundRepository;
            _play = new Subject<TSoundVO>();
            _volume = new ReactiveProperty<float>(0.0f);
            _isMute = new ReactiveProperty<bool>(false);
        }

        public virtual Subject<TSoundVO> play => _play;
        public virtual ReadOnlyReactiveProperty<float> volume => _volume;
        public virtual ReadOnlyReactiveProperty<bool> isMute => _isMute;

        protected abstract UniTask<VolumeVO> LoadVolumeAsync(CancellationToken token);

        public virtual async UniTask LoadAsync(CancellationToken token)
        {
            var data = await LoadVolumeAsync(token);
            SetVolume(data.value);
            _isMute.Value = data.isMute;
        }

        public virtual void Play(TType type, float delay = 0.0f)
        {
            var audio = _soundRepository.Find(type);
            var sound = CreateSound(audio, delay);
            _play?.OnNext(sound);
        }

        public virtual void SetVolume(float value)
        {
            _volume.Value = Mathf.Clamp01(value);
        }

        public virtual void SwitchMute()
        {
            _isMute.Value = !isMute.CurrentValue;
            SaveVolume();
        }

        public abstract void SaveVolume();

        protected abstract TSoundVO CreateSound(AudioVO<TType> audio, float delay);

        void IDisposable.Dispose()
        {
            _play?.Dispose();
            _volume?.Dispose();
            _isMute?.Dispose();
        }
    }
}