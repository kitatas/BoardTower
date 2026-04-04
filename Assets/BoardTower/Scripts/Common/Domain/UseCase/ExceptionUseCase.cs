using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Domain.Ports;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Common.Domain.UseCase
{
    public sealed class ExceptionUseCase
    {
        private readonly ExceptionPorts _exceptionPorts;

        public ExceptionUseCase(ExceptionPorts exceptionPorts)
        {
            _exceptionPorts = exceptionPorts;
        }

        public IAsyncSubscriber<ExceptionNotifyVO> exception => _exceptionPorts.exceptionSubscriber;

        public UniTask ThrowAsync(ExceptionVO ex, CancellationToken token)
        {
            var notify = ExceptionNotifyVO.Create(ex, Fade.In, ExceptionConfig.FADE_DURATION);
            return _exceptionPorts.PublishExceptionAsync(notify, token);
        }

        public UniTask ThrowRebootAsync(string message, CancellationToken token)
        {
            return ThrowAsync(new RebootExceptionVO(message), token);
        }

        public UniTask ThrowRetryAsync(string message, CancellationToken token)
        {
            return ThrowAsync(new RetryExceptionVO(message), token);
        }

        public UniTask ThrowQuitAsync(string message, CancellationToken token)
        {
            return ThrowAsync(new QuitExceptionVO(message), token);
        }

        public UniTask FadeOutAsync(float duration, CancellationToken token)
        {
            // NOTE: FadeOut時はException不要なのでnull指定
            var notify = ExceptionNotifyVO.Create(null, Fade.Out, duration);
            return _exceptionPorts.PublishExceptionAsync(notify, token);
        }
    }
}