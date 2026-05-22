using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace BoardTower.Common.Utility
{
    public static class ResourceHelper
    {
        private static readonly Dictionary<string, AsyncOperationHandle> _handleCaches = new();

        public static Coroutine LoadAsset<T>(this MonoBehaviour self, string path, Action<T> action) where T : Object
        {
            return self.StartCoroutine(LoadCor<T>(path, action));
        }

        public static IEnumerator LoadCor<T>(string path, Action<T> action) where T : Object
        {
            if (_handleCaches.TryGetValue(path, out var cache))
            {
                if (cache.Status == AsyncOperationStatus.Succeeded)
                {
                    action?.Invoke(cache.Result as T);
                    yield break;
                }
                else if (cache.Status == AsyncOperationStatus.Failed)
                {
                    yield break;
                }
            }

            var handle = Addressables.LoadAssetAsync<T>(path);
            yield return handle;

            try
            {
                _handleCaches[path] = handle;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    action?.Invoke(handle.Result);
                }
                else if (handle.Status == AsyncOperationStatus.Failed)
                {
                    var msg = handle.OperationException?.Message ?? "Unknown error";
                    throw new Exception(ZString.Format("{0} load failed: {1}", path, msg));
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}