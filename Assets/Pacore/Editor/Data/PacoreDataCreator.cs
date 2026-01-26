using PashaBibko.Pacore.Data;
using UnityEditor;
using System.IO;
using System.Reflection;

namespace PashaBibko.Pacore.Editor.Data
{
    [InitializeOnLoad] public static class PacoreDataCreator
    {
        private static StaticFieldValues ValuesAsset;
        
        static PacoreDataCreator() // Runs on editor startup or recompile
        {
            /* Creates the folder for the data files */
            if (!Directory.Exists(PacoreDataLoader.FullResourcesPath))
            {
                Directory.CreateDirectory(PacoreDataLoader.FullResourcesPath);
            }
            
            /* Creates the ScriptableObject asset file for StaticFieldValues */
            const string VALUES_NAME = "StaticFieldValues.asset"; // .asset is REQUIRED by unity API
            string valuesPath = Path.Combine(PacoreDataLoader.ResourcesPath, VALUES_NAME);
            if (!File.Exists(valuesPath))
            {
                ValuesAsset = ScriptableObjectGenerator.Create<StaticFieldValues>
                (
                    path: PacoreDataLoader.ResourcesPath,
                    name: VALUES_NAME
                );
            }
            
            /* If one already exists loads it */
            else
            {
                ValuesAsset = AssetDatabase.LoadAssetAtPath<StaticFieldValues>(valuesPath);
            }
        }

        public static void SetStaticFieldValue(FieldInfo field, object value)
            => ValuesAsset.AddValue(field, value);
    }
}
