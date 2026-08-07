using System.Threading;
using BoardTower.Game.Application;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Game.Domain.Ports
{
    public sealed class RankingPorts
    {
        public readonly IAsyncSubscriber<ScoreRankingVO> scoreRankingSubscriber;
        private readonly IAsyncPublisher<ScoreRankingVO> _scoreRankingPublisher;

        public RankingPorts(IAsyncSubscriber<ScoreRankingVO> scoreRankingSubscriber,
            IAsyncPublisher<ScoreRankingVO> scoreRankingPublisher)
        {
            this.scoreRankingSubscriber = scoreRankingSubscriber;
            _scoreRankingPublisher = scoreRankingPublisher;
        }

        public UniTask PublishScoreRankingAsync(ScoreRankingVO scoreRanking, CancellationToken token)
        {
            return _scoreRankingPublisher.PublishAsync(scoreRanking, token);
        }
    }
}