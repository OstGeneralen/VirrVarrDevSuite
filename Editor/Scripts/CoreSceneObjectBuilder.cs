using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirrVarr.Editor
{
    public static class CoreSceneObjectBuilder
    {
        public static void Create( string sceneName )
        {
            string coreObjectName = string.Format("=== VirrVarr_{0}_CoreObj ===", sceneName);
            var coreObject = new GameObject(coreObjectName);
            coreObject.AddComponent<ServiceLocator>();
        }
    }
}
