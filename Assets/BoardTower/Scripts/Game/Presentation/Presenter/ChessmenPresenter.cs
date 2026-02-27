using System;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Game.Presentation.Facade;
using MessagePipe;
using R3;
using VContainer.Unity;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class ChessmenPresenter : IStartable, IDisposable
    {
        private readonly ChessmenUseCase _chessmenUseCase;
        private readonly ChessmenFacade _chessmenFacade;
        private readonly CompositeDisposable _disposable;

        public ChessmenPresenter(ChessmenUseCase chessmenUseCase, ChessmenFacade chessmenFacade)
        {
            _chessmenUseCase = chessmenUseCase;
            _chessmenFacade = chessmenFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _chessmenUseCase.transition
                .Subscribe((t, ct) => _chessmenFacade.FadeAsync(t, ct))
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}