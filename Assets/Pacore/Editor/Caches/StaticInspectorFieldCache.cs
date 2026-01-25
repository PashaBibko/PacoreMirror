using PashaBibko.Pacore.Attributes;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace PashaBibko.Pacore.Editor.Caches
{
    public static class StaticInspectorFieldCache
    {
        public struct FieldData
        {
            public FieldInfo Info;
            public object Value;
        }

        private const BindingFlags BINDING_FLAGS =
            BindingFlags.Static |
            BindingFlags.NonPublic |
            BindingFlags.Public;

        private static Dictionary<Type, FieldData[]> Cache { get; } = new();

        public static FieldData[] GetAllFieldsOfType(Type type)
        {
            /* Checks the cache for the type */
            if (Cache.TryGetValue(type, out FieldData[] cached))
            {
                return cached;
            }
            
            /* Else finds all the fields with the attribute */
            FieldInfo[] fields = type.GetFields(BINDING_FLAGS);
            List<FieldData> instances = new();

            foreach (FieldInfo field in fields)
            {
                StaticInspectorFieldAttribute attribute
                    = field.GetCustomAttribute<StaticInspectorFieldAttribute>();

                if (attribute != null)
                {
                    FieldData data = new()
                    {
                        Info = field
                    };
                        
                    instances.Add(data);
                }
            }

            /* Adds the fields to the cache before returning */
            FieldData[] array = instances.ToArray();
            Cache.Add(type, array);
            return array;
        }
    }
}