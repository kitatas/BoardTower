using System;

namespace BoardTower.Base.Data.Entity
{
    public abstract class BaseStateEntity<T> : BaseEntity<T> where T : Enum
    {
    }
}