using System;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Game.Presentation.Facade;
using R3;
using VContainer.Unity;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class LotRelicPresenter : IStartable, IDisposable
    {
        private readonly LotRelicUseCase _lotRelicUseCase;
        private readonly LotRelicFacade _lotRelicFacade;
        private readonly CompositeDisposable _disposable;

        public LotRelicPresenter(LotRelicUseCase lotRelicUseCase, LotRelicFacade lotRelicFacade)
        {
            _lotRelicUseCase = lotRelicUseCase;
            _lotRelicFacade = lotRelicFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _lotRelicUseCase.lotRelic
                .Subscribe(_lotRelicFacade.Render)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}