using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ------------------------------------------------------------
// VirrVarr.LayerAsset
// ------------------------------------------------------------
// Asset type that allows data driving checks for a single
// layer (user defined or engine defined)
// ------------------------------------------------------------

namespace VirrVarr
{
    [CreateAssetMenu(fileName = "NewLayer", menuName = "VirrVarr/Layer")]
    public class LayerAsset : ScriptableObject
    {
        public string layerName;
        private int layerID;

        /// <summary>
        /// Check if a game object is in the layer the asset represents
        /// </summary>
        /// <param name="gameObject">The game object to check</param>
        /// <returns>true if the game object layer matches, false if not</returns>
        public bool IsGameObjectInLayer( GameObject gameObject )
        {
            return gameObject.layer == layerID;
        }

        private void OnEnable()
        {
            layerID = LayerMask.NameToLayer(layerName);
            if(layerID == -1)
            {
                Logging.LogWarn(this, "Layer with name '{0}' could not be found. Ensure you have a layer with this name.", layerName);
            }
        }
    }
}