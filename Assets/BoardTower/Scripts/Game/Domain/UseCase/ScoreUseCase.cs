using System;
using R3;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class ScoreUseCase : IDisposable
    {
        private readonly ReactiveProperty<int> _score;

        public ScoreUseCase()
        {
            _score = new ReactiveProperty<int>(0);
        }

        public ReadOnlyReactiveProperty<int> score => _score;

        void IDisposable.Dispose()
        {
            _score?.Dispose();
        }
    }
}