using System;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using R3;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class GemUseCase : IDisposable
    {
        private readonly EventLogEntity _eventLogEntity;
        private readonly GemEntity _gemEntity;
        private readonly ReactiveProperty<int> _gem;

        public GemUseCase(EventLogEntity eventLogEntity, GemEntity gemEntity)
        {
            _eventLogEntity = eventLogEntity;
            _gemEntity = gemEntity;
            _gem = new ReactiveProperty<int>(0);
        }

        public Observable<int> gem => _gem;

        public void SetUp()
        {
            _gemEntity.Reset();
            _gem.Value = _gemEntity.value;
        }

        public void Add(int value)
        {
            _gemEntity.Add(value);
            _gem.Value = _gemEntity.value;

            _eventLogEntity.Add(EventLogType.Gem, value);
        }

        void IDisposable.Dispose()
        {
            _gem?.Dispose();
        }
    }
}