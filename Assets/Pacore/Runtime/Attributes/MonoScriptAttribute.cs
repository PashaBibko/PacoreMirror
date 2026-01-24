using System;
using UnityEngine;

namespace PashaBibko.Pacore.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class MonoScriptAttribute : PropertyAttribute
    {
        public Type InheritedFrom { get; }
        public Type MonoType { get; }

        public MonoScriptAttribute(Type inherited, Type type = null)
        {
            InheritedFrom = inherited;
            MonoType = type;
        }

        public MonoScriptAttribute(Type type = null)
        {
            InheritedFrom = null;
            MonoType = type;
        }
    }
}