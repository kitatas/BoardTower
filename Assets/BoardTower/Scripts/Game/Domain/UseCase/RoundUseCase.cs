using System;
using BoardTower.Game.Application;
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
            _roundEntity.Set(0);
            _round = new ReactiveProperty<int>(_roundEntity.value);
        }

        public Observable<int> round => _round;

        public void Init()
        {
            _roundEntity.Reset();
            _round.Value = _roundEntity.value;
        }

        public void Increment()
        {
            _roundEntity.Add(1);
            _round.Value = _roundEntity.value;
        }

        public bool IsMaxRound()
        {
            return _roundEntity.value == RoundConfig.MAX_NUM;
        }

        void IDisposable.Dispose()
        {
            _round?.Dispose();
        }
    }
}