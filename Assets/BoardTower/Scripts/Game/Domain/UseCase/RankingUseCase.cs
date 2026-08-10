using System.Linq;
using System.Threading;
using BoardTower.Common.Data.Entity;
using BoardTower.Common.Domain.Repository;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.Ports;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class RankingUseCase
    {
        private readonly UserEntity _userEntity;
        private readonly RankingPorts _rankingPorts;
        private readonly PlayFabRepository _playFabRepository;

        public RankingUseCase(UserEntity userEntity, RankingPorts rankingPorts, PlayFabRepository playFabRepository)
        {
            _userEntity = userEntity;
            _rankingPorts = rankingPorts;
            _playFabRepository = playFabRepository;
        }

        public IAsyncSubscriber<ScoreRankingVO> scoreRanking => _rankingPorts.scoreRankingSubscriber;

        public async UniTask PublishScoreRankingAsync(CancellationToken token)
        {
            var ranking = await _playFabRepository.GetScoreRankingAsync(token);
            var entries = ranking.entries
                .Select(x => new ScoreRankingEntryVO(x.rank, x.displayName, x.score, _userEntity.IsEqualEntityId(x.entityId)));

            await _rankingPorts.PublishScoreRankingAsync(new ScoreRankingVO(entries), token);
        }
    }
}