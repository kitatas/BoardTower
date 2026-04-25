using BoardTower.Common.Data.DataStore;
using BoardTower.Common.Domain.Ports;
using BoardTower.Common.Domain.Repository;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Common.Presentation.Facade;
using BoardTower.Common.Presentation.Presenter;
using BoardTower.Common.Presentation.View;
using BoardTower.Common.Utility;
using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace BoardTower.Common.Installer
{
    public sealed class CommonInstaller : LifetimeScope
    {
        [SerializeField] private BgmTable bgmTable = default;
        [SerializeField] private SeTable seTable = default;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterMessagePipe();

            // DataStore
            builder.RegisterInstance<BgmTable>(bgmTable);
            builder.RegisterInstance<SeTable>(seTable);

            // Ports
            builder.Register<ExceptionPorts>(Lifetime.Singleton);
            builder.Register<LoadingPorts>(Lifetime.Singleton);

            // Repository
            builder.Register<SoundRepository>(Lifetime.Singleton);

            // UseCase
            builder.Register<BgmUseCase>(Lifetime.Singleton);
            builder.Register<ExceptionUseCase>(Lifetime.Singleton);
            builder.Register<LoadingUseCase>(Lifetime.Singleton);
            builder.Register<SceneUseCase>(Lifetime.Singleton);
            builder.Register<SeUseCase>(Lifetime.Singleton);

            // Presenter
            builder.UseEntryPoints(Lifetime.Singleton, entryPoints =>
            {
                entryPoints.Add<BgmPresenter>();
                entryPoints.Add<ExceptionPresenter>();
                entryPoints.Add<LoadingPresenter>();
                entryPoints.Add<ScenePresenter>();
                entryPoints.Add<SePresenter>();
            });

            // Facade
            builder.Register<BgmFacade>(Lifetime.Singleton);
            builder.Register<ExceptionFacade>(Lifetime.Singleton);
            builder.Register<LoadingFacade>(Lifetime.Singleton);
            builder.Register<SceneFacade>(Lifetime.Singleton);
            builder.Register<SeFacade>(Lifetime.Singleton);

            // View
            builder.RegisterFindFirstObjectByType<SoundView>();
            builder.RegisterFindFirstObjectByType<ExceptionView>();
            builder.RegisterFindFirstObjectByType<LoadingView>();
            builder.RegisterFindFirstObjectByType<TransitionView>();
        }
    }
}