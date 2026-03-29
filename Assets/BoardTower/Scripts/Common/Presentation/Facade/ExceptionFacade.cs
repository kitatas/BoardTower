using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Presentation.View;
using Cysharp.Threading.Tasks;

namespace BoardTower.Common.Presentation.Facade
{
    public sealed class ExceptionFacade
    {
        private readonly ExceptionView _exceptionView;

        public ExceptionFacade(ExceptionView exceptionView)
        {
            _exceptionView = exceptionView;
        }

        public UniTask RenderAsync(ExceptionVO exception, CancellationToken token)
        {
            return _exceptionView.Render(exception.message, ExceptionConfig.FADE_DURATION)
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);
        }
    }
}