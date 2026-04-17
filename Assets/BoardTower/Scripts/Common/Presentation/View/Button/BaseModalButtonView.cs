using System;
using BoardTower.Common.Application;
using R3;
using R3.Triggers;
using UnityEngine;

namespace BoardTower.Common.Presentation.View.Button
{
    public abstract class BaseModalButtonView<T> : MonoBehaviour where T : Enum
    {
        [SerializeField] protected UnityEngine.UI.Button button = default;
        [SerializeField] protected T modalType = default(T);
        [SerializeField] protected Fade fadeType = default;

        protected abstract BaseModalVO<T> modal { get; }

        public Observable<BaseModalVO<T>> pointerDown => button
            .OnPointerDownAsObservable()
            .Select(_ => modal);
    }
}