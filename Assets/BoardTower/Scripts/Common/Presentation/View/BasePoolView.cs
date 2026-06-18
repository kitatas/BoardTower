using UnityEngine;
using UnityEngine.Pool;

namespace BoardTower.Common.Presentation.View
{
    public abstract class BasePoolView<T> : MonoBehaviour where T : MonoBehaviour
    {
        private ObjectPool<T> _pool;

        public ObjectPool<T> pool => _pool ??= new ObjectPool<T>(
            Create,
            OnGet,
            OnRelease,
            OnDestroyItem,
            true,
            10,
            50
        );

        public abstract T Create();

        public virtual void OnGet(T item)
        {
        }

        public virtual void OnRelease(T item)
        {
        }

        public virtual void OnDestroyItem(T item)
        {
            Destroy(item.gameObject);
        }
    }
}