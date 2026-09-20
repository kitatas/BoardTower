using BoardTower.Boot.Data.DataStore;
using BoardTower.Boot.Data.Entity;
using BoardTower.Boot.Domain.Ports;
using BoardTower.Boot.Domain.Repository;
using BoardTower.Boot.Domain.UseCase;
using BoardTower.Boot.Presentation.Facade;
using BoardTower.Boot.Presentation.Presenter;
using BoardTower.Boot.Presentation.State;
using BoardTower.Boot.Presentation.View;
using BoardTower.Boot.Presentation.View.Button;
using BoardTower.Boot.Presentation.View.Modal;
using BoardTower.Common.Presentation.Facade;
using BoardTower.Common.Presentation.Presenter;
using BoardTower.Common.Presentation.View.Button;
using BoardTower.Common.Utility;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace BoardTower.Boot.Installer
{
    public sealed class BootInstaller : LifetimeScope
    {
        [SerializeField] private SplashTable splashTable = default;

        protected override void Configure(IContainerBuilder builder)
        {
            // DataStore
            builder.RegisterInstance<SplashTable>(splashTable);

            // Entity
            builder.Register<BootModalEntity>(Lifetime.Scoped);
            builder.Register<BootStateEntity>(Lifetime.Scoped);

            // Ports
            builder.Register<BootModalPorts>(Lifetime.Scoped);
            builder.Register<BootStatePorts>(Lifetime.Scoped);
            builder.Register<SplashPorts>(Lifetime.Scoped);

            // Repository
            builder.Register<AppVersionRepository>(Lifetime.Scoped);
            builder.Register<SplashRepository>(Lifetime.Scoped);

            // UseCase
            builder.Register<AppVersionUseCase>(Lifetime.Scoped);
            builder.Register<BootModalUseCase>(Lifetime.Scoped);
            builder.Register<BootStateUseCase>(Lifetime.Scoped);
            builder.Register<DisplayNameUseCase>(Lifetime.Scoped);
            builder.Register<LoginUseCase>(Lifetime.Scoped);
            builder.Register<ResourceUseCase>(Lifetime.Scoped);
            builder.Register<SplashUseCase>(Lifetime.Scoped);

            // Facade
            builder.Register<BootModalFacade>(Lifetime.Scoped);
            builder.Register<ButtonFacade>(Lifetime.Scoped);
            builder.Register<DisplayNameFacade>(Lifetime.Scoped);
            builder.Register<GameModeFacade>(Lifetime.Scoped);
            builder.Register<SplashFacade>(Lifetime.Scoped);

            // State
            builder.Register<BaseBootState, BootInitState>(Lifetime.Scoped);
            builder.Register<BaseBootState, BootLoadState>(Lifetime.Scoped);
            builder.Register<BaseBootState, BootLoginState>(Lifetime.Scoped);
            builder.Register<BaseBootState, BootSplashState>(Lifetime.Scoped);

            // Presenter
            builder.UseEntryPoints(Lifetime.Scoped, entryPoints =>
            {
                entryPoints.Add<BootModalPresenter>();
                entryPoints.Add<BootStatePresenter>();
                entryPoints.Add<ButtonPresenter>();
                entryPoints.Add<DisplayNamePresenter>();
                entryPoints.Add<GameModePresenter>();
                entryPoints.Add<SplashPresenter>();
            });

            // View
            builder.RegisterFindObjectsByType<BaseButtonView>();
            builder.RegisterFindObjectsByType<BootModalButtonView>();
            builder.RegisterFindObjectsByType<BaseBootModalView>();
            builder.RegisterComponentInHierarchy<DisplayNameModalView>();
            builder.RegisterComponentInHierarchy<GameModeView>();
            builder.RegisterComponentInHierarchy<SplashView>();
            builder.RegisterComponentInHierarchy<UpdateView>();
        }
    }
}