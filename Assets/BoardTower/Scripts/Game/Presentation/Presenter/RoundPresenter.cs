using System;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Game.Presentation.Facade;
using R3;
using VContainer.Unity;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class RoundPresenter : IStartable, IDisposable
    {
        private readonly GemUseCase _gemUseCase;
        private readonly PlyUseCase _plyUseCase;
        private readonly RoundUseCase _roundUseCase;
        private readonly RoundClearUseCase _roundClearUseCase;
        private readonly RoundFacade _roundFacade;
        private readonly CompositeDisposable _disposable;

        public RoundPresenter(GemUseCase gemUseCase, PlyUseCase plyUseCase, RoundUseCase roundUseCase,
            RoundClearUseCase roundClearUseCase, RoundFacade roundFacade)
        {
            _gemUseCase = gemUseCase;
            _plyUseCase = plyUseCase;
            _roundUseCase = roundUseCase;
            _roundClearUseCase = roundClearUseCase;
            _roundFacade = roundFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _roundUseCase.round
                .DistinctUntilChanged()
                .Pairwise()
                .Subscribe(_roundFacade.Render)
                .AddTo(_disposable);

            _roundUseCase.round
                .Subscribe(x =>
                {
                    _gemUseCase.SetUp();
                    _plyUseCase.SetUp(x);
                    _roundClearUseCase.SetUp(x);
                })
                .AddTo(_disposable);

            _roundFacade.RenderMax((0, RoundConfig.MAX_NUM));
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}