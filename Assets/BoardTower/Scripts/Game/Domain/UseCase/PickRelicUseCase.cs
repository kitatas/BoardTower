using System;
using System.Linq;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using R3;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class PickRelicUseCase : IDisposable
    {
        private readonly LotRelicEntity _lotRelicEntity;
        private readonly Subject<RelicVO> _pick;

        public PickRelicUseCase(LotRelicEntity lotRelicEntity)
        {
            _lotRelicEntity = lotRelicEntity;
            _pick = new Subject<RelicVO>();
        }

        public void HandlePick(SelectRelicVO selectRelic)
        {
            var relic = _lotRelicEntity.value.relics.ElementAt(selectRelic.index);
            _pick?.OnNext(relic);
        }

        void IDisposable.Dispose()
        {
            _pick?.Dispose();
        }
    }
}