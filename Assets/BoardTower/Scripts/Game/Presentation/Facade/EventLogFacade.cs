using System.Threading;
using BoardTower.Game.Application;
using BoardTower.Game.Presentation.View;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Presentation.Facade
{
    public sealed class EventLogFacade
    {
        private readonly EventLogView _eventLogView;

        public EventLogFacade(EventLogView eventLogView)
        {
            _eventLogView = eventLogView;
        }

        public async UniTask RenderAsync(EventLogMessageVO eventLogMessage, CancellationToken token)
        {
            await _eventLogView.Tween(eventLogMessage)
                .ToUniTask(TweenCancelBehaviour.KillWithCompleteCallbackAndCancelAwait, token);
        }
    }
}