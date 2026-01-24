using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;
using UnityEngine;

namespace PashaBibko.Pacore.Runtime
{
    public static class ClassAttributeCache
    {
        private static Dictionary<Type, List<Type>> AttributeCache { get; set; }

        public static ReadOnlyCollection<Type> GetAttributesOf<T>()
        {
            return AttributeCache.TryGetValue(typeof(T), out List<Type> attributes) ?
                attributes.AsReadOnly() :
                throw new ArgumentException($"Attribute [{nameof(T)}] is not used by any classes");
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void ScanAllAssemblies()
        {
            /* Fetches all the class types in all loaded assemblies */
            Type[] classes = AppDomain.CurrentDomain.GetAssemblies() // Assembly[]
                .SelectMany(assembly =>
                {
                    try
                    {
                        return assembly.GetTypes();
                    }

                    catch (ReflectionTypeLoadException err)
                    {
                        return err.Types.Where(t => t != null);
                    }
                }) // IEnumerable<Type>
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
            foreach (Type current in classes)
            {
                /* Finds all the attributes of the current class */
                Type[] attached = current // Type
                    .GetCustomAttributes(inherit: true) // object[]
                    .Select(attribute => attribute.GetType()) // IEnumerable<Type>
                    .Distinct().ToArray();
                
                /* Adds the class type to each attribute in the dictionary */
                foreach (Type attribute in attached)
                {
                    AttributeCache[attribute].Add(current);
                }
            }
        }
    }
}