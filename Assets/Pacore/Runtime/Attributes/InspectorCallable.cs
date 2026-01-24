using JetBrains.Annotations;
using System;

namespace PashaBibko.Pacore.Shared.Attributes
{
    [MeansImplicitUse, AttributeUsage(AttributeTargets.Method)]
    public class InspectorCallableAttribute : Attribute
    {
        public string ButtonName { get; }

        public InspectorCallableAttribute(string name)
        {
            ButtonName = name;
        }
    }
}
