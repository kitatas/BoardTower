using System;
using BoardTower.Common.Application;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Game.Presentation.Facade;
using R3;
using VContainer.Unity;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class UserPresenter : IStartable, IDisposable
    {
        private readonly UserDataUseCase _userDataUseCase;
        private readonly UserFacade _userFacade;
        private readonly CompositeDisposable _disposable;

        public UserPresenter(UserDataUseCase userDataUseCase, UserFacade userFacade)
        {
            _userDataUseCase = userDataUseCase;
            _userFacade = userFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _userFacade.OnDecisionDisplayName()
                .SubscribeAwait(async (x, ct) =>
                {
                    var userDisplayName = new UserDisplayNameVO(x);
                    await _userDataUseCase.UpdateDisplayNameAsync(userDisplayName, ct);
                    _userFacade.Render(_userDataUseCase.user);
                })
                .AddTo(_disposable);

            _userFacade.Render(_userDataUseCase.user);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}