using System;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Game.Presentation.Facade;
using MessagePipe;
using R3;
using VContainer.Unity;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class EventLogPresenter : IStartable, IDisposable
    {
        private readonly EventLogUseCase _eventLogUseCase;
        private readonly EventLogFacade _eventLogFacade;
        private readonly CompositeDisposable _disposable;

        public EventLogPresenter(EventLogUseCase eventLogUseCase, EventLogFacade eventLogFacade)
        {
            _eventLogUseCase = eventLogUseCase;
            _eventLogFacade = eventLogFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _eventLogUseCase.eventLogMessage
                .Subscribe(_eventLogFacade.RenderAsync)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}