using BoardTower.Boot.Data.DataStore;
using BoardTower.Boot.Data.Entity;
using BoardTower.Boot.Domain.Ports;
using BoardTower.Boot.Domain.Repository;
using BoardTower.Boot.Domain.UseCase;
using BoardTower.Boot.Presentation.Facade;
using BoardTower.Boot.Presentation.Presenter;
using BoardTower.Boot.Presentation.State;
using BoardTower.Boot.Presentation.View;
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
            builder.Register<BootStateEntity>(Lifetime.Scoped);

            // Ports
            builder.Register<BootStatePorts>(Lifetime.Scoped);
            builder.Register<SplashPorts>(Lifetime.Scoped);

            // Repository
            builder.Register<SplashRepository>(Lifetime.Scoped);

            // UseCase
            builder.Register<BootStateUseCase>(Lifetime.Scoped);
            builder.Register<SplashUseCase>(Lifetime.Scoped);

            // Facade
            builder.Register<SplashFacade>(Lifetime.Scoped);

            // State
            builder.Register<BaseBootState, BootInitState>(Lifetime.Scoped);
            builder.Register<BaseBootState, BootLoadState>(Lifetime.Scoped);

            // Presenter
            builder.UseEntryPoints(Lifetime.Scoped, entryPoints =>
            {
                entryPoints.Add<BootStatePresenter>();
                entryPoints.Add<SplashPresenter>();
            });

            // View
            builder.RegisterComponentInHierarchy<SplashView>();
        }
    }
}