using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

namespace PashaBibko.Pacore
{
    public static class ClassAttributeCache
    {
        private static Dictionary<Type, List<Type>> AttributeCache { get; set; }

        public static ReadOnlyCollection<Type> GetAttributesOf<T>()
        {
            return AttributeCache.TryGetValue(typeof(T), out List<Type> classes)
                ? classes.AsReadOnly()
                : throw new ArgumentException($"Attribute [{nameof(T)}] is not used by any classes");
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void ScanAllAssemblies()
        {
            /* Fetches all the class types in all loaded assemblies */
            Type[] classes = AppDomain.CurrentDomain.GetAssemblies() // Assembly[]
                .SelectMany(assembly => assembly.GetTypes()) // IEnumerable<Type>
                .Where(type => type.IsClass && !type.IsAbstract) // IEnumerable<Type>
                .ToArray();

            /* Allocates space for the cache */
            AttributeCache = classes // Type[]
                .Where(type => typeof(Attribute).IsAssignableFrom(type)) // IEnumerable<Type>
                .ToDictionary
                (
                    keySelector: t => t,
                    elementSelector: _ => new List<Type>()
                ); // Dictionary<Type, List<Type>>

            /* Finds which attributes are attached to what classes */
            HashSet<Type> seen = new();
            foreach (Type current in classes)
            {
                /* Tracks the seen attributes */
                seen.Clear();
                foreach (object attr in current.GetCustomAttributes(inherit: true))
                {
                    seen.Add(attr.GetType());
                }

                /* Adds the class type to each attribute in the dictionary */
                foreach (Type type in seen)
                {
                    AttributeCache[type].Add(current);
                }
            }
        }
    }
}
