using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Game.Presentation.Presenter;
using BoardTower.Game.Presentation.State;
using BoardTower.Game.Presentation.View;
using MessagePipe;
using VContainer;
using VContainer.Unity;

namespace BoardTower.Game.Installer
{
    public sealed class GameInstaller : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterMessagePipe();

            // Entity
            builder.Register<GameStateEntity>(Lifetime.Scoped);

            // UseCase
            builder.Register<BoardUseCase>(Lifetime.Scoped);
            builder.Register<GameStateUseCase>(Lifetime.Scoped);

            // State
            builder.Register<BaseGameState, GameInitState>(Lifetime.Scoped);

            // Presenter
            builder.UseEntryPoints(Lifetime.Scoped, entryPoints =>
            {
                entryPoints.Add<BoardPresenter>();
                entryPoints.Add<GameStatePresenter>();
            });

            // View
            builder.RegisterComponentInHierarchy<BoardView>();
        }
    }
}