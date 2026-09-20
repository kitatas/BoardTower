using System.Collections.Generic;
using BoardTower.Boot.Application;
using BoardTower.Boot.Presentation.View.Button;
using BoardTower.Boot.Presentation.View.Modal;
using BoardTower.Common.Presentation.Facade;

namespace BoardTower.Boot.Presentation.Facade
{
    public sealed class BootModalFacade : BaseModalFacade<BootModalType>
    {
        public BootModalFacade(IEnumerable<BaseBootModalView> modals, IEnumerable<BootModalButtonView> buttons) : base(
            modals, buttons)
        {
        }
    }
}