using System;
using BoardTower.Game.Data.Entity;
using R3;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class RoundUseCase : IDisposable
    {
        private readonly RoundEntity _roundEntity;
        private readonly ReactiveProperty<int> _round;

        public RoundUseCase(RoundEntity roundEntity)
        {
            _roundEntity = roundEntity;
            _round = new ReactiveProperty<int>(0);
        }

        public Observable<int> round => _round;

        public void Init()
        {
            _roundEntity.Set(0);
            _round.Value = _roundEntity.value;
        }

        public void Increment()
        {
            _roundEntity.Add(1);
            _round.Value = _roundEntity.value;
        }

        void IDisposable.Dispose()
        {
            _round?.Dispose();
        }
    }
}