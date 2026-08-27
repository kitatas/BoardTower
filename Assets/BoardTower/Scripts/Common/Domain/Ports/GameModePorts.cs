using System.Threading;
using BoardTower.Common.Application;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Common.Domain.Ports
{
    public sealed class GameModePorts
    {
        public readonly IAsyncSubscriber<GameModeTransitionVO> gameModeTransitionSubscriber;
        private readonly IAsyncPublisher<GameModeTransitionVO> _gameModeTransitionPublisher;

        public GameModePorts(IAsyncSubscriber<GameModeTransitionVO> gameModeTransitionSubscriber,
            IAsyncPublisher<GameModeTransitionVO> gameModeTransitionPublisher)
        {
            this.gameModeTransitionSubscriber = gameModeTransitionSubscriber;
            _gameModeTransitionPublisher = gameModeTransitionPublisher;
        }

        public UniTask PublishGameModeAsync(GameModeTransitionVO gameModeTransition, CancellationToken token)
        {
            return _gameModeTransitionPublisher.PublishAsync(gameModeTransition, token);
        }
    }
}