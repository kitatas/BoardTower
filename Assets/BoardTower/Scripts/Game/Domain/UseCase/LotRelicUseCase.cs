using System;
using System.Linq;
using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.Ports;
using BoardTower.Game.Domain.Repository;
using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class LotRelicUseCase : IDisposable
    {
        private readonly LotRelicEntity _lotRelicEntity;
        private readonly LotRelicPorts _lotRelicPorts;
        private readonly RelicRepository _relicRepository;
        private readonly Subject<LotRelicVO> _lotRelic;

        public LotRelicUseCase(LotRelicEntity lotRelicEntity, LotRelicPorts lotRelicPorts,
            RelicRepository relicRepository)
        {
            _lotRelicEntity = lotRelicEntity;
            _lotRelicPorts = lotRelicPorts;
            _relicRepository = relicRepository;
            _lotRelic = new Subject<LotRelicVO>();
        }

        public IAsyncSubscriber<LotRelicTransitionVO> transition => _lotRelicPorts.lotRelicTransitionSubscriber;
        public Observable<LotRelicVO> lotRelic => _lotRelic;

        public UniTask InitAsync(CancellationToken token)
        {
            var lotRelicTransition = LotRelicTransitionVO.Create(Fade.Out, 0.0f);
            return _lotRelicPorts.PublishLotRelicTransitionAsync(lotRelicTransition, token);
        }

        public UniTask FadeAsync(Fade fade, CancellationToken token)
        {
            var lotRelicTransition = LotRelicTransitionVO.Create(fade, RelicConfig.LOT_FADE_DURATION);
            return _lotRelicPorts.PublishLotRelicTransitionAsync(lotRelicTransition, token);
        }

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