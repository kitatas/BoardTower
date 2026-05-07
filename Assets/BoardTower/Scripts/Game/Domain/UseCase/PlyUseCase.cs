using System;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.Repository;
using R3;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class PlyUseCase : IDisposable
    {
        private readonly PlyEntity _plyEntity;
        private readonly RoundPlyRepository _roundPlyRepository;
        private readonly ReactiveProperty<int> _ply;
        private readonly ReactiveProperty<int> _plyMax;

        public PlyUseCase(PlyEntity plyEntity, RoundPlyRepository roundPlyRepository)
        {
            _plyEntity = plyEntity;
            _roundPlyRepository = roundPlyRepository;
            _ply = new ReactiveProperty<int>(0);
            _plyMax = new ReactiveProperty<int>(0);
        }

        public Observable<int> ply => _ply;
        public Observable<int> plyMax => _plyMax;

        public void SetUp(int round)
        {
            var plyCount = round > 0 ? _roundPlyRepository.Find(round)?.plyCount ?? 0 : 0;
            _plyEntity.SetUp(plyCount);
            _ply.Value = _plyEntity.value;
            _plyMax.Value = _plyEntity.maxValue;
        }

        public void Add(int value)
        {
            _plyEntity.Add(value);
            _ply.Value = _plyEntity.value;
        }

        public void Decrease()
        {
            _plyEntity.Subtract(1);
            _ply.Value = _plyEntity.value;
        }

        public bool IsZero() => _plyEntity.value == 0;

        void IDisposable.Dispose()
        {
            _ply?.Dispose();
            _plyMax?.Dispose();
        }
    }
}