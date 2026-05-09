using System;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.Repository;
using R3;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class RoundClearUseCase : IDisposable
    {
        private readonly GemEntity _gemEntity;
        private readonly RoundClearGemEntity  _roundClearGemEntity;
        private readonly RoundRepository _roundRepository;
        private readonly ReactiveProperty<int> _roundClear;

        public RoundClearUseCase(GemEntity gemEntity, RoundClearGemEntity roundClearGemEntity,
            RoundRepository roundRepository)
        {
            _gemEntity = gemEntity;
            _roundClearGemEntity = roundClearGemEntity;
            _roundRepository = roundRepository;
            _roundClear = new ReactiveProperty<int>(0);
        }

        public Observable<int> roundClear => _roundClear;

        public void SetUp(int round)
        {
            var gemCount = round > 0 ? _roundRepository.Find(round)?.gemCount ?? 0 : 0; 
            _roundClearGemEntity.Set(gemCount);
            _roundClear.Value = _roundClearGemEntity.value;
        }

        public bool IsClear()
        {
            return _roundClearGemEntity.value <= _gemEntity.value;
        }

        void IDisposable.Dispose()
        {
            _roundClear?.Dispose();
        }
    }
}