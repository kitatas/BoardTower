using System;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Game.Presentation.Facade;
using MessagePipe;
using R3;
using VContainer.Unity;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class AchievementPresenter : IStartable, IDisposable
    {
        private readonly AchievementUseCase _achievementUseCase;
        private readonly AchievementFacade _achievementFacade;
        private readonly CompositeDisposable _disposable;

        public AchievementPresenter(AchievementUseCase achievementUseCase, AchievementFacade achievementFacade)
        {
            _achievementUseCase = achievementUseCase;
            _achievementFacade = achievementFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _achievementUseCase.achievementContents
                .Subscribe(_achievementFacade.RenderAsync)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}