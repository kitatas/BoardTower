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

        public PlyUseCase(PlyEntity plyEntity, RoundPlyRepository roundPlyRepository)
        {
            _plyEntity = plyEntity;
            _roundPlyRepository = roundPlyRepository;
            _ply = new ReactiveProperty<int>(0);
        }

        public Observable<int> ply => _ply;

        public void SetUp(int round)
        {
            var vo = _roundPlyRepository.Find(round);
            _plyEntity.Set(vo.plyCount);
            _ply.Value = _plyEntity.value;
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
        }
    }
}