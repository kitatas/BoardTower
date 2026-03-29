using System.Threading;
using BoardTower.Common.Application;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Common.Domain.Ports
{
    public sealed class ExceptionPorts
    {
        public readonly IAsyncSubscriber<ExceptionVO> exceptionSubscriber;
        private readonly IAsyncPublisher<ExceptionVO> _exceptionPublisher;

        public ExceptionPorts(IAsyncSubscriber<ExceptionVO> exceptionSubscriber,
            IAsyncPublisher<ExceptionVO> exceptionPublisher)
        {
            this.exceptionSubscriber = exceptionSubscriber;
            _exceptionPublisher = exceptionPublisher;
        }

        public UniTask PublishExceptionAsync(ExceptionVO exception, CancellationToken token)
        {
            return _exceptionPublisher.PublishAsync(exception, token);
        }
    }
}