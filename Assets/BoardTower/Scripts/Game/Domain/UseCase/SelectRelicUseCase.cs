using System;
using BoardTower.Game.Application;
using R3;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class SelectRelicUseCase : IDisposable
    {
        private readonly BehaviorSubject<SelectRelicVO> _selectRelic;
        private readonly BehaviorSubject<SelectRelicVO> _decisionRelic;

        public SelectRelicUseCase()
        {
            _selectRelic = new BehaviorSubject<SelectRelicVO>(null);
            _decisionRelic = new BehaviorSubject<SelectRelicVO>(null);
        }

        public Observable<SelectRelicVO> selectRelic => _selectRelic.Where(x => x != null);
        public Observable<SelectRelicVO> decisionRelic => _decisionRelic.Where(x => x != null);

        public void Select(SelectRelicVO relic)
        {
            if (_selectRelic.Value == null)
            {
                // 初回選択
                _selectRelic?.OnNext(relic);
            }
            else if (_selectRelic.Value.index == relic.index)
            {
                // 選択済みが再度選択された場合は決定
                _selectRelic?.OnNext(null);
                _decisionRelic?.OnNext(relic);
            }
            else
            {
                // 他を選択
                _selectRelic?.OnNext(relic);
            }
        }

        void IDisposable.Dispose()
        {
            _selectRelic?.Dispose();
            _decisionRelic?.Dispose();
        }
    }
}