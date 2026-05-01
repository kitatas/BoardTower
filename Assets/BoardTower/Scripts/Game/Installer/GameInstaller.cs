using BoardTower.Common.Presentation.Facade;
using BoardTower.Common.Presentation.Presenter;
using BoardTower.Common.Presentation.View.Button;
using BoardTower.Common.Utility;
using BoardTower.Game.Data.DataStore;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.Ports;
using BoardTower.Game.Domain.Repository;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Game.Presentation.Facade;
using BoardTower.Game.Presentation.Presenter;
using BoardTower.Game.Presentation.State;
using BoardTower.Game.Presentation.View;
using BoardTower.Game.Presentation.View.Button;
using BoardTower.Game.Presentation.View.Modal;
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
            // DataStore
            builder.RegisterInstance<MemoryDatabase>(new MemoryDatabase(memoryFile.bytes));
            builder.RegisterInstance<SquareEventTable>(squareEventTable);

            // Entity
            builder.Register<BoardEntity>(Lifetime.Scoped);
            builder.Register<ChessmenEntity>(Lifetime.Scoped);
            builder.Register<GameModalEntity>(Lifetime.Scoped);
            builder.Register<GameStateEntity>(Lifetime.Scoped);
            builder.Register<GemEntity>(Lifetime.Scoped);
            builder.Register<PlyEntity>(Lifetime.Scoped);
            builder.Register<RoundEntity>(Lifetime.Scoped);
            builder.Register<RoundClearGemEntity>(Lifetime.Scoped);

            // Ports
            builder.Register<BoardPorts>(Lifetime.Scoped);
            builder.Register<ChessmenPorts>(Lifetime.Scoped);
            builder.Register<EventPorts>(Lifetime.Scoped);
            builder.Register<FinishPorts>(Lifetime.Scoped);
            builder.Register<HudRootPorts>(Lifetime.Scoped);
            builder.Register<GameModalPorts>(Lifetime.Scoped);
            builder.Register<GameStatePorts>(Lifetime.Scoped);
            builder.Register<MovementPorts>(Lifetime.Scoped);
            builder.Register<TapScreenPorts>(Lifetime.Scoped);

            // Repository
            builder.Register<BoardPatternRepository>(Lifetime.Scoped);
            builder.Register<ChessmenMovementRepository>(Lifetime.Scoped);
            builder.Register<RoundClearRepository>(Lifetime.Scoped);
            builder.Register<RoundPlyRepository>(Lifetime.Scoped);
            builder.Register<SquareEventRepository>(Lifetime.Scoped);

            // UseCase
            builder.Register<BoardUseCase>(Lifetime.Scoped);
            builder.Register<ChessmenUseCase>(Lifetime.Scoped);
            builder.Register<EventUseCase>(Lifetime.Scoped);
            builder.Register<FinishUseCase>(Lifetime.Scoped);
            builder.Register<HudRootUseCase>(Lifetime.Scoped);
            builder.Register<GameModalUseCase>(Lifetime.Scoped);
            builder.Register<GameStateUseCase>(Lifetime.Scoped);
            builder.Register<GemUseCase>(Lifetime.Scoped);
            builder.Register<MovementUseCase>(Lifetime.Scoped);
            builder.Register<PlyUseCase>(Lifetime.Scoped);
            builder.Register<RoundUseCase>(Lifetime.Scoped);
            builder.Register<RoundClearUseCase>(Lifetime.Scoped);
            builder.Register<TapScreenUseCase>(Lifetime.Scoped);

            // State
            builder.Register<BaseGameState, GameClearState>(Lifetime.Scoped);
            builder.Register<BaseGameState, GameEventState>(Lifetime.Scoped);
            builder.Register<BaseGameState, GameFailState>(Lifetime.Scoped);
            builder.Register<BaseGameState, GameFinishState>(Lifetime.Scoped);
            builder.Register<BaseGameState, GameInitState>(Lifetime.Scoped);
            builder.Register<BaseGameState, GameInputState>(Lifetime.Scoped);
            builder.Register<BaseGameState, GameJudgeState>(Lifetime.Scoped);
            builder.Register<BaseGameState, GameSetUpState>(Lifetime.Scoped);

            // Presenter
            builder.UseEntryPoints(Lifetime.Scoped, entryPoints =>
            {
                entryPoints.Add<BoardPresenter>();
                entryPoints.Add<ButtonPresenter>();
                entryPoints.Add<ChessmenPresenter>();
                entryPoints.Add<DeletePresenter>();
                entryPoints.Add<FinishPresenter>();
                entryPoints.Add<HudRootPresenter>();
                entryPoints.Add<GameModalPresenter>();
                entryPoints.Add<GameStatePresenter>();
                entryPoints.Add<GemPresenter>();
                entryPoints.Add<PlyPresenter>();
                entryPoints.Add<RoundPresenter>();
                entryPoints.Add<RoundClearPresenter>();
                entryPoints.Add<TapScreenPresenter>();
                entryPoints.Add<VolumePresenter>();
            });

            // Facade
            builder.Register<BoardFacade>(Lifetime.Scoped);
            builder.Register<ButtonFacade>(Lifetime.Scoped);
            builder.Register<ChessmenFacade>(Lifetime.Scoped);
            builder.Register<DeleteFacade>(Lifetime.Scoped);
            builder.Register<FinishFacade>(Lifetime.Scoped);
            builder.Register<HudRootFacade>(Lifetime.Scoped);
            builder.Register<GameModalFacade>(Lifetime.Scoped);
            builder.Register<GemFacade>(Lifetime.Scoped);
            builder.Register<PlyFacade>(Lifetime.Scoped);
            builder.Register<RoundFacade>(Lifetime.Scoped);
            builder.Register<RoundClearFacade>(Lifetime.Scoped);
            builder.Register<TapScreenFacade>(Lifetime.Scoped);
            builder.Register<VolumeFacade>(Lifetime.Scoped);

            // View
            builder.RegisterFindObjectsByType<BaseButtonView>();
            builder.RegisterFindObjectsByType<GameModalButtonView>();
            builder.RegisterFindObjectsByType<BaseGameModalView>();
            builder.RegisterComponentInHierarchy<BoardView>();
            builder.RegisterComponentInHierarchy<ChessmenView>();
            builder.RegisterComponentInHierarchy<DeleteView>();
            builder.RegisterComponentInHierarchy<FinishView>();
            builder.RegisterComponentInHierarchy<HudRootView>();
            builder.RegisterComponentInHierarchy<GemView>();
            builder.RegisterComponentInHierarchy<PlyView>();
            builder.RegisterComponentInHierarchy<RoundView>();
            builder.RegisterComponentInHierarchy<RoundClearGemCountView>();
            builder.RegisterComponentInHierarchy<RoundMaxNumView>();
            builder.RegisterComponentInHierarchy<TapScreenView>();
            builder.RegisterComponentInHierarchy<VolumeView>();
        }
    }
}