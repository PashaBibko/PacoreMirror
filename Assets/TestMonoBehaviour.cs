using PashaBibko.Pacore.Shared.Attributes;
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
        Debug.Log($"Test value [{Test}]");
    }
    
    [InspectorCallable("Other Test button")] public void DontLogTestValue()
    {
        Debug.Log("Test");
    }
}
