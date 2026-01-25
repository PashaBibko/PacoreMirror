using PashaBibko.Pacore.Attributes;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace PashaBibko.Pacore.Editor.Caches
{
    public static class StaticInspectorFieldCache
    {
        private const BindingFlags BINDING_FLAGS =
            BindingFlags.Static |
            BindingFlags.NonPublic |
            BindingFlags.Public;

        private static Dictionary<Type, FieldInfo[]> Cache { get; } = new();

        public static FieldInfo[] GetAllFieldsOfType(Type type)
        {
            /* Checks the cache for the type */
            if (Cache.TryGetValue(type, out FieldInfo[] fields))
            {
                return fields;
            }
            
            /* Else finds all the fields with the attribute */
            fields = type.GetFields(BINDING_FLAGS);
            List<FieldInfo> instances = new();

            foreach (FieldInfo field in fields)
            {
                StaticInspectorFieldAttribute attribute
                    = field.GetCustomAttribute<StaticInspectorFieldAttribute>();

                if (attribute != null)
                {
                    instances.Add(field);
                }
            }

            /* Adds the fields to the cache before returning */
            FieldInfo[] array = instances.ToArray();
            Cache.Add(type, array);
            return array;
        }
    }
}