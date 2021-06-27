using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ------------------------------------------------------------
// VirrVarr.LayerMaskAsset
// ------------------------------------------------------------
// Asset type that allows data driving checks for a full mask
// of layers (user defined or engine defined)
// ------------------------------------------------------------

namespace VirrVarr
{
    [CreateAssetMenu(fileName = "NewLayerMask", menuName = "VirrVarr/Layer Mask")]
    public class LayerMaskAsset : ScriptableObject
    {
        public string[] layers;
        private int mask;

        /// <summary>
        /// Check if a game object is in a layer that is part of the mask
        /// </summary>
        /// <param name="gameObject">The game object to check</param>
        /// <returns>true if the game object layer is in the mask, false if not</returns>
        public bool MatchGameObjectToMask( GameObject gameObject )
        {
            return (gameObject.layer & mask) != 0;
        }

        private void OnEnable()
        {
            mask = LayerMask.GetMask(layers);
        }
    }
}
