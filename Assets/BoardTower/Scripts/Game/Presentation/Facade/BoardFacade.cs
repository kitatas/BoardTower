using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Presentation.View;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Presentation.Facade
{
    public sealed class BoardFacade
    {
        private readonly BoardView _boardView;

        public BoardFacade(BoardView boardView)
        {
            _boardView = boardView;
        }

        public UniTask FadeAsync(BoardTransitionVO transition, CancellationToken token)
        {
            return transition.fade switch
            {
                Fade.In => _boardView.FadeInAsync(transition.duration, token),
                Fade.Out => _boardView.FadeOutAsync(transition.duration, token),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_FADE),
            };
        }
    }
}