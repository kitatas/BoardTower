using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.Ports;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class TapScreenUseCase
    {
        private readonly TapScreenPorts _tapScreenPorts;

        public TapScreenUseCase(TapScreenPorts tapScreenPorts)
        {
            _tapScreenPorts = tapScreenPorts;
        }

        public IAsyncSubscriber<TapScreenVO> tapScreen => _tapScreenPorts.tapScreenSubscriber;

        public UniTask FadeAsync(Fade fade, CancellationToken token)
        {
            var tap = TapScreenVO.Create(fade);
            return _tapScreenPorts.PublishTapScreenAsync(tap, token);
        }
    }
}