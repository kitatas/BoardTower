using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace BoardTower.Common.Utility
{
    public static class LifetimeScopeHelper
    {
        public static RegistrationBuilder RegisterFindFirstObjectByType<T>(this IContainerBuilder builder)
            where T : MonoBehaviour
        {
            return builder.RegisterInstance<T>(Object.FindFirstObjectByType<T>());
        }

        public static RegistrationBuilder RegisterFindObjectsByType<T>(this IContainerBuilder builder)
            where T : MonoBehaviour
        {
            return builder.RegisterInstance<IEnumerable<T>>(Object.FindObjectsByType<T>(FindObjectsSortMode.None));
        }
    }
}