using UnityEngine.Scripting;
using System;

namespace PashaBibko.Pacore.Shared.Attributes
{
    [Preserve, AttributeUsage(AttributeTargets.Class)]
    public class CreateInstanceOnStartAttribute : Attribute { }
}
