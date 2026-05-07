using System;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Game.Presentation.Facade;
using MessagePipe;
using R3;
using VContainer.Unity;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class LotRelicPresenter : IStartable, IDisposable
    {
        private readonly LotRelicUseCase _lotRelicUseCase;
        private readonly SelectRelicUseCase _selectRelicUseCase;
        private readonly LotRelicFacade _lotRelicFacade;
        private readonly CompositeDisposable _disposable;

        public LotRelicPresenter(LotRelicUseCase lotRelicUseCase, SelectRelicUseCase selectRelicUseCase,
            LotRelicFacade lotRelicFacade)
        {
            _lotRelicUseCase = lotRelicUseCase;
            _selectRelicUseCase = selectRelicUseCase;
            _lotRelicFacade = lotRelicFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _lotRelicUseCase.lotRelic
                .Subscribe(_lotRelicFacade.Render)
                .AddTo(_disposable);

            _lotRelicUseCase.transition
                .Subscribe((r, ct) => _lotRelicFacade.FadeAsync(r, ct))
                .AddTo(_disposable);

            _lotRelicFacade.OnClickAnyAsObservable()
                .Subscribe(_selectRelicUseCase.Select)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}