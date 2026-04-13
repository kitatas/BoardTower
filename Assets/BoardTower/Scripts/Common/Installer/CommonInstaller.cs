using BoardTower.Common.Domain.Ports;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Common.Presentation.Facade;
using BoardTower.Common.Presentation.Presenter;
using BoardTower.Common.Presentation.View;
using BoardTower.Common.Utility;
using MessagePipe;
using VContainer;
using VContainer.Unity;

namespace BoardTower.Common.Installer
{
    public sealed class CommonInstaller : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterMessagePipe();

            // Ports
            builder.Register<ExceptionPorts>(Lifetime.Singleton);
            builder.Register<LoadingPorts>(Lifetime.Singleton);

            // UseCase
            builder.Register<ExceptionUseCase>(Lifetime.Singleton);
            builder.Register<LoadingUseCase>(Lifetime.Singleton);
            builder.Register<SceneUseCase>(Lifetime.Singleton);

            // Presenter
            builder.UseEntryPoints(Lifetime.Singleton, entryPoints =>
            {
                entryPoints.Add<ExceptionPresenter>();
                entryPoints.Add<LoadingPresenter>();
                entryPoints.Add<ScenePresenter>();
            });

            // Facade
            builder.Register<ExceptionFacade>(Lifetime.Singleton);
            builder.Register<LoadingFacade>(Lifetime.Singleton);
            builder.Register<SceneFacade>(Lifetime.Singleton);

            // View
            builder.RegisterFindFirstObjectByType<ExceptionView>();
            builder.RegisterFindFirstObjectByType<LoadingView>();
            builder.RegisterFindFirstObjectByType<TransitionView>();
        }
    }
}