using UnityEngine.Scripting;
using System;

namespace PashaBibko.Pacore.Attributes
{
    [Preserve, AttributeUsage(AttributeTargets.Class)]
    public class CreateInstanceOnStartAttribute : Attribute { }
}
