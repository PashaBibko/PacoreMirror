using System.Collections.Concurrent;
using PashaBibko.Pacore.Attributes;
using UnityEngine;
using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Debug = UnityEngine.Debug;

namespace PashaBibko.Pacore.Threading
{
    [CreateInstanceOnStart] public class ThreadDispatcher : MonoBehaviour
    {
        private static ConcurrentQueue<IEnumerator> MainThreadMultistepQueue { get; } = new();
        private static ConcurrentQueue<Action> MainThreadImmediateQueue { get; } = new();
        private static ConcurrentQueue<Action> BackgroundQueue { get; } = new();

        private static SemaphoreSlim Semaphore { get; } = new(initialCount: 4);
        private static int IsBackgroundProcessing; // Pseudo boolean

        private static IEnumerator ActiveMultistepRoutine { get; set; }
        private static ThreadDispatcher Instance;
        
        private const long MAIN_THREAD_MS_MAX = 5;
        
        private void Awake()
        {
            /* Makes sure there is only one instance */
            if (Instance is not null)
            {
                Debug.LogError($"Cannot have multiple instances of [{nameof(ThreadDispatcher)}]");
                return;
            }

            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null; // Allows the Dispatcher to be destroyed
        }

        public static void QueueMultistep(IEnumerator routine) => MainThreadMultistepQueue.Enqueue(routine);
        public static void QueueImmediate(Action action) => MainThreadImmediateQueue.Enqueue(action);

        public static void QueueBackground(Action action)
        {
            /* Adds to the queue and runs if there are no active threads */
            BackgroundQueue.Enqueue(action);
            TriggerBackgroundProcessing();
        }

        private static void TriggerBackgroundProcessing()
        {
            /* Makes sure there are not too many threads queued */
            if (Interlocked.Exchange(ref IsBackgroundProcessing, 1) != 1)
            {
                Task.Run(ProcessBackgroundQueue);
            }
        }

        private static async Task ProcessBackgroundQueue()
        {
            /* Empties the queue of all tasks */
            while (BackgroundQueue.TryDequeue(out Action task))
            {
                await Semaphore.WaitAsync();
                _ = Task.Run(() =>
                {
                    ThreadSafe.Try
                    (
                        action: task,
                        final: () => Semaphore.Release()
                    );
                });
            }
            
            /* Cleans up to allow for future procession */
            Interlocked.Exchange(ref IsBackgroundProcessing, 0);
            if (!BackgroundQueue.IsEmpty) // Items may be queued during cleanup
            {
                TriggerBackgroundProcessing();
            }
        }

        private void Update()
        {
            /* Runs the Actions in the immediate queue */
            Stopwatch sw = Stopwatch.StartNew(); // Used to make sure not too much processing is done in one go
            while (MainThreadImmediateQueue.TryDequeue(out Action current) && sw.ElapsedMilliseconds < MAIN_THREAD_MS_MAX)
            {
                current.Invoke();
            }
            
            /* Runs the multistep actions (if there is still time) */
            while (sw.ElapsedMilliseconds < MAIN_THREAD_MS_MAX)
            {
                /* Gets a new routine if there is none active */
                if (ActiveMultistepRoutine == null)
                {
                    if (!MainThreadMultistepQueue.TryDequeue(out IEnumerator next))
                    {
                        return; // There is none left so we can return early
                    }
                    
                    ActiveMultistepRoutine = next;
                }
                
                /* Runs the next step in the routine */
                if (!ActiveMultistepRoutine.MoveNext())
                {
                    ActiveMultistepRoutine = null; // [MoveNext() -> false] means the routine has ended
                }
            }
        }
    }
}