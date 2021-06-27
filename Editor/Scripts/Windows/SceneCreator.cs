using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace VirrVarr.Editor.Windows
{

    public class SceneCreatorWindow : EditorWindow
    {
        string sceneName = "";
        List<string> topLevelObjectNames = new List<string>();
        DirectoryBrowser directoryBrowser = new DirectoryBrowser();

        bool showAdvanced = false;
        string sceneLevelContentName = "=== Level ===";
        string sceneManagersName = "=== Managers ===";

        void InitializeTopLevelObjectNames()
        {
            topLevelObjectNames.Add("=== Level ===");
            topLevelObjectNames.Add("=== Lighting ===");
            topLevelObjectNames.Add("=== Managers ===");
        }

        void OnGUI()
        {
            if (topLevelObjectNames.Count == 0)
            {
                InitializeTopLevelObjectNames();
            }

            string selectionPath = EditorUtilities.GetSelectionDirectoryPath();

            titleContent = new GUIContent("Create Scene");
            sceneName = EditorGUILayout.TextField("Scene name:", sceneName);
            EditorGUILayout.TextField("Create in:", selectionPath);

            showAdvanced = EditorGUILayout.Foldout(showAdvanced, "Additional Settings");

            if (showAdvanced)
            {
                ShowAdvancedSettings();
            }

            bool doCreate = GUILayout.Button("Create");

            if (doCreate)
            {
                DoCreateScene(sceneName, selectionPath);
            }
        }

        void ShowAdvancedSettings()
        {
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Root Objects:");
            ListEditor.Display(ref topLevelObjectNames);
            EditorGUILayout.Separator();
        }

        void DoCreateScene(string sceneName, string createPath)
        {
            if (!createPath.EndsWith("/"))
            {
                createPath += "/";
            }

            string fullScenePath = createPath + sceneName + ".unity";

            var createdScene = UnityEditor.SceneManagement.EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.EmptyScene, UnityEditor.SceneManagement.NewSceneMode.Single);

            UnityEditor.SceneManagement.EditorSceneManager.SetActiveScene(createdScene);
            for(int i = 0; i < topLevelObjectNames.Count; ++i)
            {
                new GameObject(topLevelObjectNames[i]);
            }
            CoreSceneObjectBuilder.Create(sceneName);
            UnityEditor.SceneManagement.EditorSceneManager.SaveScene(createdScene, fullScenePath);

            Close();
        }
    }
}