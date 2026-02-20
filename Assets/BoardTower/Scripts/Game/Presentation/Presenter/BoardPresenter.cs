using System;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Game.Presentation.Facade;
using MessagePipe;
using VContainer.Unity;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class BoardPresenter : IStartable, IDisposable
    {
        private readonly BoardUseCase _boardUseCase;
        private readonly BoardFacade  _boardFacade;
        private IDisposable _subscription;

        public BoardPresenter(BoardUseCase boardUseCase, BoardFacade boardFacade)
        {
            _boardUseCase = boardUseCase;
            _boardFacade = boardFacade;
        }

        void IStartable.Start()
        {
            _subscription = _boardUseCase.subscriber
                .Subscribe(async (t, ct) =>
                {
                    await _boardFacade.FadeAsync(t, ct);
                });
        }

        void IDisposable.Dispose()
        {
            _subscription?.Dispose();
        }
    }
}