using System.Collections.Generic;
using BoardTower.Common.Presentation.Facade;
using BoardTower.Game.Application;
using BoardTower.Game.Presentation.View.Modal;

namespace BoardTower.Game.Presentation.Facade
{
    public sealed class GameModalFacade : BaseModalFacade<GameModalType>
    {
        public GameModalFacade(IEnumerable<BaseGameModalView> modals) : base(modals)
        {
        }
    }
}