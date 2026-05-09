using System;
using R3;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class GemComboUseCase : IDisposable
    {
        private readonly ReactiveProperty<int> _combo;

        public GemComboUseCase()
        {
            _combo = new ReactiveProperty<int>(0);
        }

        public ReadOnlyReactiveProperty<int> combo => _combo;

        void IDisposable.Dispose()
        {
            _combo?.Dispose();
        }
    }
}