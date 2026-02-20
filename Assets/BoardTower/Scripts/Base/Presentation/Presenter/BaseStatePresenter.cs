using System;
using System.Collections.Generic;
using System.Threading;
using BoardTower.Base.Domain.UseCase;
using BoardTower.Base.Presentation.State;
using BoardTower.Common.Application;
using BoardTower.Common.Utility;
using Cysharp.Threading.Tasks;
using MessagePipe;
using VContainer.Unity;

namespace BoardTower.Base.Presentation.Presenter
{
    public abstract class BaseStatePresenter<T> : IAsyncStartable, IDisposable where T : Enum
    {
        private readonly BaseStateUseCase<T> _stateUseCase;
        private readonly Dictionary<T, BaseState<T>> _stateMap;
        private readonly AsyncLockLite _locker;
        private IDisposable _subscription;

        public BaseStatePresenter(BaseStateUseCase<T> stateUseCase, IEnumerable<BaseState<T>> states)
        {
            _stateUseCase = stateUseCase;
            _stateMap = new Dictionary<T, BaseState<T>>();
            foreach (var s in states) _stateMap.TryAdd(s.state, s);
            _locker = new AsyncLockLite();
        }

        async UniTask IAsyncStartable.StartAsync(CancellationToken token)
        {
            await UniTask.WhenAll(_stateMap.Values
                .Select(x => x.InitAsync(token)));

            _subscription = _stateUseCase.subscriber
                .Subscribe(async (s, ct) =>
                {
                    T nextState;
                    {
                        using var _ = await _locker.LockAsync(ct);
                        nextState = await RunAsync(s, ct);
                    }

                    await _stateUseCase.PublishAsync(nextState, ct);
                });

            await _stateUseCase.InitAsync(token);
        }

        private async UniTask<T> RunAsync(T state, CancellationToken token)
        {
            try
            {
                if (!_stateMap.TryGetValue(state, out var currentState))
                {
                    throw new QuitExceptionVO(ExceptionConfig.NOT_FOUND_STATE);
                }

                var nextState = state;

                await currentState.EnterAsync(token);
                while (EqualityComparer<T>.Default.Equals(nextState, state))
                {
                    nextState = await currentState.TickAsync(token);
                }
                await currentState.ExitAsync(token);

                return nextState;
            }
            catch (Exception e)
            {
                // TODO: catch exception
                throw;
            }
        }

        void IDisposable.Dispose()
        {
            _subscription?.Dispose();
        }
    }
}