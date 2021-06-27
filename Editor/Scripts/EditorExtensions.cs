using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace VirrVarr.Editor
{
    internal static class MenuEntries
    {
        private const string Create_ = "VirrVarr/Create/";

        public const string Create_Scene = Create_ + "Scene";
        public const string Create_Script = Create_ + "Script";
    }

    public class EditorExtensions
    {
        [MenuItem(MenuEntries.Create_Scene)]
        public static void CreateScene()
        {
            EditorWindow.GetWindow(typeof(Windows.SceneCreatorWindow));
        }

        [MenuItem(MenuEntries.Create_Script)]
        public static void CreateScript()
        {
            EditorWindow.GetWindow(typeof(Windows.ScriptCreatorWindow));
        }
    }
}
