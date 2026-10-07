using System.Collections.Generic;
using System.Reflection;
using BoardTower.Common.Application;
using BoardTower.Common.Data.DataStore;
using UnityEngine;

namespace BoardTower.Tests.EditMode.TestHelpers
{
    /// <summary>
    /// SoundRepository 構築用に BgmData / SeData / Table を生成するテスト用ヘルパー。
    /// 生成した ScriptableObject は呼び出し側で DestroyImmediate すること。
    /// </summary>
    internal static class SoundTableHelper
    {
        public static BgmData CreateBgmData(BgmType type)
        {
            var data = ScriptableObject.CreateInstance<BgmData>();
            var field = typeof(BgmData).GetField("bgmType", BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(data, type);
            return data;
        }

        public static SeData CreateSeData(SeType type)
        {
            var data = ScriptableObject.CreateInstance<SeData>();
            var field = typeof(SeData).GetField("seType", BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(data, type);
            return data;
        }

        // BaseTable<T> の list は private のため、リフレクションで差し替える
        public static void SetTableList<T>(BaseTable<T> table, List<T> items) where T : ScriptableObject
        {
            var field = typeof(BaseTable<T>).GetField("list", BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(table, items);
        }
    }
}
