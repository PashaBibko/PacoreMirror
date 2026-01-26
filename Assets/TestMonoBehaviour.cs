using PashaBibko.Pacore.Attributes;
using UnityEngine;

[CreateInstanceOnStart, AllowStaticInspectorFields]
public class TestMonoBehaviour : MonoBehaviour
{
    public int TestValue;
    
    [StaticInspectorField] private static string StaticText;
    [StaticInspectorField] private static string OtherStaticText;
    [StaticInspectorField] private static int StaticInt;

    [InspectorCallable(nameof(PrintStaticFields))]
    public void PrintStaticFields()
    {
        Debug.Log
        (
            $"{nameof(TestValue)}: [{TestValue}] " +
            $"{nameof(StaticText)}: [{StaticText}] " +
            $"{nameof(OtherStaticText)}: [{OtherStaticText}]"
        );
    }
}
