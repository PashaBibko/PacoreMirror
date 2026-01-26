using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PashaBibko.Pacore.Data;
using UnityEditor;
using UnityEngine;

namespace PashaBibko.Pacore.Editor.Data
{
    public static class StaticFieldValuesExtensions
    {
        public static void AddValue(this StaticFieldValues asset, FieldInfo info, object value)
        {
            /* Checks field has a declaring type */
            Type declaringType = info.DeclaringType;
            if (declaringType == null)
            {
                Debug.LogError($"[StaticFieldValues] Cannot add field {info.Name} because it has no declaring type");
                return;
            }

            /* Checks if the type is already contained */
            string typename = declaringType.FullName;
            int tIdx = -1;

            if (asset.StoredValues != null)
            {
                for (int i = 0; i < asset.StoredValues.Length; i++)
                {
                    if (asset.StoredValues[i].Typename == typename)
                    {
                        tIdx = i;
                        break;
                    }
                }
            }

            /* Adds the type if it is not contained */
            if (tIdx == -1)
            {
                StaticFieldValues.TypeFieldValues newType = new()
                {
                    Typename = typename,
                    Fields = Array.Empty<StaticFieldValues.FieldValue>()
                };

                List<StaticFieldValues.TypeFieldValues> types = asset.StoredValues?.ToList() ??
                                                                new List<StaticFieldValues.TypeFieldValues>();
                types.Add(newType);

                asset.StoredValues = types.ToArray();
                tIdx = types.Count - 1; // -1 for 0 based indexing
            }

            /* Checks if the field is already contained in the type */
            string fieldName = info.Name;
            int sIdx = -1;

            Debug.Assert(asset.StoredValues != null, nameof(asset.StoredValues) + " != null");
            for (int i = 0; i < asset.StoredValues[tIdx].Fields.Length; i++)
            {
                if (asset.StoredValues[tIdx].Fields[i].Name == fieldName)
                {
                    sIdx = i;
                    break;
                }
            }

            /* Adds the field if it is not contained */
            if (sIdx == -1)
            {
                StaticFieldValues.FieldValue newValue = new()
                {
                    SerializedValue = value.ToString(),
                    Name = fieldName
                };

                List<StaticFieldValues.FieldValue> fields = asset.StoredValues[tIdx].Fields?.ToList() ??
                                                            new List<StaticFieldValues.FieldValue>();
                fields.Add(newValue);

                asset.StoredValues[tIdx].Fields = fields.ToArray();
            }

            /* Else updates the value */
            else
            {
                asset.StoredValues[tIdx].Fields[sIdx].SerializedValue = value.ToString();
            }

            /* Tells Unity to save the value */
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
        }
    }
}