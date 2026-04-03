using System.Collections.Generic;
using BoardTower.Boot.Application;
using BoardTower.Boot.Domain.UseCase;
using BoardTower.Boot.Presentation.State;
using BoardTower.Common.Presentation.Presenter;

namespace BoardTower.Boot.Presentation.Presenter
{
    public sealed class BootStatePresenter : BaseStatePresenter<BootState>
    {
        public BootStatePresenter(BootStateUseCase stateUseCase, IEnumerable<BaseBootState> states) : base(stateUseCase, states)
        {
        }
    }
}