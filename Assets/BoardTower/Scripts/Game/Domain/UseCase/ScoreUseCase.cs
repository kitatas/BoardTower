using System;
using BoardTower.Game.Data.Entity;
using R3;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class ScoreUseCase : IDisposable
    {
        private readonly ScoreEntity _scoreEntity;
        private readonly ReactiveProperty<int> _score;

        public ScoreUseCase(ScoreEntity scoreEntity)
        {
            _scoreEntity = scoreEntity;
            _score = new ReactiveProperty<int>(0);
        }

        public ReadOnlyReactiveProperty<int> score => _score;

        public void Init()
        {
            _scoreEntity.Reset();
            _score.Value = _scoreEntity.value;
        }

        void IDisposable.Dispose()
        {
            _score?.Dispose();
        }
    }
}