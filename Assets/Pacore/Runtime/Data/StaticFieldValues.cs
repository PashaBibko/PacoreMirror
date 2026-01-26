using UnityEngine;
using System;

namespace PashaBibko.Pacore.Data
{
    public class StaticFieldValues : ScriptableObject
    {
        [Serializable] public struct FieldValue
        {
            public string SerializedValue;
            public string Name;
        }

        [Serializable] public struct TypeFieldValues
        {
            public string Typename;
            public FieldValue[] Fields;
        }

        public TypeFieldValues[] StoredValues;
    }
}
