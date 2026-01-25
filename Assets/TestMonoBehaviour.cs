using PashaBibko.Pacore.Attributes;
using UnityEngine;

[CreateInstanceOnStart] public class TestMonoBehaviour : MonoBehaviour
{
    public int TestValue;
    
    [StaticInspectorField] private static string StaticText;
    [StaticInspectorField] private static string OtherStaticText;
    [StaticInspectorField] private static int StaticInt;

    [InspectorCallable(nameof(PrintStaticField))] public void PrintStaticField()
    {
        Debug.Log(StaticText);
    }
}
