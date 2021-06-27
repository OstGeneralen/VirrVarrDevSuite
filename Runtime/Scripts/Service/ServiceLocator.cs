using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirrVarr
{
    public abstract class IService
    {
        public ServiceLocator OwnerLocator { get { return registeredWithLocator; } }
        private ServiceLocator registeredWithLocator;

        public void BindLocatorToService( ServiceLocator locator )
        {
            registeredWithLocator = locator;
        }
    }

    // ------------------------------------------------------------
    // ServiceLocator
    // ------------------------------------------------------------
    // A locator which you can register services with and then
    // get from any VirrVarr.BehaviourBase.
    // ------------------------------------------------------------
    public class ServiceLocator : MonoBehaviour
    {
        public static ServiceLocator FindForGameObject( GameObject go )
        {
            var locatorList = GameObject.FindObjectsOfType<ServiceLocator>();

            for(int locatorIndex = 0; locatorIndex < locatorList.Length; ++locatorIndex)
            {
                if(locatorList[locatorIndex].LocatorSceneHandle == go.scene.handle)
                {
                    return locatorList[locatorIndex];
                }
            }

            return null;
        }

        private Dictionary<System.Type, IService> serviceRegistry;
        public int LocatorSceneHandle { get { if (sceneHandle == -1) { return gameObject.scene.handle; } return sceneHandle; } }
        private int sceneHandle = -1;

        public ServiceLocator()
        {
            serviceRegistry = new Dictionary<System.Type, IService>();
        }

        public void BindCustomSceneHandle( int handle )
        {
            sceneHandle = handle;
        }

        // PUBLIC INTERFACE
        // ------------------------------------------------------------
        /// <summary>
        /// Register a service with the Locator
        /// </summary>
        /// <typeparam name="TServiceType">The service type you want to register. Must be of IService type.</typeparam>
        public void RegisterService<TServiceType>() where TServiceType : IService, new()
        {
            var serviceInstance = new TServiceType();
            serviceInstance.BindLocatorToService(this);
            RegisterService<TServiceType>(serviceInstance);
        }

        /// <summary>
        /// Register a service by providing the instance. Useful for when your service requires constructor arguments.
        /// </summary>
        /// <typeparam name="TServiceType">The service type you want to register. Must be of IService type.</typeparam>
        /// <param name="instansiatedService">The newed instance of the service to be registered.</param>
        public void RegisterService<TServiceType>(TServiceType instansiatedService) where TServiceType : IService, new()
        {
            if (HasService<TServiceType>())
            {
                Logging.LogErr(this, "RegisterService called for service of type '{0}' which was already registered.", GetServiceTypeName<TServiceType>());
                return;
            }

            instansiatedService.BindLocatorToService(this);
            serviceRegistry.Add(GetServiceType<TServiceType>(), instansiatedService);
        }

        /// <summary>
        /// Unregister a service from the Locator.
        /// </summary>
        /// <typeparam name="TServiceType">The service type you want to unregister.</typeparam>
        public void UnregisterService<TServiceType>() where TServiceType : IService, new()
        {
            if(!HasService<TServiceType>())
            {
                Logging.LogWarn(this, "UnregisterService called for service of type '{0}' which was not first registered. Did you forget registering it?", GetServiceTypeName<TServiceType>());
                return;
            }

            serviceRegistry.Remove(GetServiceType<TServiceType>());
        }

        /// <summary>
        /// Get a service from the Locator.
        /// </summary>
        /// <typeparam name="TServiceType">The service type you want to get.</typeparam>
        /// <returns>The service instance or null if service was not registered.</returns>
        public TServiceType GetService< TServiceType >() where TServiceType: IService, new()
        {
            IService foundService;
            if( serviceRegistry.TryGetValue( GetServiceType<TServiceType>(), out foundService ) )
            {
                return (TServiceType)foundService;
            }

            return default(TServiceType);
        }

        /// <summary>
        /// Get a service from the Locator with an assertion if the service did not exist. 
        /// </summary>
        /// <typeparam name="TServiceType">The service type you want to get.</typeparam>
        /// <returns>The service instance.</returns>
        public TServiceType GetServiceChecked< TServiceType >() where TServiceType: IService, new()
        {
            TServiceType foundService = GetService<TServiceType>();
            if(foundService == null)
            {
                Logging.LogAssert(this, "GetServiceChecked called for service of type '{0}' which was not registered.", GetServiceTypeName<TServiceType>());
            }
            return foundService;
        }

        /// <summary>
        /// Check if a service type is registered with the Locator.
        /// </summary>
        /// <typeparam name="TServiceType">The service type to look for.</typeparam>
        /// <returns>true if the service was registered, false if not.</returns>
        public bool HasService<TServiceType>() where TServiceType : IService, new()
        {
            return serviceRegistry.ContainsKey(GetServiceType<TServiceType>());
        }
        // ------------------------------------------------------------

        // PRIVATE INTERFACE
        // ------------------------------------------------------------
        private string GetServiceTypeName<TServiceType>() where TServiceType : IService, new()
        {
            return GetServiceType<TServiceType>().Name;
        }
        private System.Type GetServiceType<TServiceType>() where TServiceType : IService, new()
        {
            return typeof(TServiceType);
        }
        // ------------------------------------------------------------
    }
    // ------------------------------------------------------------
}
