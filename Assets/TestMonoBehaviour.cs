using System.Threading;
using PashaBibko.Pacore.Attributes;
using PashaBibko.Pacore.DevTools;
using UnityEngine;

[CreateInstanceOnStart] public class TestMonoBehaviour : MonoBehaviour
{
    [InspectorReadOnly, SerializeField] private GameObject go;

    [MonoScript(inherited: typeof(MonoBehaviour))] public string Spawnable;
    
    [DetectInspectorChanges("OnTestChange")]
    public int Test;

    private void OnTestChange() => LogTestValue();

    private void Update()
    {
        using (CodeProfiler.Start("Test Snippet"))
        {
            SpinWait sw = new();
            int count = Random.Range(1, 50);

            for (int i = 0; i < count; i++)
            {
                sw.SpinOnce();
            }
        }
    }

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
