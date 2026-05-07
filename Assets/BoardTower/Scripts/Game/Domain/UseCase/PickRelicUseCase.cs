using System;
using System.Linq;
using System.Threading;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.Ports;
using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class PickRelicUseCase : IDisposable
    {
        private readonly LotRelicEntity _lotRelicEntity;
        private readonly PickRelicEntity _pickRelicEntity;
        private readonly PickRelicPorts _pickRelicPorts;
        private readonly Subject<RelicVO> _pick;

        public PickRelicUseCase(LotRelicEntity lotRelicEntity, PickRelicEntity pickRelicEntity,
            PickRelicPorts pickRelicPorts)
        {
            _lotRelicEntity = lotRelicEntity;
            _pickRelicEntity = pickRelicEntity;
            _pickRelicPorts = pickRelicPorts;
            _pick = new Subject<RelicVO>();
        }

        public IAsyncSubscriber<PickRelicVO> pickRelic => _pickRelicPorts.pickRelicSubscriber;

        public UniTask InitAsync(CancellationToken token)
        {
            _pickRelicEntity.Set(PickRelicVO.Empty());
            return _pickRelicPorts.PublishPickRelicAsync(_pickRelicEntity.value, token);
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
            await _pickRelicPorts.PublishPickRelicAsync(_pickRelicEntity.value, token);
        }

        void IDisposable.Dispose()
        {
            _pick?.Dispose();
        }
    }
}