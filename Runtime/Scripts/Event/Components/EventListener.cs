using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirrVarr
{
    [AddComponentMenu("VirrVarr/Event/EventListener")]
    public class EventListener : MonoBehaviour
    {
        public EventAsset eventType;
        public UnityEngine.Events.UnityEvent onEventTriggered;

        private void OnEnable()
        {
            eventType?.Subscribe(EventTriggeredResponse);
        }


        private void OnDisable()
        {
            eventType?.Unsubscribe(EventTriggeredResponse);
        }

        void EventTriggeredResponse()
        {
            onEventTriggered?.Invoke();
        }
    }
}