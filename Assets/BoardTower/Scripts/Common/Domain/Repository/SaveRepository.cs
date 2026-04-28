using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Data.DataStore;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BoardTower.Common.Domain.Repository
{
    public sealed class SaveRepository
    {
        private static SaveDTO Load()
        {
            var data = ES3.Load(SaveConfig.ES3_KEY, defaultValue: "");
            return string.IsNullOrEmpty(data)
                ? Create()
                : JsonUtility.FromJson<SaveDTO>(data);
        }

        public async UniTask<SaveVO> LoadAsync(CancellationToken token)
        {
            var dto = await UniTask.RunOnThreadPool(Load, cancellationToken: token);
            return dto.ToVO();
        }

        private static SaveDTO Create()
        {
            var data = new SaveDTO();
            Save(data);
            return data;
        }

        private static void Save(SaveDTO data)
        {
            ES3.Save(SaveConfig.ES3_KEY, JsonUtility.ToJson(data));
        }
    }
}