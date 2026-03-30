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
    }
}