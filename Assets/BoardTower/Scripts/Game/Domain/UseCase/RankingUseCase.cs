using System.Threading;
using BoardTower.Common.Domain.Repository;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.Ports;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class RankingUseCase
    {
        private readonly RankingPorts _rankingPorts;
        private readonly PlayFabRepository _playFabRepository;

        public RankingUseCase(RankingPorts rankingPorts, PlayFabRepository playFabRepository)
        {
            _rankingPorts = rankingPorts;
            _playFabRepository = playFabRepository;
        }

        public IAsyncSubscriber<ScoreRankingVO> scoreRanking => _rankingPorts.scoreRankingSubscriber;

        public async UniTask PublishScoreRankingAsync(CancellationToken token)
        {
            var ranking = await _playFabRepository.GetScoreRankingAsync(token);
            await _rankingPorts.PublishScoreRankingAsync(new ScoreRankingVO(ranking.entries), token);
        }
    }
}