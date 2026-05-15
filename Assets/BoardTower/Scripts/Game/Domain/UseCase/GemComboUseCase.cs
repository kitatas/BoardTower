using System;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using R3;
using Random = UnityEngine.Random;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class GemComboUseCase : IDisposable
    {
        private readonly GemComboEntity _gemComboEntity;
        private readonly PickRelicEntity _pickRelicEntity;
        private readonly ReactiveProperty<int> _combo;

        public GemComboUseCase(GemComboEntity gemComboEntity, PickRelicEntity pickRelicEntity)
        {
            _gemComboEntity = gemComboEntity;
            _pickRelicEntity = pickRelicEntity;
            _combo = new ReactiveProperty<int>(0);
        }

        public ReadOnlyReactiveProperty<int> combo => _combo;

        public void SetUp()
        {
            _gemComboEntity.Reset();
            _combo.Value = _gemComboEntity.value;
        }

        public void Increment()
        {
            _gemComboEntity.Add(1);
            _combo.Value = _gemComboEntity.value;
        }

        public void Apply(SquareEventType type)
        {
            if (type is SquareEventType.Gem)
            {
                Increment();
            }
            else if (IsComboContinuation())
            {
            }
            else
            {
                SetUp();
            }
        }

        private bool IsComboContinuation()
        {
            var relicEffect = RelicEffectVO.Create(_pickRelicEntity.relicTypes);
            if (relicEffect.isComboContinuation)
            {
                return Random.Range(0, 100) > RelicConfig.CONTINUATION_THRESHOLD;
            }

            return false;
        }

        void IDisposable.Dispose()
        {
            _combo?.Dispose();
        }
    }
}