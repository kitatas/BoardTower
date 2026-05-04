using System.Linq;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.Repository;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class LotRelicUseCase
    {
        private readonly LotRelicEntity _lotRelicEntity;
        private readonly RelicRepository _relicRepository;

        public LotRelicUseCase(LotRelicEntity lotRelicEntity, RelicRepository relicRepository)
        {
            _lotRelicEntity = lotRelicEntity;
            _relicRepository = relicRepository;
        }

        public void Lot()
        {
            // TODO: 保持済み・抽選済みの重複回避
            var relics = Enumerable.Range(1, 4)
                .Select(x => _relicRepository.Find((RelicType)x));

            _lotRelicEntity.Set(new LotRelicVO(relics));
        }
    }
}