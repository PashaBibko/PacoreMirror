using System;

namespace PashaBibko.Pacore.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Field)]
    public class StaticInspectorFieldAttribute : Attribute { }
}