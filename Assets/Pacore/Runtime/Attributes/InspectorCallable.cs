using System;

namespace PashaBibko.Pacore.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class InspectorCallableAttribute : Attribute
    {
        public string ButtonName { get; }

        public InspectorCallableAttribute(string name)
        {
            ButtonName = name;
        }
    }
}
