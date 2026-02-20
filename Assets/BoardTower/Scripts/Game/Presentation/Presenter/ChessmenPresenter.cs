using System;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Game.Presentation.Facade;
using MessagePipe;
using VContainer.Unity;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class ChessmenPresenter : IStartable, IDisposable
    {
        private readonly ChessmenUseCase _chessmenUseCase;
        private readonly ChessmenFacade _chessmenFacade;
        private IDisposable _subscription;

        public ChessmenPresenter(ChessmenUseCase chessmenUseCase, ChessmenFacade chessmenFacade)
        {
            _chessmenUseCase = chessmenUseCase;
            _chessmenFacade = chessmenFacade;
        }

        void IStartable.Start()
        {
            _subscription = _chessmenUseCase.subscriber
                .Subscribe(async (t, ct) =>
                {
                    await _chessmenFacade.FadeAsync(t, ct);
                });
        }

        void IDisposable.Dispose()
        {
            _subscription?.Dispose();
        }
    }
}