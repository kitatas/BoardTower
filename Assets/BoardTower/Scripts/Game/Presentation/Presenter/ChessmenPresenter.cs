using System;
using BoardTower.Common.Application;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Game.Presentation.View;
using Cysharp.Threading.Tasks;
using MessagePipe;
using VContainer.Unity;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class ChessmenPresenter : IStartable, IDisposable
    {
        private readonly ChessmenUseCase _chessmenUseCase;
        private readonly ChessmenView _chessmenView;
        private IDisposable _subscription;

        public ChessmenPresenter(ChessmenUseCase chessmenUseCase, ChessmenView chessmenView)
        {
            _chessmenUseCase = chessmenUseCase;
            _chessmenView = chessmenView;
        }

        void IStartable.Start()
        {
            _subscription = _chessmenUseCase.subscriber
                .Subscribe(async (t, ct) =>
                {
                    await (t.fade switch
                    {
                        Fade.In => _chessmenView.FadeIn(t.duration)
                            .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, ct),
                        Fade.Out => _chessmenView.FadeOut(t.duration)
                            .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, ct),
                        _ => throw new Exception(), // TODO: Exception
                    });
                });
        }

        void IDisposable.Dispose()
        {
            _subscription?.Dispose();
        }
    }
}