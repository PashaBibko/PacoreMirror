using PashaBibko.Pacore.Shared.Attributes;
using System.Collections.ObjectModel;
using UnityEngine;
using System;

using Object = UnityEngine.Object;

namespace PashaBibko.Pacore.Runtime
{
    public static class CreateInstanceOnStartLoader
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void CreateAllInstances()
        {
            /* Fetches all the types with the CreateInstanceOnStart attribute */
            ReadOnlyCollection<Type> types =
                ClassAttributeCache.GetAttributesOf<CreateInstanceOnStartAttribute>();

            /* Creates a holder for all the other game objects to not clutter inspector */
            GameObject holder = new("ScriptSpawnedInstances");
            Object.DontDestroyOnLoad(holder); // Stops all the game objects from being destroyed
            
            Transform parent = holder.transform;
            
            /* Creates all the game objects for the selected types */ 
            foreach (Type type in types)
            {
                GameObject go = new(type.Name, type);
                go.transform.SetParent(parent);
            }
        }
    }
}
