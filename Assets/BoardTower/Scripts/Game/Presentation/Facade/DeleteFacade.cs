using BoardTower.Game.Presentation.View;
using R3;

namespace BoardTower.Game.Presentation.Facade
{
    public sealed class DeleteFacade
    {
        private readonly DeleteView _deleteView;

        public DeleteFacade(DeleteView deleteView)
        {
            _deleteView = deleteView;
        }

        public Observable<Unit> clickDecision => _deleteView.clickDecision;
        public Observable<Unit> clickBackTitle => _deleteView.clickBackTitle;
    }
}