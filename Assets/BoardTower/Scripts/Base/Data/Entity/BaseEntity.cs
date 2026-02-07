namespace BoardTower.Base.Data.Entity
{
    public abstract class BaseEntity<T>
    {
        public T value { get; protected set; } = default;

        public virtual void Set(T t) => value = t;
        public virtual bool IsEqual(T t) => value.Equals(t);
    }
}