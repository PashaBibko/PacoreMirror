using System;
using UnityEngine;

namespace PashaBibko.Pacore.Shared.Attributes
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