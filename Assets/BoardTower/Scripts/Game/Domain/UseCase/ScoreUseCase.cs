using System;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.Repository;
using R3;
using UnityEngine;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class ScoreUseCase : IDisposable
    {
        private readonly GemComboEntity _gemComboEntity;
        private readonly RoundEntity _roundEntity;
        private readonly ScoreEntity _scoreEntity;
        private readonly ScoreRateRepository _scoreRateRepository;
        private readonly ReactiveProperty<int> _score;

        public ScoreUseCase(GemComboEntity gemComboEntity, RoundEntity roundEntity, ScoreEntity scoreEntity,
            ScoreRateRepository scoreRateRepository)
        {
            _gemComboEntity = gemComboEntity;
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

        public void ApplyGemScore(int gemNum)
        {
            var roundRate = _scoreRateRepository.FindRoundGemRate(_roundEntity.value);
            var comboRate = _scoreRateRepository.FindGemComboRate(_gemComboEntity.value);
            var rate = roundRate.value * comboRate.value;
            var value = Mathf.CeilToInt(ScoreConfig.BASE_GEM_VALUE * rate) * gemNum;
            Add(value);
        }

        public void ApplyRoundClearScore()
        {
            var roundRate = _scoreRateRepository.FindRoundClearRate(_roundEntity.value);
            var rate = roundRate.value;
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