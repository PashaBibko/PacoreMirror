using Unity.Collections.LowLevel.Unsafe;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using Unity.Collections;
using System;

namespace PashaBibko.Pacore.MemUtil
{
    public unsafe struct Unique<T> : IDisposable
        where T : unmanaged
    {
        private T* mPtr;

        public bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => mPtr != null;
        }

        public T Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                DebugOnly_CheckValid();
                return *mPtr;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                DebugOnly_CheckValid();
                *mPtr = value;
            }
        }

        public ref T Ref
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                DebugOnly_CheckValid();
                return ref *mPtr;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Unique<T> Create(T val = default)
        {
            T* ptr = (T*)UnsafeUtility.Malloc
            (
                sizeof(T),
                UnsafeUtility.SizeOf<T>(),
                Allocator.Persistent
            );
            *ptr = val;

            return new Unique<T> { mPtr = ptr };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            if (IsValid)
            {
                UnsafeUtility.Free(mPtr, Allocator.Persistent);
            }
        }

        [Conditional("UNITY_EDITOR")]
        private void DebugOnly_CheckValid()
        {
            if (!IsValid)
            {
                throw new InvalidOperationException
                (
                    $"Owned<{nameof(T)}> was expected to be valid."
                );
            }
        }
    }
}
