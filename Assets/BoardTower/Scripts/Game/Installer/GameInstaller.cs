using BoardTower.Game.Data.DataStore;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.Ports;
using BoardTower.Game.Domain.Repository;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Game.Presentation.Facade;
using BoardTower.Game.Presentation.Presenter;
using BoardTower.Game.Presentation.State;
using BoardTower.Game.Presentation.View;
using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace BoardTower.Game.Installer
{
    public sealed class GameInstaller : LifetimeScope
    {
        [SerializeField] private TextAsset memoryFile = default;
        [SerializeField] private SquareEventTable squareEventTable = default;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterMessagePipe();

            // DataStore
            builder.RegisterInstance<MemoryDatabase>(new MemoryDatabase(memoryFile.bytes));
            builder.RegisterInstance<SquareEventTable>(squareEventTable);

            // Entity
            builder.Register<BoardEntity>(Lifetime.Scoped);
            builder.Register<ChessmenEntity>(Lifetime.Scoped);
            builder.Register<GameStateEntity>(Lifetime.Scoped);
            builder.Register<GemEntity>(Lifetime.Scoped);
            builder.Register<PlyEntity>(Lifetime.Scoped);
            builder.Register<RoundEntity>(Lifetime.Scoped);

            // Ports
            builder.Register<BoardPorts>(Lifetime.Scoped);
            builder.Register<ChessmenPorts>(Lifetime.Scoped);
            builder.Register<EventPorts>(Lifetime.Scoped);
            builder.Register<GameStatePorts>(Lifetime.Scoped);
            builder.Register<MovementPorts>(Lifetime.Scoped);

            // Repository
            builder.Register<BoardPatternRepository>(Lifetime.Scoped);
            builder.Register<ChessmenMovementRepository>(Lifetime.Scoped);
            builder.Register<SquareEventRepository>(Lifetime.Scoped);

            // UseCase
            builder.Register<BoardUseCase>(Lifetime.Scoped);
            builder.Register<ChessmenUseCase>(Lifetime.Scoped);
            builder.Register<EventUseCase>(Lifetime.Scoped);
            builder.Register<GameStateUseCase>(Lifetime.Scoped);
            builder.Register<GemUseCase>(Lifetime.Scoped);
            builder.Register<MovementUseCase>(Lifetime.Scoped);
            builder.Register<PlyUseCase>(Lifetime.Scoped);
            builder.Register<RoundUseCase>(Lifetime.Scoped);

            // State
            builder.Register<BaseGameState, GameEventState>(Lifetime.Scoped);
            builder.Register<BaseGameState, GameInitState>(Lifetime.Scoped);
            builder.Register<BaseGameState, GameInputState>(Lifetime.Scoped);
            builder.Register<BaseGameState, GameJudgeState>(Lifetime.Scoped);
            builder.Register<BaseGameState, GameSetUpState>(Lifetime.Scoped);

            // Presenter
            builder.UseEntryPoints(Lifetime.Scoped, entryPoints =>
            {
                entryPoints.Add<BoardPresenter>();
                entryPoints.Add<ChessmenPresenter>();
                entryPoints.Add<GameStatePresenter>();
                entryPoints.Add<GemPresenter>();
                entryPoints.Add<PlyPresenter>();
                entryPoints.Add<RoundPresenter>();
            });

            // Facade
            builder.Register<BoardFacade>(Lifetime.Scoped);
            builder.Register<ChessmenFacade>(Lifetime.Scoped);
            builder.Register<GemFacade>(Lifetime.Scoped);
            builder.Register<PlyFacade>(Lifetime.Scoped);
            builder.Register<RoundFacade>(Lifetime.Scoped);

            // View
            builder.RegisterComponentInHierarchy<BoardView>();
            builder.RegisterComponentInHierarchy<ChessmenView>();
            builder.RegisterComponentInHierarchy<GemView>();
            builder.RegisterComponentInHierarchy<PlyView>();
            builder.RegisterComponentInHierarchy<RoundView>();
        }
    }
}