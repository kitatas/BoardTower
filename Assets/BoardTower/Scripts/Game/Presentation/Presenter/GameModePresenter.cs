using System;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Game.Presentation.Facade;
using MessagePipe;
using R3;
using VContainer.Unity;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class GameModePresenter : IStartable, IDisposable
    {
        private readonly GameModeUseCase _gameModeUseCase;
        private readonly GameModeFacade _gameModeFacade;
        private readonly CompositeDisposable _disposable;

        public GameModePresenter(GameModeUseCase gameModeUseCase, GameModeFacade gameModeFacade)
        {
            _gameModeUseCase = gameModeUseCase;
            _gameModeFacade = gameModeFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _gameModeUseCase.gameModeTransition
                .Subscribe(_gameModeFacade.FadeAsync)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}