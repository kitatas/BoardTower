using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Game.Presentation.Presenter;
using BoardTower.Game.Presentation.State;
using VContainer;
using VContainer.Unity;

namespace BoardTower.Game.Installer
{
    public sealed class GameInstaller : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // Entity
            builder.Register<GameStateEntity>(Lifetime.Scoped);

            // UseCase
            builder.Register<GameStateUseCase>(Lifetime.Scoped);

            // State
            builder.Register<BaseGameState, GameInitState>(Lifetime.Scoped);

            // Presenter
            builder.UseEntryPoints(Lifetime.Scoped, entryPoints =>
            {
                //
                entryPoints.Add<GameStatePresenter>();
            });
        }
    }
}