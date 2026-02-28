using System;
using BoardTower.Game.Data.Entity;
using R3;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class PlyUseCase : IDisposable
    {
        private readonly PlyEntity _plyEntity;
        private readonly ReactiveProperty<int> _ply;

        public PlyUseCase(PlyEntity plyEntity)
        {
            _plyEntity = plyEntity;
            _ply = new ReactiveProperty<int>(0);
        }

        public Observable<int> ply => _ply;

        public void Init()
        {
            // TODO: 仮初期値
            _plyEntity.Set(10);
            _ply.Value = _plyEntity.value;
        }

        public void Increase()
        {
            _plyEntity.Add(1);
            _ply.Value = _plyEntity.value;
        }

        public void Decrease()
        {
            _plyEntity.Subtract(1);
            _ply.Value = _plyEntity.value;
        }

        void IDisposable.Dispose()
        {
            _ply?.Dispose();
        }
    }
}