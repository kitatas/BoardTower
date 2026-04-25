using System;
using BoardTower.Common.Application;
using BoardTower.Common.Domain.Repository;
using R3;

namespace BoardTower.Common.Domain.UseCase
{
    public abstract class BaseSoundUseCase<TType, TSoundVO> : IDisposable
        where TType : Enum where TSoundVO : SoundVO<TType>
    {
        private readonly SoundRepository _soundRepository;
        private readonly Subject<TSoundVO> _play;

        public BaseSoundUseCase(SoundRepository soundRepository)
        {
            _soundRepository = soundRepository;
            _play = new Subject<TSoundVO>();
        }

        public virtual Subject<TSoundVO> play => _play;

        public virtual void Play(TType type, float delay = 0.0f)
        {
            var audio = _soundRepository.Find(type);
            var sound = CreateSound(audio, delay);
            _play?.OnNext(sound);
        }

        protected abstract TSoundVO CreateSound(AudioVO<TType> audio, float delay);

        void IDisposable.Dispose()
        {
            _play?.Dispose();
        }
    }
}