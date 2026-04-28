using BoardTower.Common.Application;
using BoardTower.Common.Data.DataStore;
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

        public SaveVO LoadData()
        {
            return Load().ToVO();
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