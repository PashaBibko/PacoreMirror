using PashaBibko.Pacore.Attributes;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace PashaBibko.Pacore.Editor.Caches
{
    public class InspectorCallableCache
    {
        public struct AttributeInfo
        {
            public InspectorCallableAttribute Attribute;
            public MethodInfo AttachedMethod;
        }

        private const BindingFlags BINDING_FLAGS =
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.Instance;

        private static Dictionary<Type, AttributeInfo[]> Cache { get; } = new();

        public static AttributeInfo[] GetAllAttributes(Type type)
        {
            /* Checks the cache for the type */
            if (Cache.TryGetValue(type, out AttributeInfo[] attributes))
            {
                return attributes;
            }

            /* Finds all the functions with the attribute */
            MethodInfo[] methods = type.GetMethods(BINDING_FLAGS);
            List<AttributeInfo> buttons = new();

            foreach (MethodInfo method in methods)
            {
                InspectorCallableAttribute attribute = method.GetCustomAttribute<InspectorCallableAttribute>();
                if (attribute != null)
                {
                    AttributeInfo info = new()
                    {
                        Attribute = attribute,
                        AttachedMethod = method,
                    };

                    buttons.Add(info);
                }
            }

            /* Adds it to the cache before returning */
            AttributeInfo[] array = buttons.ToArray();
            Cache.Add(type, array);
            return array;
        }
    }
}