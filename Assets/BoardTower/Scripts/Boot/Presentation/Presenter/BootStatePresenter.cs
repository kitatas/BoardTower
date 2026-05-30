using System.Collections.Generic;
using BoardTower.Boot.Application;
using BoardTower.Boot.Domain.UseCase;
using BoardTower.Boot.Presentation.State;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Common.Presentation.Presenter;

namespace BoardTower.Boot.Presentation.Presenter
{
    public sealed class BootStatePresenter : BaseStatePresenter<BootState>
    {
        public BootStatePresenter(ExceptionUseCase exceptionUseCase, BootStateUseCase stateUseCase,
            IEnumerable<BaseBootState> states) : base(exceptionUseCase, stateUseCase, states)
        {
        }
    }
}