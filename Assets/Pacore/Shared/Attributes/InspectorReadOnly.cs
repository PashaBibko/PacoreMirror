using JetBrains.Annotations;
using UnityEngine;
using System;

namespace PashaBibko.Pacore.Shared.Attributes
{
#if UNITY_EDITOR
    [MeansImplicitUse, AttributeUsage(validOn: AttributeTargets.Field)]
    public class InspectorReadOnlyAttribute : PropertyAttribute
    {
        public string Name { get; }

        public InspectorReadOnlyAttribute(string _name = null)
        {
            Name = _name;
        }
    }
#else // #if UNITY_EDITOR
    [MeansImplicitUse, AttributeUsage(validOn: AttributeTargets.Field)]
    public class InspectorReadOnlyAttribute : Attribute
    {
        public InspectorReadOnlyAttribute(string _name = null) { }
    }
#endif // UNITY_EDITOR
}
