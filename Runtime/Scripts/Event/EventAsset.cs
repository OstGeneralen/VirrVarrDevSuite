using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirrVarr
{
    [CreateAssetMenu( fileName = "New Event", menuName = "VirrVarr/Event" )]
    public class EventAsset : ScriptableObject
    {
        private readonly List<UnityEngine.Events.UnityAction> subscriptions = new List<UnityEngine.Events.UnityAction>();

        public void Subscribe(UnityEngine.Events.UnityAction callbackAction)
        {
            subscriptions.Add(callbackAction);
        }

        public void Unsubscribe( UnityEngine.Events.UnityAction usedCallbackAction )
        {
            subscriptions.Remove(usedCallbackAction);
        }

        public void Trigger()
        {
            for(int i = 0; i < subscriptions.Count; ++i)
            {
                subscriptions[i].Invoke();
            }
        }
    }
}
