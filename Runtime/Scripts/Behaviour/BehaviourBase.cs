using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ------------------------------------------------------------
// VirrVarr.BehaviourBase
// ------------------------------------------------------------
// Base class for VirrVarr behaviours
// acts as a standard MonoBehaviour with some additional
// utility functions and members.
// ------------------------------------------------------------

namespace VirrVarr
{
    public class BehaviourBase : MonoBehaviour
    {
        // ------------------------------------------------------------
        // Start logic members
        // ------------------------------------------------------------
        public int StartOrderIndex = 0;
        private bool started = false;
        
        private ServiceLocator boundServiceLocator;

        // ------------------------------------------------------------
        // Default MonoBehaviour Interface
        // ------------------------------------------------------------
        private void Start()
        {
            boundServiceLocator = FindServiceLocator();
            boundServiceLocator.GetServiceChecked<StartOrderingService>().RegisterStartOrderIndexBehaviour(StartOrderIndex, this);
        }
        private void Update()
        {
            if (started)
            {
                Behaviour_Update();
            }
        }
        private void FixedUpdate()
        {
            if(started)
            {
                Behaviour_FixedUpdate();
            }
        }

        // ------------------------------------------------------------
        // Setup interface
        // ------------------------------------------------------------
        /// <summary>
        /// Looks for a service locator belonging to the same scene handle as the behaviour.
        /// </summary>
        /// <returns>The found locator or null if not found. Will assert on null</returns>
        private ServiceLocator FindServiceLocator()
        {
            ServiceLocator found = null;
            var allLocators = FindObjectsOfType<ServiceLocator>();

            foreach (var locator in allLocators)
            {
                if(locator.LocatorSceneHandle == BehaviourSceneHandle)
                {
                    found = locator;
                    break;
                }
            }

            if(found == null)
            {
                Logging.LogAssert(this, "No service locator found for scene handle {0}. Ensure there is a service locator in the scene.", BehaviourSceneHandle);
            }

            return found;
        }
        /// <summary>
        /// The implementation function for starting this behaviour.
        /// Do not call this yourself! It's managed by the VirrVarr start ordering functionality.
        /// </summary>
        public void DoStart_Impl()
        {
            Behaviour_Start();
            started = true;
        }

        // ------------------------------------------------------------
        // VirrVarr Behaviour Interface
        // ------------------------------------------------------------
        /// <summary>
        /// Called before the first update.
        /// Will respect start order indexing.
        /// </summary>
        protected virtual void Behaviour_Start()
        {
        }
        /// <summary>
        /// Called every tick after start-up.
        /// Not called if behaviour is disabled.
        /// </summary>
        protected virtual void Behaviour_Update()
        {
        }
        /// <summary>
        /// Called at a constant timing after start-up.
        /// Not called if behaviour is disabled.
        /// </summary>
        protected virtual void Behaviour_FixedUpdate()
        {
        }

        public ServiceLocator serviceLocator { get { return boundServiceLocator; } } // The service locator relevant to this behaviour
        public int BehaviourSceneHandle { get { return gameObject.scene.handle; } } // The scene handle of this behaviour
    }
}
