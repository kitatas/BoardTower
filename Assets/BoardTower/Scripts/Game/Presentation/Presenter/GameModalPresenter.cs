using BoardTower.Common.Presentation.Presenter;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Game.Presentation.Facade;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class GameModalPresenter : BaseModalPresenter<GameModalType>
    {
        public GameModalPresenter(GameModalUseCase modalUseCase, GameModalFacade modalFacade)
            : base(modalUseCase, modalFacade)
        {
        }
    }
}