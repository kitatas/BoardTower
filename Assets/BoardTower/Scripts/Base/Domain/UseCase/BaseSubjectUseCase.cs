using System;
using BoardTower.Base.Data.Entity;
using R3;

namespace BoardTower.Base.Domain.UseCase
{
    public abstract class BaseSubjectUseCase<T> : IDisposable
    {
        protected readonly BaseEntity<T> _entity;
        protected readonly BehaviorSubject<T> _subject;

        public BaseSubjectUseCase(BaseEntity<T> entity)
        {
            _entity = entity;
            _subject = new BehaviorSubject<T>(_entity.value);
        }

        public virtual Observable<T> subject => _subject;

        public virtual void Set(T t)
        {
            _entity.Set(t);
            _subject?.OnNext(_entity.value);
        }

        public void Dispose()
        {
            _subject?.Dispose();
        }
    }
}