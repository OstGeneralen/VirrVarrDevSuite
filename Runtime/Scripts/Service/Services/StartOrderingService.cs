using System.Collections.Generic;

namespace VirrVarr
{
    class StartOrderRegistration
    {
        public int startOrderIndex;
        public List<BehaviourBase> registeredBehaviours;
    }

    public class StartOrderingService : IService
    {
        List<StartOrderRegistration> registeredStartOrderIndexes;
        int numSceneObjects = 0;
        int numRegisteredObjects = 0;
        bool immediateStartEnabled = false;

        /// <summary>
        /// Constructs a new Start Ordering service.
        /// </summary>
        public StartOrderingService()
        {
            registeredStartOrderIndexes = new List<StartOrderRegistration>();
            SweepSceneObjects();
        }
        public void RegisterStartOrderIndexBehaviour( int startOrderIndex, BehaviourBase behaviour )
        {
            int foundIndex = registeredStartOrderIndexes.FindIndex( (StartOrderRegistration reg ) => { return reg.startOrderIndex == startOrderIndex; } );
            numRegisteredObjects++;

            if(foundIndex != -1)
            {
                registeredStartOrderIndexes[foundIndex].registeredBehaviours.Add(behaviour);
            }
            else
            {
                var addReg = new StartOrderRegistration();
                addReg.registeredBehaviours = new List<BehaviourBase>();
                addReg.startOrderIndex = startOrderIndex;
                addReg.registeredBehaviours.Add(behaviour);
                registeredStartOrderIndexes.Add(addReg);
            }

            if(immediateStartEnabled)
            {
                behaviour.DoStart_Impl();
            }

            // All scene objects have registered themselves so we can safely run the start order
            if(numRegisteredObjects >= numSceneObjects)
            {
                RunStartOrder();
            }
        }
        private void RunStartOrder()
        {
            // Sort the registrations from lower -> higher
            registeredStartOrderIndexes.Sort( CompareRegistration );

            for(int regIndex = 0; regIndex < registeredStartOrderIndexes.Count; ++regIndex)
            {
                for(int behaviourIndex = 0; behaviourIndex < registeredStartOrderIndexes[regIndex].registeredBehaviours.Count; ++behaviourIndex)
                {
                    registeredStartOrderIndexes[regIndex].registeredBehaviours[behaviourIndex].DoStart_Impl();
                }
            }

            immediateStartEnabled = true;
        }
        private void SweepSceneObjects()
        {
            var behaviourBaseList = UnityEngine.GameObject.FindObjectsOfType<BehaviourBase>();

            for(int i = 0; i < behaviourBaseList.Length; ++i)
            {
                if(behaviourBaseList[i].BehaviourSceneHandle == OwnerLocator.LocatorSceneHandle)
                {
                    ++numSceneObjects;
                }
            }
        }
        private static int CompareRegistration( StartOrderRegistration a, StartOrderRegistration b )
        {
            if(a.startOrderIndex == b.startOrderIndex)
            {
                return 0;
            }
            else if(a.startOrderIndex < b.startOrderIndex)
            {
                return -1;
            }
            else if(a.startOrderIndex > b.startOrderIndex)
            {
                return 1;
            }

            return 0;
        }
    }
}
