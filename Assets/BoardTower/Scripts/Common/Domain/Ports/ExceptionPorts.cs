using System.Threading;
using BoardTower.Common.Application;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Common.Domain.Ports
{
    public sealed class ExceptionPorts
    {
        public readonly IAsyncSubscriber<ExceptionNotifyVO> exceptionNotifySubscriber;
        public readonly IAsyncSubscriber<ExceptionActionVO> exceptionActionSubscriber;
        private readonly IAsyncPublisher<ExceptionNotifyVO> _exceptionNotifyPublisher;
        private readonly IAsyncPublisher<ExceptionActionVO> _exceptionActionPublisher;

        public ExceptionPorts(IAsyncSubscriber<ExceptionNotifyVO> exceptionNotifySubscriber,
            IAsyncSubscriber<ExceptionActionVO> exceptionActionSubscriber,
            IAsyncPublisher<ExceptionNotifyVO> exceptionNotifyPublisher,
            IAsyncPublisher<ExceptionActionVO> exceptionActionPublisher)
        {
            this.exceptionNotifySubscriber = exceptionNotifySubscriber;
            this.exceptionActionSubscriber = exceptionActionSubscriber;
            _exceptionNotifyPublisher = exceptionNotifyPublisher;
            _exceptionActionPublisher = exceptionActionPublisher;
        }

        public UniTask PublishExceptionNotifyAsync(ExceptionNotifyVO exceptionNotify, CancellationToken token)
        {
            return _exceptionNotifyPublisher.PublishAsync(exceptionNotify, token);
        }

        public UniTask PublishExceptionActionAsync(ExceptionActionVO exceptionAction, CancellationToken token)
        {
            return _exceptionActionPublisher.PublishAsync(exceptionAction, token);
        }
    }
}