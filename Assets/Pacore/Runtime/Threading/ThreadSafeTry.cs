using System.Runtime.CompilerServices;
using UnityEngine;
using System;

namespace PashaBibko.Pacore.Threading
{
    public static partial class ThreadSafe
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Try(Action action, Action final = null)
        {
            try { action(); }

            /* Makes sure any exceptions are caught and logged properly */
            catch (Exception ex)
            {
                ThreadDispatcher.QueueImmediate(() => Debug.Log($"Exception: [{ex.Message}]"));
                throw;
            }

            finally { final?.Invoke(); }
        }
    }
}
