using PashaBibko.Pacore.Shared.Attributes;
using UnityEngine;

public class TestMonoBehaviour : MonoBehaviour
{
    [InspectorReadOnly, SerializeField] private GameObject go;
    
    [DetectInspectorChanges("OnTestChange")]
    public int Test;

    private void OnTestChange()
    {
        Debug.Log($"New value: {Test}");
    }
}
