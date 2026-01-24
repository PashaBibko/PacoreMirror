using PashaBibko.Pacore.Shared.Attributes;
using UnityEngine;

public class TestMonoBehaviour : MonoBehaviour
{
    [InspectorReadOnly, SerializeField] private GameObject go;
    
    [DetectInspectorChanges("OnTestChange")]
    public int Test;

    private void OnTestChange() => LogTestValue();

    [InspectorCallable("Test button")] public void LogTestValue()
    {
        Debug.Log($"Test value [{Test}]");
    }
    
    [InspectorCallable("Other Test button")] public void DontLogTestValue()
    {
    }
}
