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
        private readonly LotRelicFacade _lotRelicFacade;
        private readonly SelectRelicFacade _selectRelicFacade;
        private readonly CompositeDisposable _disposable;

        public LotRelicPresenter(LotRelicUseCase lotRelicUseCase, LotRelicFacade lotRelicFacade,
            SelectRelicFacade selectRelicFacade)
        {
            _lotRelicUseCase = lotRelicUseCase;
            _lotRelicFacade = lotRelicFacade;
            _selectRelicFacade = selectRelicFacade;
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
                .Subscribe(_selectRelicFacade.SetPosition)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}