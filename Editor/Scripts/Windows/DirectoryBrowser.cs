using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace VirrVarr.Editor.Windows
{
    public class DirectoryBrowser
    {
        List<string> directoryCache = new List<string>();
        List<int> directoryCacheEntryDepths = new List<int>();

        public void Display()
        {
            if(directoryCache.Count == 0)
            {
                RebuildDirCache();
            }

            for(int directoryCacheEntryIndex = 0; directoryCacheEntryIndex < directoryCache.Count; ++directoryCacheEntryIndex)
            {
                string dirOnlyName = directoryCache[directoryCacheEntryIndex].Substring(directoryCache[directoryCacheEntryIndex].LastIndexOf('/') + 1);
                DisplayDirectory(dirOnlyName, directoryCacheEntryDepths[directoryCacheEntryIndex]);
            }
        }

        void DisplayDirectory( string directoryName, int directoryDepth )
        {
        }

        void RebuildDirCache()
        {
            directoryCache.Add("Assets");
            directoryCacheEntryDepths.Add(0);
            DirCacheRecurse("Assets", 1);
        }

        void DirCacheRecurse( string topLevel, int depth )
        {
            string[] subDirectories = AssetDatabase.GetSubFolders(topLevel);

            foreach(string subDir in subDirectories)
            {
                directoryCache.Add(subDir);
                directoryCacheEntryDepths.Add(depth);
                DirCacheRecurse(subDir, depth + 1);
            }
        }
    }
}
