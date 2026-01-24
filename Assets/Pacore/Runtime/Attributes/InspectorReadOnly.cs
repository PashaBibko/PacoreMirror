using JetBrains.Annotations;
using UnityEngine;
using System;

namespace PashaBibko.Pacore.Attributes
{
#if UNITY_EDITOR
    [MeansImplicitUse, AttributeUsage(validOn: AttributeTargets.Field)]
    public sealed class InspectorReadOnlyAttribute : PropertyAttribute
    {
        public string Name { get; }

        public InspectorReadOnlyAttribute(string name = null)
        {
            Name = name;
        }
    }
#else // #if UNITY_EDITOR
    [MeansImplicitUse, AttributeUsage(validOn: AttributeTargets.Field)]
    public sealed class InspectorReadOnlyAttribute : Attribute
    {
        public InspectorReadOnlyAttribute(string _name = null) { }
    }
#endif // UNITY_EDITOR
}
