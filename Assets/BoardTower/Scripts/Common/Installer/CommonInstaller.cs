using BoardTower.Common.Domain.UseCase;
using BoardTower.Common.Presentation.Facade;
using BoardTower.Common.Presentation.Presenter;
using BoardTower.Common.Presentation.View;
using VContainer;
using VContainer.Unity;

namespace BoardTower.Common.Installer
{
    public sealed class CommonInstaller : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // UseCase
            builder.Register<SceneUseCase>(Lifetime.Singleton);

            // Presenter
            builder.UseEntryPoints(Lifetime.Singleton, entryPoints =>
            {
                entryPoints.Add<ScenePresenter>();
            });

            // Facade
            builder.Register<SceneFacade>(Lifetime.Singleton);

            // View
            builder.RegisterComponentInHierarchy<TransitionView>();
        }
    }
}