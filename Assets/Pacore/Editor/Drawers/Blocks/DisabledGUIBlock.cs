using UnityEngine;
using System;

namespace PashaBibko.Pacore.Editor.Drawers
{
    public sealed class DisabledGUIBlock : IDisposable
    {
        public DisabledGUIBlock() => GUI.enabled = false;
        public void Dispose() => GUI.enabled = true;
    }
}