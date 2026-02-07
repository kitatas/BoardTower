using System;
using BoardTower.Base.Data.Entity;

namespace BoardTower.Base.Domain.UseCase
{
    public abstract class BaseStateUseCase<T> : BaseSubjectUseCase<T> where T : Enum
    {
        protected BaseStateUseCase(BaseStateEntity<T> stateEntity) : base(stateEntity)
        {
        }
    }
}