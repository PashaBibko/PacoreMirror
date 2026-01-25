using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using System;

namespace PashaBibko.Pacore.Threading
{
    public static partial class ThreadSafe
    {
        private static SynchronizationContext MainThreadContext { get; set; }

        public class IncorrectThreadException : Exception
        {
            public IncorrectThreadException(string message)
                : base(message)
            { }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void CaptureMainThreadContext()
        {
            MainThreadContext = SynchronizationContext.Current;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EnforceMainThread()
        {
            if (SynchronizationContext.Current != MainThreadContext)
            {
                throw new IncorrectThreadException("Main thread function was called on a background thread");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EnforceBackgroundThread()
        {
            if (SynchronizationContext.Current == MainThreadContext)
            {
                throw new IncorrectThreadException("Background thread function was called on the main thread");
            }
        }
    }
}