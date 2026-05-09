using System;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Game.Presentation.Facade;
using R3;
using VContainer.Unity;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class GemComboPresenter : IStartable, IDisposable
    {
        private readonly GemComboUseCase _gemComboUseCase;
        private readonly GemComboFacade _gemComboFacade;
        private readonly CompositeDisposable _disposable;

        public GemComboPresenter(GemComboUseCase gemComboUseCase, GemComboFacade gemComboFacade)
        {
            _gemComboUseCase = gemComboUseCase;
            _gemComboFacade = gemComboFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _gemComboUseCase.combo
                .DistinctUntilChanged()
                .Pairwise()
                .Subscribe(_gemComboFacade.Render)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}