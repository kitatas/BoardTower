using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace BoardTower.Base.Presentation.State
{
    public abstract class BaseState<T> where T : Enum
    {
        public abstract T state { get; }

        public virtual async UniTask InitAsync(CancellationToken token)
        {
            await UniTask.Yield(token);
        }

        public virtual async UniTask<T> EnterAsync(CancellationToken token)
        {
            await UniTask.Yield(PlayerLoopTiming.Update, token);
            return state;
        }

        public virtual async UniTask<T> TickAsync(CancellationToken token)
        {
            await UniTask.Yield(PlayerLoopTiming.Update, token);
            return state;
        }
    }
}