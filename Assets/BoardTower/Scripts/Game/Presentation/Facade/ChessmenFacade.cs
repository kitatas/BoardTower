using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Presentation.View;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Presentation.Facade
{
    public sealed class ChessmenFacade
    {
        private readonly ChessmenView _chessmenView;

        public ChessmenFacade(ChessmenView chessmenView)
        {
            _chessmenView = chessmenView;
        }

        public UniTask FadeAsync(ChessmenTransitionVO transition, CancellationToken token)
        {
            var tween = transition.fade switch
            {
                Fade.In => _chessmenView.FadeIn(transition.duration),
                Fade.Out => _chessmenView.FadeOut(transition.duration),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_FADE),
            };

            return tween
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);
        }
    }
}