using System;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Game.Presentation.Facade;
using MessagePipe;
using R3;
using VContainer.Unity;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class BoardPresenter : IStartable, IDisposable
    {
        private readonly BoardUseCase _boardUseCase;
        private readonly MovementUseCase _movementUseCase;
        private readonly BoardFacade _boardFacade;
        private readonly CompositeDisposable _disposable;

        public BoardPresenter(BoardUseCase boardUseCase, MovementUseCase movementUseCase, BoardFacade boardFacade)
        {
            _boardUseCase = boardUseCase;
            _movementUseCase = movementUseCase;
            _boardFacade = boardFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _boardUseCase.subscriber
                .Subscribe(async (t, ct) =>
                {
                    await _boardFacade.FadeAsync(t, ct);
                })
                .AddTo(_disposable);

            _movementUseCase.subscriber
                .Subscribe(async (hs, ct) =>
                {
                    await _boardFacade.ShowHighlightAsync(hs, ct);
                })
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}