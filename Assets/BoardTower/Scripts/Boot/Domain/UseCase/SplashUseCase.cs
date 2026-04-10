using System.Threading;
using BoardTower.Boot.Application;
using BoardTower.Boot.Domain.Ports;
using BoardTower.Boot.Domain.Repository;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Boot.Domain.UseCase
{
    public sealed class SplashUseCase
    {
        private readonly SplashPorts _splashPorts;
        private readonly SplashRepository _splashRepository;

        public SplashUseCase(SplashPorts splashPorts, SplashRepository splashRepository)
        {
            _splashPorts = splashPorts;
            _splashRepository = splashRepository;
        }

        public IAsyncSubscriber<SplashTransitionVO> transition => _splashPorts.splashTransitionSubscriber;
    }
}