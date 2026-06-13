using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Toolbars;
using UnityEngine;

namespace BoardTower.Editor
{
    public static class BootScenePlayer
    {
        [MainToolbarElement(
                "Custom/" + nameof(BootScenePlayer),
                defaultDockPosition = MainToolbarDockPosition.Middle),
        ]
        public static MainToolbarElement CreatePlaySceneZeroButton()
        {
            var icon = EditorGUIUtility.IconContent("PlayButton").image as Texture2D;
            var content = new MainToolbarContent(icon, "Play Boot Scene");
            return new MainToolbarButton(content, OnClick);
        }

        private static void OnClick()
        {
            // 1. ビルド設定にシーンが登録されているか確認
            if (EditorBuildSettings.scenes.Length == 0)
            {
                EditorUtility.DisplayDialog("Error", "Not found Build Settings scenes.", "OK");
                return;
            }

            // 2. 編集中のシーンに変更があれば保存を促す
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                return;
            }

            // 3. 再生時の初期シーンをScene 0に固定する
            var scene0Path = EditorBuildSettings.scenes[0].path;
            var scene0Asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scene0Path);

            if (scene0Asset == null)
            {
                EditorUtility.DisplayDialog("Error", "Failed to load Scene 0.", "OK");
                return;
            }

            EditorSceneManager.playModeStartScene = scene0Asset;
            EditorApplication.isPlaying = true;

            // 再生が停止した時に設定を元に戻す
            EditorApplication.playModeStateChanged += ResetPlayModeStartScene;
        }

        private static void ResetPlayModeStartScene(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode)
            {
                EditorSceneManager.playModeStartScene = null;
                EditorApplication.playModeStateChanged -= ResetPlayModeStartScene;
            }
        }
    }
}