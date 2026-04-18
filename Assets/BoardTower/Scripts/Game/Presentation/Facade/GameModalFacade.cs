using BoardTower.Common.Presentation.Facade;
using BoardTower.Game.Application;
using BoardTower.Game.Presentation.View.Button;
using BoardTower.Game.Presentation.View.Modal;

namespace BoardTower.Game.Presentation.Facade
{
    public sealed class GameModalFacade : BaseModalFacade<GameModalType>
    {
        public GameModalFacade(GameModalView[] modals, GameModalButtonView[] buttons) : base(modals, buttons)
        {
        }
    }
}