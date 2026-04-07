using System;
using BoardTower.Common.Application;

namespace BoardTower.Common.Utility
{
    public static class CollectionHelper
    {
        public static T[] CopyShuffle<T>(this T[] array)
        {
            if (array == null) throw new QuitExceptionVO(ExceptionConfig.INVALID_COLLECTION);
            if (array.Length <= 1) return (T[])array.Clone();
    
            var copy = new T[array.Length];
            Array.Copy(array, copy, array.Length);
    
            for (int i = copy.Length - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);
                (copy[i], copy[j]) = (copy[j], copy[i]);
            }
    
            return copy;
        }
    }
}