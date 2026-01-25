using System;
using System.Threading;
using PashaBibko.Pacore.Attributes;
using PashaBibko.Pacore.DevTools;
using PashaBibko.Pacore.Threading;
using UnityEngine;

[CreateInstanceOnStart] public class TestMonoBehaviour : MonoBehaviour
{
    [InspectorReadOnly, SerializeField] private GameObject go;

    [MonoScript(inherited: typeof(MonoBehaviour))] public string Spawnable;
    
    [DetectInspectorChanges("OnTestChange")]
    public int Test;

    private void OnTestChange() => LogTestValue();

    [InspectorCallable(nameof(LogSpawnableType))]
    public void LogSpawnableType()
    {
        Debug.Log(Spawnable);
    }

    [InspectorCallable("Test button")] public void LogTestValue()
    {
        try
        {
            ThreadSafe.EnforceBackgroundThread();
        }
        catch (Exception err)
        {
            Debug.LogError(err.Message);
        }
    }
}
