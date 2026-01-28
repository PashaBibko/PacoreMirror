using JetBrains.Annotations;
using UnityEngine;
using System;

namespace PashaBibko.Pacore.Attributes
{
    [MeansImplicitUse, AttributeUsage(validOn: AttributeTargets.Field)]
    public sealed class DetectInspectorChangesAttribute : PropertyAttribute
    {
        public string ActionName { get; }

        public DetectInspectorChangesAttribute([NotNull] string function)
        {
            ActionName = function;
        }
    }
}
