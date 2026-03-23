using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Presentation.View;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Presentation.Facade
{
    public sealed class FinishFacade
    {
        private readonly FinishView _finishView;

        public FinishFacade(FinishView finishView)
        {
            _finishView = finishView;
        }

        public UniTask FadeAsync(FinishVO finish, CancellationToken token)
        {
            var tween = finish.transition.fade switch
            {
                Fade.In => _finishView.FadeIn(finish.type, finish.transition.duration),
                Fade.Out => _finishView.FadeOut(finish.type, finish.transition.duration),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_FADE),
            };

            return tween
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);
        }
    }
}