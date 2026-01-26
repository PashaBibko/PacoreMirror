using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PashaBibko.Pacore.Data
{
    public static class PacoreDataLoader
    {
        private const string PATH_FROM_RESOURCES = "PacoreData";
        private const string BASE_PATH = "Pacore/Resources/PacoreData";
        public static string FullResourcesPath => Path.Join(Application.dataPath, BASE_PATH);
        public static string ResourcesPath => Path.Join("Assets", BASE_PATH);

        public static void LoadAndApplyStaticValues()
        {
            /* Loads the static values from the serialized file */
            string filename = Path.Join(PATH_FROM_RESOURCES, "StaticFieldValues");
            StaticFieldValues asset = Resources.Load<StaticFieldValues>(filename);
            if (asset is null)
            {
                Debug.LogWarning("Could not find StaticFieldValues asset to load");
                return;
            }

            /* Applies the values to the static fields */
            foreach (StaticFieldValues.TypeFieldValues values in asset.StoredValues)
            {
                /* Makes sure the type is valid before trying to apply values */
                Type type = Type.GetType($"{values.Typename}, Assembly-CSharp");
                if (type is null)
                {
                    Debug.LogWarning($"Could not find type [{values.Typename}]");
                    continue;
                }
                
                ApplyStaticValuesToType(type, values.Fields);
            }
        }

        private static void ApplyStaticValuesToType(Type type, StaticFieldValues.FieldValue[] fields)
        {
            const BindingFlags FLAGS = BindingFlags.NonPublic |
                                       BindingFlags.Public |
                                       BindingFlags.Static;
            
            /* Converts the fields to a dictionary for easier access */
            Dictionary<string, string> values = fields.ToDictionary
            (
                f => f.Name,
                f => f.SerializedValue
            );
            
            /* Fetches the static field values of the current type and applies the values */
            FieldInfo[] typeFields = type.GetFields(FLAGS);
            foreach (FieldInfo field in typeFields)
            {
                string name = field.Name;
                if (values.TryGetValue(name, out string serialized))
                {
                    object value = ConvertFromString(field.FieldType, serialized);
                    field.SetValue(null, value);
                    
                    Debug.Log($"Set [{field.Name}] to [{value}]");
                }
                else
                {
                    Debug.LogWarning($"Could not find field [{name}]");
                }
            }
        }

        private static object ConvertFromString(Type type, string serialized)
        {
            if (string.IsNullOrEmpty(serialized))
                return type.IsValueType ? Activator.CreateInstance(type) : null;

            if (type.IsEnum)
                return Enum.Parse(type, serialized, ignoreCase: true);

            if (type == typeof(Vector2))
                return JsonUtility.FromJson<Vector2>(serialized);

            if (type == typeof(Vector3))
                return JsonUtility.FromJson<Vector3>(serialized);

            if (type == typeof(Vector4))
                return JsonUtility.FromJson<Vector4>(serialized);

            if (type == typeof(Color))
                return JsonUtility.FromJson<Color>(serialized);

            if (type == typeof(string))
                return serialized;

            if (type.IsPrimitive || type == typeof(decimal))
                return Convert.ChangeType(serialized, type);

            return type.IsSerializable
                ? JsonUtility.FromJson(serialized, type)
                : throw new NotSupportedException($"Cannot convert {type} to type {type.FullName}");
        }
    }
}
