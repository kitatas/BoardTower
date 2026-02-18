using System;
using BoardTower.Common.Application;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Game.Presentation.View;
using MessagePipe;
using VContainer.Unity;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class BoardPresenter : IStartable, IDisposable
    {
        private readonly BoardUseCase _boardUseCase;
        private readonly BoardView _boardView;
        private IDisposable _subscription;

        public BoardPresenter(BoardUseCase boardUseCase, BoardView boardView)
        {
            _boardUseCase = boardUseCase;
            _boardView = boardView;
        }

        void IStartable.Start()
        {
            _subscription = _boardUseCase.subscriber
                .Subscribe(async (t, ct) =>
                {
                    await (t.fade switch
                    {
                        Fade.In => _boardView.FadeInAsync(t.duration, ct),
                        Fade.Out => _boardView.FadeOutAsync(t.duration, ct),
                        _ => throw new Exception(), // TODO: Exception
                    });
                });
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}