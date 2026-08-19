#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.Tools
{
    public static class StoryTools
    {
        [MenuItem("Tools/Story/Reveal Save Folder")]
        private static void RevealSaveFolder()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }

        [MenuItem("Tools/Story/Reload All Dialogue Assets")]
        private static void ReloadDialogueAssets()
        {
            AssetDatabase.Refresh();
            Debug.Log("[StoryTools] Dialogue assets reloaded.");
        }

        [MenuItem("Tools/Story/Open DialogueManager")]
        private static void OpenDialogueManager()
        {
            var script = AssetDatabase.LoadAssetAtPath<MonoScript>(
                "Assets/Scripts/Core/DialogueManager.cs"
            );
            AssetDatabase.OpenAsset(script);
        }
    }
}
#endif