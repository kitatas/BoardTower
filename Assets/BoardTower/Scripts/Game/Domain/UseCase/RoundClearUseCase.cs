using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.Repository;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class RoundClearUseCase
    {
        private readonly GemEntity _gemEntity;
        private readonly RoundEntity _roundEntity;
        private readonly RoundClearRepository _roundClearRepository;

        public RoundClearUseCase(GemEntity gemEntity, RoundEntity roundEntity,
            RoundClearRepository roundClearRepository)
        {
            _gemEntity = gemEntity;
            _roundEntity = roundEntity;
            _roundClearRepository = roundClearRepository;
        }

        public bool IsClear()
        {
            var roundClear = _roundClearRepository.Find(_roundEntity.value);
            return roundClear.gemCount <= _gemEntity.value;
        }
    }
}