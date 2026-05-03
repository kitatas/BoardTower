using BoardTower.Common.Domain.UseCase;
using BoardTower.Game.Presentation.Facade;
using VContainer.Unity;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class UserPresenter : IStartable
    {
        private readonly UserDataUseCase _userDataUseCase;
        private readonly UserFacade _userFacade;

        public UserPresenter(UserDataUseCase userDataUseCase, UserFacade userFacade)
        {
            _userDataUseCase = userDataUseCase;
            _userFacade = userFacade;
        }

        void IStartable.Start()
        {
            _userFacade.Render(_userDataUseCase.user);
        }
    }
}