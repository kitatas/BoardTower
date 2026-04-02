using System;

namespace BoardTower.Common.Data.Entity
{
    public abstract class BaseStateEntity<T> : BaseEntity<T> where T : Enum
    {
    }
}