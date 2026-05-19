using System;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.Repository;
using R3;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class ScoreUseCase : IDisposable
    {
        private readonly GemComboEntity _gemComboEntity;
        private readonly PickRelicEntity _pickRelicEntity;
        private readonly RoundEntity _roundEntity;
        private readonly ScoreEntity _scoreEntity;
        private readonly ScoreRateRepository _scoreRateRepository;
        private readonly ReactiveProperty<int> _score;

        public ScoreUseCase(GemComboEntity gemComboEntity, PickRelicEntity pickRelicEntity, RoundEntity roundEntity,
            ScoreEntity scoreEntity, ScoreRateRepository scoreRateRepository)
        {
            _gemComboEntity = gemComboEntity;
            _pickRelicEntity = pickRelicEntity;
            _roundEntity = roundEntity;
            _scoreEntity = scoreEntity;
            _scoreRateRepository = scoreRateRepository;
            _score = new ReactiveProperty<int>(0);
        }

        public ReadOnlyReactiveProperty<int> score => _score;

        public void Init()
        {
            _scoreEntity.Reset();
            _score.Value = _scoreEntity.value;
        }

        public void ApplyGemScore(int gemNum, bool isClear)
        {
            var roundRate = _scoreRateRepository.FindRoundGemRate(_roundEntity.value);
            var comboRate = _scoreRateRepository.FindGemComboRate(_gemComboEntity.value);
            var relicEffect = _pickRelicEntity.effect;
            var unitRelicRate = _scoreRateRepository.FindGemUnitRelicRate(relicEffect.gemUnitRateNum);
            var rate = roundRate.value * comboRate.value * unitRelicRate.value;
            if (isClear && relicEffect.isOverflowRoundGem) rate *= ScoreConfig.OVERFLOW_ROUND_GEM_RATE;
            var value = Mathf.CeilToInt(ScoreConfig.BASE_GEM_VALUE * rate) * gemNum;
            Add(value);
        }

        public void ApplyRideOnSquare(SquareEventType type)
        {
            if (type is SquareEventType.Collapse) ApplyRideOnCollapseScore();
        }

        private void ApplyRideOnCollapseScore()
        {
            var seed = Random.Range(0, 100);
            if (seed > RelicConfig.ADDITION_THRESHOLD) return;

            var relicEffect = _pickRelicEntity.effect;
            var value = ScoreConfig.BASE_RIDE_ON_COLLAPSE_VALUE * relicEffect.rideOnCollapseNum;
            Add(value);
        }

        public void ApplyRoundClearScore()
        {
            var roundRate = _scoreRateRepository.FindRoundClearRate(_roundEntity.value);
            var relicEffect = _pickRelicEntity.effect;
            var clearRelicRate = _scoreRateRepository.FindRoundClearRelicRate(relicEffect.roundClearRateNum);
            var rate = roundRate.value * clearRelicRate.value;
            var value = Mathf.CeilToInt(ScoreConfig.BASE_ROUND_CLEAR_VALUE * rate);
            Add(value);
        }

        private void Add(int value)
        {
            _scoreEntity.Add(value);
            _score.Value = _scoreEntity.value;
        }

        void IDisposable.Dispose()
        {
            _score?.Dispose();
        }
    }
}