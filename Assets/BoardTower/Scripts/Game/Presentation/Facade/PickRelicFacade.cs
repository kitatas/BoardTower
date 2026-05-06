using System.Threading;
using BoardTower.Game.Application;
using BoardTower.Game.Presentation.View;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Presentation.Facade
{
    public sealed class PickRelicFacade
    {
        private readonly PickRelicView _pickRelicView;

        public PickRelicFacade(PickRelicView pickRelicView)
        {
            _pickRelicView = pickRelicView;
        }

        public UniTask RenderAsync(PickRelicVO pickRelic, CancellationToken token)
        {
            _pickRelicView.Render(pickRelic);
            return UniTask.Yield(token);
        }
    }
}