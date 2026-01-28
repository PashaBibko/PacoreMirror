using UnityEngine;
using System;

namespace PashaBibko.Pacore.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Field)]
    public sealed class InspectorReadOnlyAttribute : PropertyAttribute
    {
        public string Name { get; }

        public InspectorReadOnlyAttribute(string name = null)
        {
            Name = name;
        }
    }
}
