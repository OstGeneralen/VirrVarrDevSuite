using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirrVarr.Internal
{
    public static class VirrVarrCoreServiceRegistrator
    {
        public static void RegisterCoreServices( ServiceLocator locator )
        {
            locator.RegisterService<StartOrderingService>();
        }
    }
}
