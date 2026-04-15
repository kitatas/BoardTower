using System;
using System.Collections.Generic;
using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Presentation.View.Modal;
using Cysharp.Threading.Tasks;

namespace BoardTower.Common.Presentation.Facade
{
    public abstract class BaseModalFacade<T> where T : Enum
    {
        private readonly Dictionary<T, BaseModalView<T>> _modalMap;

        public BaseModalFacade(IEnumerable<BaseModalView<T>> modals)
        {
            _modalMap = new Dictionary<T, BaseModalView<T>>();
            foreach (var m in modals)
            {
                _modalMap.TryAdd(m.type, m);
                m.FadeOut(0.0f);
            }
        }

        public UniTask FadeAsync(BaseModalTransitionVO<T> transition, CancellationToken token)
        {
            if (!_modalMap.TryGetValue(transition.type, out var modal))
                throw new QuitExceptionVO(ExceptionConfig.INVALID_MODAL);

            var tween = transition.transition.fade switch
            {
                Fade.In => modal.FadeIn(transition.transition.duration),
                Fade.Out => modal.FadeOut(transition.transition.duration),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_FADE),
            };

            return tween
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);
        }
    }
}