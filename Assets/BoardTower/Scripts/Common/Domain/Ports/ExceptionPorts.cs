using System.Threading;
using BoardTower.Common.Application;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Common.Domain.Ports
{
    public sealed class ExceptionPorts
    {
        public readonly IAsyncSubscriber<ExceptionNotifyVO> exceptionSubscriber;
        private readonly IAsyncPublisher<ExceptionNotifyVO> _exceptionPublisher;

        public ExceptionPorts(IAsyncSubscriber<ExceptionNotifyVO> exceptionSubscriber,
            IAsyncPublisher<ExceptionNotifyVO> exceptionPublisher)
        {
            this.exceptionSubscriber = exceptionSubscriber;
            _exceptionPublisher = exceptionPublisher;
        }

        public UniTask PublishExceptionAsync(ExceptionNotifyVO exception, CancellationToken token)
        {
            return _exceptionPublisher.PublishAsync(exception, token);
        }
    }
}