using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace VirrVarr.Editor
{
    public static class EditorUtilities
    {
        public static string GetSelectionDirectoryPath()
        {
            foreach(var selectedObj in Selection.GetFiltered<Object>(SelectionMode.Assets))
            {
                string selectedObjPath = AssetDatabase.GetAssetPath(selectedObj);

                if(System.IO.Directory.Exists(selectedObjPath))
                {
                    return selectedObjPath;
                }
                else if(System.IO.File.Exists(selectedObjPath))
                {
                    return System.IO.Path.GetDirectoryName(selectedObjPath);
                }

            }
            
            return "Assets";
        }

    }
}
