using System;
using BoardTower.Game.Data.Entity;
using R3;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class GemUseCase : IDisposable
    {
        private readonly GemEntity _gemEntity;
        private readonly ReactiveProperty<int> _gem;

        public GemUseCase(GemEntity gemEntity)
        {
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
        }

        void IDisposable.Dispose()
        {
            _gem?.Dispose();
        }
    }
}