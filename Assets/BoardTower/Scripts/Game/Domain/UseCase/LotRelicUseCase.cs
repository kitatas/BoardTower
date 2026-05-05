using System;
using System.Linq;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.Repository;
using R3;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class LotRelicUseCase : IDisposable
    {
        private readonly LotRelicEntity _lotRelicEntity;
        private readonly RelicRepository _relicRepository;
        private readonly Subject<LotRelicVO> _lotRelic;

        public LotRelicUseCase(LotRelicEntity lotRelicEntity, RelicRepository relicRepository)
        {
            _lotRelicEntity = lotRelicEntity;
            _relicRepository = relicRepository;
            _lotRelic = new Subject<LotRelicVO>();
        }

        public Observable<LotRelicVO> lotRelic => _lotRelic;

        public void Lot()
        {
            // TODO: 保持済み・抽選済みの重複回避
            var relics = Enumerable.Range(1, 4)
                .Select(x => _relicRepository.Find((RelicType)x));

            _lotRelicEntity.Set(new LotRelicVO(relics));
            _lotRelic?.OnNext(_lotRelicEntity.value);
        }

        void IDisposable.Dispose()
        {
            _lotRelic?.Dispose();
        }
    }
}