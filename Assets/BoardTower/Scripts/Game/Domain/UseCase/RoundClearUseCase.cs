using System;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.Repository;
using R3;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class RoundClearUseCase : IDisposable
    {
        private readonly GemEntity _gemEntity;
        private readonly RoundEntity _roundEntity;
        private readonly RoundClearGemEntity  _roundClearGemEntity;
        private readonly RoundClearRepository _roundClearRepository;
        private readonly ReactiveProperty<int> _roundClear;

        public RoundClearUseCase(GemEntity gemEntity, RoundEntity roundEntity, RoundClearGemEntity roundClearGemEntity,
            RoundClearRepository roundClearRepository)
        {
            _gemEntity = gemEntity;
            _roundEntity = roundEntity;
            _roundClearGemEntity = roundClearGemEntity;
            _roundClearRepository = roundClearRepository;
            _roundClear = new ReactiveProperty<int>(0);
        }

        public Observable<int> roundClear => _roundClear;

        public void SetUp(int round)
        {
            var vo = _roundClearRepository.Find(round);
            _roundClearGemEntity.Set(vo.gemCount);
            _roundClear.Value = _roundClearGemEntity.value;
        }

        public bool IsClear()
        {
            var vo = _roundClearRepository.Find(_roundEntity.value);
            return vo.gemCount <= _gemEntity.value;
        }

        void IDisposable.Dispose()
        {
            _roundClear?.Dispose();
        }
    }
}