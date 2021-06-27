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

        public StartOrderingService()
        {
            registeredStartOrderIndexes = new List<StartOrderRegistration>();
            SweepSceneObjects();
        }

        /// <summary>
        /// Register a behaviour with its start index.
        /// Note: Start ordering is only respected for scene object.
        /// If start order has been run and a behaviour registers, it will start immediately.
        /// </summary>
        /// <param name="startOrderIndex">The start index of the behaviour</param>
        /// <param name="behaviour">The behaviour itself</param>
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

        /// <summary>
        /// Start all registered behaviours by increasing start index
        /// </summary>
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
        /// <summary>
        /// Sweep the scene for all behaviours in it.
        /// </summary>
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

        /// <summary>
        /// Comparer for start order registrations so that we can order them
        /// </summary>
        /// <param name="a">Compare reg a</param>
        /// <param name="b">Compare reg b</param>
        /// <returns></returns>
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
