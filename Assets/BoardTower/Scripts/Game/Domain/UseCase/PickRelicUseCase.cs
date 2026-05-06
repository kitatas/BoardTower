using System;
using System.Linq;
using System.Threading;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using Cysharp.Threading.Tasks;
using R3;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class PickRelicUseCase : IDisposable
    {
        private readonly LotRelicEntity _lotRelicEntity;
        private readonly PickRelicEntity _pickRelicEntity;
        private readonly Subject<RelicVO> _pick;

        public PickRelicUseCase(LotRelicEntity lotRelicEntity, PickRelicEntity pickRelicEntity)
        {
            _lotRelicEntity = lotRelicEntity;
            _pickRelicEntity = pickRelicEntity;
            _pick = new Subject<RelicVO>();
        }

        public void HandlePick(SelectRelicVO selectRelic)
        {
            var relic = _lotRelicEntity.value.relics.ElementAt(selectRelic.index);
            _pick?.OnNext(relic);
        }

        public async UniTask PickAsync(CancellationToken token)
        {
            var relic = await _pick.FirstAsync(cancellationToken: token);
            _pickRelicEntity.Add(relic);
        }

        void IDisposable.Dispose()
        {
            _pick?.Dispose();
        }
    }
}