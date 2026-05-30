using System;
using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Common.Presentation.Facade;
using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;
using VContainer.Unity;

namespace BoardTower.Common.Presentation.Presenter
{
    public sealed class ExceptionPresenter : IInitializable, IDisposable
    {
        private readonly ExceptionUseCase _exceptionUseCase;
        private readonly SceneUseCase _sceneUseCase;
        private readonly ExceptionFacade _exceptionFacade;
        private readonly CompositeDisposable _disposable;

        public ExceptionPresenter(ExceptionUseCase exceptionUseCase, SceneUseCase sceneUseCase,
            ExceptionFacade exceptionFacade)
        {
            _exceptionUseCase = exceptionUseCase;
            _sceneUseCase = sceneUseCase;
            _exceptionFacade = exceptionFacade;
            _disposable = new CompositeDisposable();
        }

        void IInitializable.Initialize()
        {
            _exceptionUseCase.exceptionNotify
                .Subscribe((e, ct) => _exceptionFacade.FadeAsync(e, ct))
                .AddTo(_disposable);

            _exceptionUseCase.exceptionAction
                .Subscribe((e, ct) =>
                {
                    return e.exception switch
                    {
                        RebootExceptionVO => HandleRebootActionAsync(ct),
                        RetryExceptionVO => UniTask.Yield(ct),
                        _ => HandleQuitActionAsync(ct),
                    };
                })
                .AddTo(_disposable);

            _exceptionFacade.OnDecision()
                .Subscribe(_exceptionUseCase.HandleDecision)
                .AddTo(_disposable);
        }

        private UniTask HandleRebootActionAsync(CancellationToken token)
        {
            _sceneUseCase.Load(SceneName.Boot, LoadType.Fade);
            return UniTask.Yield(token);
        }

        private static UniTask HandleQuitActionAsync(CancellationToken token)
        {
            // QuitException含む意図しないExceptionの場合は強制終了
#if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE
            UnityEngine.Application.Quit();
#else
            // TODO: 上記PF以外の場合
#endif
            return UniTask.Yield(token);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}