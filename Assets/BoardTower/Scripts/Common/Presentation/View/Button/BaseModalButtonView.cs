using System;
using BoardTower.Common.Application;
using R3;
using UnityEngine;

namespace BoardTower.Common.Presentation.View.Button
{
    public abstract class BaseModalButtonView<T> : BaseButtonView where T : Enum
    {
        [SerializeField] protected T modalType = default(T);
        [SerializeField] protected Fade fadeType = default;

        protected abstract BaseModalVO<T> modal { get; }

        public Observable<BaseModalVO<T>> clickModal => button
            .OnClickAsObservable()
            .Select(_ => modal);
    }
}