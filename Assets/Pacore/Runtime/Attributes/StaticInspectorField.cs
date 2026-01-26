using System;

namespace PashaBibko.Pacore.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Class)]
    public class AllowStaticInspectorFieldsAttribute : Attribute { }

    [AttributeUsage(validOn: AttributeTargets.Field)]
    public class StaticInspectorFieldAttribute : Attribute { }
}