using System;
using BoardTower.Common.Application;
using BoardTower.Common.Domain.Repository;
using R3;
using UnityEngine;

namespace BoardTower.Common.Domain.UseCase
{
    public abstract class BaseSoundUseCase<TType, TSoundVO> : IDisposable
        where TType : Enum where TSoundVO : SoundVO<TType>
    {
        private readonly SoundRepository _soundRepository;
        private readonly Subject<TSoundVO> _play;
        private readonly ReactiveProperty<float> _volume;

        public BaseSoundUseCase(SoundRepository soundRepository)
        {
            _soundRepository = soundRepository;
            _play = new Subject<TSoundVO>();
            _volume = new ReactiveProperty<float>(SoundConfig.INIT_VOLUME);
        }

        public virtual Subject<TSoundVO> play => _play;
        public virtual ReadOnlyReactiveProperty<float> volume => _volume;

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

        protected abstract TSoundVO CreateSound(AudioVO<TType> audio, float delay);

        void IDisposable.Dispose()
        {
            _play?.Dispose();
            _volume?.Dispose();
        }
    }
}