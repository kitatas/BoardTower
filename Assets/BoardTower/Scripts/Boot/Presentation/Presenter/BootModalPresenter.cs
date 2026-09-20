using BoardTower.Boot.Application;
using BoardTower.Boot.Domain.UseCase;
using BoardTower.Boot.Presentation.Facade;
using BoardTower.Common.Presentation.Presenter;

namespace BoardTower.Boot.Presentation.Presenter
{
    public sealed class BootModalPresenter : BaseModalPresenter<BootModalType>
    {
        public BootModalPresenter(BootModalUseCase modalUseCase, BootModalFacade modalFacade) : base(modalUseCase,
            modalFacade)
        {
        }
    }
}