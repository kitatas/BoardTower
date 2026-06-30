using System.Threading;
using BoardTower.Common.Domain.Repository;
using BoardTower.Game.Data.Entity;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class SendUseCase
    {
        private readonly ScoreEntity _scoreEntity;
        private readonly PlayFabRepository _playFabRepository;

        public SendUseCase(ScoreEntity scoreEntity, PlayFabRepository playFabRepository)
        {
            _scoreEntity = scoreEntity;
            _playFabRepository = playFabRepository;
        }

        public UniTask SendScoreAsync(CancellationToken token)
        {
            return _playFabRepository.UpdatePlayerScoreAsync(_scoreEntity.value, token);
        }
    }
}