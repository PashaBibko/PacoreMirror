using UnityEngine;
using System;

namespace PashaBibko.Pacore.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Field)]
    public sealed class DetectInspectorChangesAttribute : PropertyAttribute
    {
        public string ActionName { get; }

        public DetectInspectorChangesAttribute(string function)
        {
            ActionName = function;
        }
    }
}
