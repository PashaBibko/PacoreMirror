using PashaBibko.Pacore.Attributes;
using UnityEngine;

[CreateInstanceOnStart] public class TestMonoBehaviour : MonoBehaviour
{
    public int TestValue;
    
    [StaticInspectorField] private static string StaticText;
    [StaticInspectorField] private static string OtherStaticText;
    [StaticInspectorField] private static int StaticInt = 0;
    [StaticInspectorField] private static int OtherStaticInt = 0;

    [InspectorCallable(nameof(PrintStaticFields))]
    public void PrintStaticFields()
    {
        Debug.Log
        (
            $"{nameof(StaticInt)}: [{StaticInt}] " +
            $"{nameof(OtherStaticInt)}: [{OtherStaticInt}] " +
            $"{nameof(StaticText)}: [{StaticText}] " +
            $"{nameof(OtherStaticText)}: [{OtherStaticText}]"
        );
    }
}
