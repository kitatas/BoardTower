using System;
using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Game.Presentation.Facade;
using Cysharp.Threading.Tasks;
using R3;
using VContainer.Unity;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class UserPresenter : IStartable, IDisposable
    {
        private readonly ExceptionUseCase _exceptionUseCase;
        private readonly LoadingUseCase _loadingUseCase;
        private readonly UserDataUseCase _userDataUseCase;
        private readonly UserFacade _userFacade;
        private readonly CompositeDisposable _disposable;

        public UserPresenter(ExceptionUseCase exceptionUseCase, LoadingUseCase loadingUseCase,
            UserDataUseCase userDataUseCase, UserFacade userFacade)
        {
            _exceptionUseCase = exceptionUseCase;
            _loadingUseCase = loadingUseCase;
            _userDataUseCase = userDataUseCase;
            _userFacade = userFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _userFacade.OnDecisionDisplayName()
                .SubscribeAwait(async (x, ct) => await UpdateDisplayNameAsync(x, ct))
                .AddTo(_disposable);

            _userFacade.Render(_userDataUseCase.user);
        }

        private async UniTask UpdateDisplayNameAsync(string name, CancellationToken token)
        {
            try
            {
                await _loadingUseCase.FadeAsync(Fade.In, token);

                var userDisplayName = new UserDisplayNameVO(name);
                await _userDataUseCase.UpdateDisplayNameAsync(userDisplayName, token);

                await _loadingUseCase.FadeAsync(Fade.Out, token);
            }
            catch (ExceptionVO e)
            {
                await _loadingUseCase.FadeAsync(Fade.Out, token);
                await _exceptionUseCase.ThrowAsync(e, token);

            }
            catch (Exception e)
            {
                await _loadingUseCase.FadeAsync(Fade.Out, token);
                await _exceptionUseCase.ThrowQuitAsync(ExceptionConfig.UNKNOWN_ERROR, token);
            }
            finally
            {
                _userFacade.Render(_userDataUseCase.user);
            }
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}