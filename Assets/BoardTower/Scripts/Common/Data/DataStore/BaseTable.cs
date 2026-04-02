using System.Collections.Generic;
using UnityEngine;

namespace BoardTower.Common.Data.DataStore
{
    public abstract class BaseTable<T> : ScriptableObject where T : ScriptableObject
    {
        [SerializeField] private List<T> list = default;

        public List<T> records => list;
    }
}