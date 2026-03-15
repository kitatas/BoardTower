using System;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Game.Presentation.Facade;
using R3;
using VContainer.Unity;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class PlyPresenter : IStartable, IDisposable
    {
        private readonly PlyUseCase _plyUseCase;
        private readonly PlyFacade _plyFacade;
        private readonly CompositeDisposable _disposable;

        public PlyPresenter(PlyUseCase plyUseCase, PlyFacade plyFacade)
        {
            _plyUseCase = plyUseCase;
            _plyFacade = plyFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _plyUseCase.ply
                .DistinctUntilChanged()
                .Pairwise()
                .Subscribe(_plyFacade.Render)
                .AddTo(_disposable);

            _plyUseCase.plyMax
                .DistinctUntilChanged()
                .Pairwise()
                .Subscribe(_plyFacade.RenderMax)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}