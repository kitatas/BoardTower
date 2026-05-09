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
        private readonly RoundEntity _roundEntity;
        private readonly ScoreEntity _scoreEntity;
        private readonly ScoreRateRepository  _scoreRateRepository;
        private readonly ReactiveProperty<int> _score;

        public ScoreUseCase(RoundEntity roundEntity, ScoreEntity scoreEntity, ScoreRateRepository scoreRateRepository)
        {
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
            var rate = _scoreRateRepository.FindGemRate(_roundEntity.value);
            var value = Mathf.CeilToInt(ScoreConfig.BASE_GEM_VALUE * rate.value) * gemNum;
            Add(value);
        }

        public void ApplyRoundClearScore()
        {
            var rate = _scoreRateRepository.FindRoundClearRate(_roundEntity.value);
            var value = Mathf.CeilToInt(ScoreConfig.BASE_ROUND_CLEAR_VALUE * rate.value);
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