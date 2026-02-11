using System;
using System.Collections.Generic;
using System.Threading;
using BoardTower.Base.Domain.UseCase;
using BoardTower.Base.Presentation.State;
using Cysharp.Threading.Tasks;
using MessagePipe;
using VContainer.Unity;

namespace BoardTower.Base.Presentation.Presenter
{
    public abstract class BaseStatePresenter<T> : IAsyncStartable, IDisposable where T : Enum
    {
        protected readonly BaseStateUseCase<T> _stateUseCase;
        protected readonly Dictionary<T, BaseState<T>> _stateMap;
        private IDisposable _disposable;

        public BaseStatePresenter(BaseStateUseCase<T> stateUseCase, IEnumerable<BaseState<T>> states)
        {
            _stateUseCase = stateUseCase;
            _stateMap = new Dictionary<T, BaseState<T>>();
            foreach (var s in states) _stateMap.TryAdd(s.state, s);
        }

        async UniTask IAsyncStartable.StartAsync(CancellationToken token)
        {
            await UniTask.WhenAll(_stateMap.Values
                .Select(x => x.InitAsync(token)));

            _disposable = _stateUseCase.subscriber
                .Subscribe(async (s, ct) =>
                {
                    var state = await ExecAsync(s, ct);
                    await _stateUseCase.PublishAsync(state, ct);
                });

            await _stateUseCase.InitAsync(token);
        }

        private async UniTask<T> ExecAsync(T state, CancellationToken token)
        {
            try
            {
                if (!_stateMap.TryGetValue(state, out var currentState))
                {
                    // TODO: exception
                    throw new Exception();
                }

                var nextState = await currentState.EnterAsync(token);
                while (nextState.Equals(state))
                {
                    nextState = await currentState.TickAsync(token);
                    await UniTask.Yield(PlayerLoopTiming.Update, token);
                }

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
            _disposable?.Dispose();
        }
    }
}